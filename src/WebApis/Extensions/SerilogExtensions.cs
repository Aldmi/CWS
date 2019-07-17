using System;
using System.Globalization;
using System.Spatial;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;

namespace WebApiSwc.Extensions
{
    public static class SerilogExtensions
    {
        private static LoggingLevelSwitch LevelSwitch { get; set; }
        public static LogEventLevel GetMinimumLevel => LevelSwitch.MinimumLevel;




        #region Methode

        public static void ChangeLogEventLevel(LogEventLevel minLevel)
        {
            LevelSwitch.MinimumLevel = minLevel;
        }


        public static IServiceCollection AddSerilogServices(this IServiceCollection services, string minLevel, string appName)
        {
            if (Enum.TryParse<LogEventLevel>(minLevel, out var result))
            {
                LevelSwitch = new LoggingLevelSwitch(result);
                var loggerConf = ConfigLogger(appName);
                Log.Logger = loggerConf.CreateLogger();
                AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
                return services.AddSingleton(Log.Logger);
            }
            throw new ParseErrorException($"minLevel для логрования задан не верно {minLevel}");
        }


        public static IServiceCollection AddSerilogServices(this IServiceCollection services, LogEventLevel minLevel, string appName)
        {  
                LevelSwitch = new LoggingLevelSwitch(minLevel);
                var loggerConf = ConfigLogger(appName);
                Log.Logger = loggerConf.CreateLogger();
                AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
                return services.AddSingleton(Log.Logger);          
        }

        #endregion


        private static LoggerConfiguration ConfigLogger(string appName)
        {
            var newIndexPerDay = 3;     //Количество дней для новго индекса.
            var loggerConf = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(LevelSwitch)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error) 
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .WriteTo.Console(LogEventLevel.Information)

                .WriteTo.File("logs/Main_Log.txt",
                    LogEventLevel.Information,
                    rollingInterval: RollingInterval.Day,                 //за 20 последних дней хранится Information лог (100МБ лимит размера файла)
                    retainedFileCountLimit: 20,
                    fileSizeLimitBytes: 100000000,                   
                    rollOnFileSizeLimit: true)

                .WriteTo.File("logs/Error_Log.txt",                  //за 20 последних дней хранится Error лог (100МБ лимит размера файла)
                    LogEventLevel.Error,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 20,
                    fileSizeLimitBytes: 100000000,
                    rollOnFileSizeLimit: true)

                .WriteTo.File("logs/Debug/Debug_Log.txt",           //За 24 последних часов переписывается Debug лог (100МБ лимит размера файла).
                    LogEventLevel.Debug,
                    rollingInterval: RollingInterval.Hour,
                    retainedFileCountLimit: 24,
                    fileSizeLimitBytes: 100000000,
                    rollOnFileSizeLimit: true,                      
                    shared: true)

                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    MinimumLogEventLevel = LogEventLevel.Information,
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                    CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),

                    //Правило формирования индекса
                    IndexDecider = (@event, offset) =>
                    {
                        var indexNumber = Math.Ceiling((double)offset.Day / newIndexPerDay);
                        var indexName = $"{appName.ToLower(CultureInfo.InvariantCulture)}-{offset:yyyy.MM}-{indexNumber}";
                        return indexName;
                    },

                    //Обработка ошибок записи в ES
                    FailureCallback = e =>
                    {
                        Console.WriteLine("ES недоступно" + e.MessageTemplate);
                    },
                    EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                       EmitEventFailureHandling.WriteToFailureSink |
                                       EmitEventFailureHandling.RaiseCallback,
                    FailureSink = new FileSink("./failures.txt", new JsonFormatter(), null),

                });

            //.WriteTo.Seq("http://localhost:5341", compact: true);

            return loggerConf;
        }
    }
}