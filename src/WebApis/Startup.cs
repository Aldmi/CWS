using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using App.Services.Actions;
using App.Services.MessageBroker;
using Autofac;
using AutoMapper;
using Domain.Device;
using Domain.Exchange;
using Domain.Exchange.Enums;
using Domain.InputDataModel.Autodictor.Model;
using Firewall;
using Infrastructure.Background;
using Infrastructure.Background.Abstarct;
using Infrastructure.Dal.Abstract;
using Infrastructure.Produser.WebClientProduser;
using Infrastructure.Transport;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Npgsql;
using Serilog;
using WebApiSwc.AutofacModules;
using WebApiSwc.Extensions;
using WebApiSwc.Hubs;
using WebApiSwc.Settings;
using TimeSpanConverter = WebApiSwc.JsonConverters.TimeSpanConverter;

namespace WebApiSwc
{
    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            Env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            AppConfiguration = builder.Build();
        }


        public IConfiguration AppConfiguration { get; }
        public IHostEnvironment Env { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();

            var loggerSettings = SettingsFactory.GetLoggerConfig(Env, AppConfiguration);
            services.AddSerilogServices(loggerSettings, Env.ApplicationName);

            services.AddTransient(provider => AppConfiguration);

            services.AddMvc()
                .AddControllersAsServices()
                .AddXmlSerializerFormatters()
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.IgnoreNullValues = true;
                    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    o.JsonSerializerOptions.WriteIndented = true;
                    o.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
                });
            
            services.AddOptions();

            services.AddAutoMapper(typeof(Startup));

            services.AddSignalR();

            var corsSettings = SettingsFactory.GetCorsConfig(Env, AppConfiguration);
            services.AddCors(options =>
            {
                // задаём политику CORS, чтобы наше клиентское приложение могло отправить запрос на сервер API
                options.AddPolicy("WebApp", policy =>
                {
                    if (corsSettings?.WebApiOrigins != null && corsSettings.WebApiOrigins.Any())
                    {
                        policy
                            .WithOrigins(corsSettings.WebApiOrigins)
                            .AllowCredentials()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                    else
                    {
                        policy
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                });
                // задаём политику CORS, для клиента SignalR (провайдер ответов)
                options.AddPolicy("SignalR_providerHub", policy =>
                {
                    if (corsSettings?.SignalROrigins != null && corsSettings.SignalROrigins.Any())
                    {
                        policy
                            .WithOrigins(corsSettings.SignalROrigins)
                            .AllowCredentials()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                    else
                    {
                        policy
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                });
            });

            services.AddHttpClient<IHttpClientSupport, HttpClientSupport>();
        }


        public void ConfigureContainer(ContainerBuilder builder)
        {
            try
            {
                var connectionString = SettingsFactory.GetDbConnectionString(Env, AppConfiguration);
                builder.RegisterModule(new RepositoryAutofacModule(connectionString));
                builder.RegisterModule(new EventBusAutofacModule());
                builder.RegisterModule(new ControllerAutofacModule());
                builder.RegisterModule(new MessageBrokerAutofacModule());
                builder.RegisterModule(new BlConfigAutofacModule());

                var inputDataName = AppConfiguration["InputDataModel"];
                switch (inputDataName)
                {
                    case "AdInputType":
                        builder.RegisterModule(new DataProviderAutofacModule<AdInputType>());
                        builder.RegisterModule(new ProduserUnionAutofacModule<AdInputType>());
                        builder.RegisterModule(new BlStorageAutofacModule<AdInputType>());
                        builder.RegisterModule(new BlBuildAutofacModule<AdInputType>());
                        builder.RegisterModule(new MediatorsAutofacModule<AdInputType>());
                        builder.RegisterModule(new ExchangeAutofacModule<AdInputType>());
                        builder.RegisterModule(new DeviceAutofacModule<AdInputType>());
                        builder.RegisterModule(new InputDataAutofacModule<AdInputType>(AppConfiguration.GetSection("MessageBrokerConsumer4InData")));
                        break;

                    case "OtherInputType":
                        throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {       
                Console.WriteLine($"Ошибка Регистрации зависимостей в DI контейнере {ex}");
                throw;
            }
        }


        public void Configure(IApplicationBuilder app,
                              IHostEnvironment env,
                              ILifetimeScope scope,
                              IMapper mapper,
                              ILogger loger)
        {
            //Настрока выдачи ответа для HealthCheck
            var options = new HealthCheckOptions
            {
                ResponseWriter = async (c, r) =>
                {
                    c.Response.ContentType = "application/json";
                    var result = JsonConvert.SerializeObject(new
                    {
                        status = r.Status.ToString(),
                        Version = Program.GetVersion()
                    });
                    await c.Response.WriteAsync(result);
                }
            };
            app.UseHealthChecks("/", options);

            //Проверка настройки маппинга
            try
            {
                mapper.ConfigurationProvider.AssertConfigurationIsValid();  //TODO: новая версия AutoMapper не выдает Exception на текущие настройки маппинга 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            InitializeAsync(scope).Wait();
            ConfigurationBackgroundProcessAsync(app, scope);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //ПОДКЛЮЧИТЬ ФАЕРВОЛ (ФИЛЬТРАЦИЮ ПО IP)
            var firewallConfig = SettingsFactory.GetFirewallConfig(Env, AppConfiguration);
            if (firewallConfig != null)
            {
                app.UseFirewall(
                    FirewallRulesEngine
                        .DenyAllAccess()
                        .ExceptFromIPAddressRanges(firewallConfig.AllowedCidRs)
                        .ExceptFromIPAddresses(firewallConfig.AllowedIPs)
                //.ExceptFromLocalhost()
                );
                loger.Information("Enable firewall !!!!");
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ProviderHub>("/providerHub")
                    .RequireCors("SignalR_providerHub");

                endpoints.MapControllers()
                    .RequireCors("WebApp");

                //endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }


        private void ConfigurationBackgroundProcessAsync(IApplicationBuilder app, ILifetimeScope scope)
        {
            var lifetimeApp = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            ApplicationStarted(lifetimeApp, scope);
            ApplicationStopping(lifetimeApp, scope);
            ApplicationStopped(lifetimeApp, scope);
        }


        private void ApplicationStarted(IHostApplicationLifetime lifetimeApp, ILifetimeScope scope)
        {
            //ЗАПУСК БЕКГРАУНДА ОПРОСА ШИНЫ ДАННЫХ
            scope.Resolve<ConsumerMessageBroker4InputData<AdInputType>>();//перед запуском bg нужно создать ConsumerMessageBroker4InputData
            bool.TryParse(AppConfiguration["MessageBrokerConsumer4InData:AutoStart"], out var autoStart);
            if (autoStart)
            {
                var backgroundName = AppConfiguration["MessageBrokerConsumer4InData:Name"];
                var bgConsumer = scope.ResolveNamed<ISimpleBackground>(backgroundName);
                lifetimeApp.ApplicationStarted.Register(() => bgConsumer.StartAsync(CancellationToken.None));
            }

            //ЗАПУСК БЕКГРАУНДА ОПРОСА УСТРОЙСТВ
            var backgroundServices = scope.Resolve<BackgroundStorage>();
            lifetimeApp.ApplicationStarted.Register(() =>
            {      
                foreach (var back in backgroundServices.Values.Where(bg => bg.AutoStart))
                {
                   back.StartAsync(CancellationToken.None);
                }
            });

            //ЗАПУСК НА ТРАНСПОРТЕ ЦИКЛИЧЕСКОГО ПЕРЕОТКРЫТИЯ УСТРОЙСТВА
            var transportServices = scope.Resolve<TransportStorage>();
            lifetimeApp.ApplicationStarted.Register(async () =>
            {
                List<Task> tasks = new List<Task>();
                foreach (var transport in transportServices.Values)
                {
                    tasks.Add(transport.CycleReOpenedExec());
                }

                await Task.WhenAll();
            });

            //ЗАПУСК НА ОБМЕНЕ ЦИКЛИЧЕСКОГО ОБМЕНА.
            var exchangeServices = scope.Resolve<ExchangeStorage<AdInputType>>();
            lifetimeApp.ApplicationStarted.Register(() =>
            {
                foreach (var exchange in exchangeServices.Values.Select(owned => owned.Value).Where(exch=> exch.CycleBehavior.CycleFuncOption.AutoStartCycleFunc))
                {
                   exchange.CycleBehavior.StartCycleExchange();
                }
            });

            //ПОДПИСКА ДЕВАЙСА.
            var deviceServices = scope.Resolve<DeviceStorage<AdInputType>>();
            lifetimeApp.ApplicationStarted.Register(() =>
            {
                foreach (var device in deviceServices.Values.Select(owned => owned.Value))
                {
                    //ПОДПИСКА НА СОБЫТИЯ ОТ ОБМЕНОВ.
                    device.SubscrubeOnExchangesEvents();
                    //ПОДПИСКА НА СОБЫТИЯ ОТ ПРОДЮССЕРОВ ОТВЕТОВ.
                    device.SubscrubeOnProdusersEvents();
                    //СОБЫТИЯ СМЕНЫ СОСТОЯНИЯ ПОСТУПЛЕНИЯ ВХОДНЫХ ДАННЫХ ДЛЯ ЦИКЛ. ОБМЕНА.
                    device.SubscrubeOnExchangesCycleBehaviorEvents();
                }
            });
        }


        private void ApplicationStopping(IHostApplicationLifetime lifetimeApp, ILifetimeScope scope)
        {
            //ОСТАНОВ БЕКГРАУНДА ОПРОСА ШИНЫ ДАННЫХ
            var backgroundName = AppConfiguration["MessageBrokerConsumer4InData:Name"];
            var bgConsumer = scope.ResolveNamed<ISimpleBackground>(backgroundName);
            lifetimeApp.ApplicationStopping.Register(() => bgConsumer.StopAsync(CancellationToken.None));

            //ОСТАНОВ ЗАПУЩЕННОГО БЕКГРАУНДА ОПРОСА УСТРОЙСТВ
            var backgroundServices = scope.Resolve<BackgroundStorage>();
            lifetimeApp.ApplicationStopping.Register(() =>
            {
                foreach (var back in backgroundServices.Values.Where(bg => bg.IsStarted))
                {
                    back.StopAsync(CancellationToken.None);
                }
            });

            //ОСТАНОВ НА ТРАНСПОРТЕ ЦИКЛИЧЕСКОГО ПЕРЕОТКРЫТИЯ УСТРОЙСТВА
            var transportServices = scope.Resolve<TransportStorage>();
            lifetimeApp.ApplicationStopping.Register(async () =>
            {
                foreach (var transport in transportServices.Values)
                {
                    transport.Dispose();
                }
            });

            //ОСТАНОВ НА ОБМЕНЕ ЦИКЛИЧЕСКОГО ОБМЕНА.
            var exchangeServices = scope.Resolve<ExchangeStorage<AdInputType>>();
            lifetimeApp.ApplicationStopping.Register(() =>
            {
                foreach (var exchange in exchangeServices.Values.Select(owned => owned.Value).Where(exch => exch.CycleBehavior.CycleBehaviorState != CycleBehaviorState.Off))
                {
                     exchange.CycleBehavior.StopCycleExchange();
                }
            });

            //ОТПИСКА ДЕВАЙСА ОТ СОБЫТИЙ
            var deviceServices = scope.Resolve<DeviceStorage<AdInputType>>();
            lifetimeApp.ApplicationStopping.Register(() =>
            {
                foreach (var device in deviceServices.Values.Select(owned => owned.Value))
                {
                    //ОТПИСКА ДЕВАЙСА ОТ СОБЫТИЙ ПУБЛИКУЕМЫХ НА ProduserUnion.
                    device.UnsubscrubeOnExchangesEvents();
                    //ОТПИСКА ОТ СОБЫТИЙ ПРОДЮССЕРОВ ОТВЕТОВ.
                    device.UnsubscrubeOnProdusersEvents();
                    //ОТПИСКА ДЕВАЙСА ОТ СОБЫТИЙ СМЕНЫ СОСТОЯНИЯ ПОСТУПЛЕНИЯ ВХОДНЫХ ДАННЫХ ДЛЯ ЦИКЛ. ОБМЕНА.
                    device.UnsubscrubeOnExchangesCycleBehaviorEvents();
                }
            });
        }


        private void ApplicationStopped(IHostApplicationLifetime lifetimeApp, ILifetimeScope scope)
        {
            lifetimeApp.ApplicationStopped.Register(() => { });
        }


        /// <summary>
        /// Инициализация системы.
        /// </summary>
        private async Task InitializeAsync(ILifetimeScope scope)
        {
            var logger = scope.Resolve<ILogger>();
            var howCreateDb = SettingsFactory.GetHowCreateDb(Env, AppConfiguration);
            //СОЗДАНИЕ БД----------------------------------------------------------------------------
            try
            {
                await scope.Resolve<IActionDb>().CreateDb(howCreateDb);
            }
            catch (PostgresException ex)
            {
                var connectionString = SettingsFactory.GetDbConnectionString(Env, AppConfiguration);
                logger.Fatal($"Ошибка создания БД. howCreateDb= {howCreateDb}  connectionString={connectionString}  Routine={ex.Routine}   SqlState= {ex.SqlState}   Exception= {ex}");
            }
            catch (Exception ex)
            {
              var connectionString = SettingsFactory.GetDbConnectionString(Env, AppConfiguration);
              logger.Fatal($"НЕ ИЗВЕСТНАЯ Ошибка создания БД. howCreateDb= {howCreateDb}  connectionString={connectionString}   Exception= {ex} ");
            }

            //СОЗДАНИЕ СПИСКА ПРОДЮССЕРОВ ДЛЯ ОТВЕТОВ-------------------------------------------------
            try
            {
                var buildProdusersUnionService = scope.Resolve<BuildProdusersUnionService<AdInputType>>();
                await buildProdusersUnionService.BuildAllProdusers();
            }
            catch (Exception ex) 
            {
                logger.Error( $"ОШИБКА СОЗДАНИЕ СПИСКА ПРОДЮССЕРОВ НА БАЗЕ ОПЦИЙ  {ex}");
            }

            //СОЗДАНИЕ СПИСКА ОПЦИЙ ДЛЯ ВСТАВОК В СТРОКУ-------------------------------------------------
            try
            {
                var buildStringInsertModelExt = scope.Resolve<BuildStringInsertModelExt>();
                await buildStringInsertModelExt.BuildAll();
            }
            catch (Exception ex)
            {
                logger.Error($"ОШИБКА СОЗДАНИЕ СПИСКА ПРОДЮССЕРОВ НА БАЗЕ ОПЦИЙ  {ex}");
            }

            //СОЗДАНИЕ СПИСКА УСТРОЙСТВ НА БАЗЕ ОПЦИЙ--------------------------------------------------
            try
            {
                var buildDeviceService = scope.Resolve<BuildDeviceService<AdInputType>>();
                await buildDeviceService.BuildAllDevices();
            }
            catch (AggregateException ex)
            {
                foreach (var innerException in ex.InnerExceptions)
                {
                    logger.Error(innerException, "ОШИБКА СОЗДАНИЕ СПИСКА УСТРОЙСТВ НА БАЗЕ ОПЦИЙ");
                }
            }
        }
    }
}
