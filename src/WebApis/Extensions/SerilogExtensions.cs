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
using WebApiSwc.Settings;

namespace WebApiSwc.Extensions
{
    public static class SerilogExtensions
    {
        private static LoggingLevelSwitch LevelSwitch { get; set; }
        private static LoggerSettings LoggerSettings { get; set; }

        public static LogEventLevel GetMinimumLevel => LevelSwitch.MinimumLevel;




        #region Methode

        public static void ChangeLogEventLevel(LogEventLevel minLevel)
        {
            LevelSwitch.MinimumLevel = minLevel;
        }


        public static IServiceCollection AddSerilogServices(this IServiceCollection services, LoggerSettings settings, string appName)
        {
            LoggerSettings = settings;
            LevelSwitch = new LoggingLevelSwitch(LoggerSettings.MinLevel);
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
                .WriteTo.Console(LogEventLevel.Information);

            if (LoggerSettings.FileSinkSetting.Enable)
            {
                loggerConf
                    .WriteTo.File("logs/Main_Log.txt",
                        LogEventLevel.Information,
                        rollingInterval: RollingInterval.Day, //за 20 последних дней хранится Information лог (100МБ лимит размера файла)
                        retainedFileCountLimit: 20,
                        fileSizeLimitBytes: 100000000,
                        rollOnFileSizeLimit: true)

                    .WriteTo.File(
                        "logs/Error_Log.txt", //за 20 последних дней хранится Error лог (100МБ лимит размера файла)
                        LogEventLevel.Error,
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 20,
                        fileSizeLimitBytes: 100000000,
                        rollOnFileSizeLimit: true)

                    .WriteTo.File(
                        "logs/Debug/Debug_Log.txt", //За 24 последних часов переписывается Debug лог (100МБ лимит размера файла).
                        LogEventLevel.Debug,
                        rollingInterval: RollingInterval.Hour,
                        retainedFileCountLimit: 24,
                        fileSizeLimitBytes: 100000000,
                        rollOnFileSizeLimit: true,
                        shared: true);
            }

            if (LoggerSettings.ElasticsearchSinkSetting.Enable)
            {
                loggerConf
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
                            var indexName =
                                $"{appName.ToLower(CultureInfo.InvariantCulture)}-{offset:yyyy.MM}-{indexNumber}";
                            return indexName;
                        },

                        //Обработка ошибок записи в ES
                        FailureCallback = e => { Console.WriteLine("ES недоступно" + e.MessageTemplate); },
                        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                       EmitEventFailureHandling.WriteToFailureSink |
                                       EmitEventFailureHandling.RaiseCallback,
                        FailureSink = new FileSink("./failures.txt", new JsonFormatter(), null),

                    });
            }
            return loggerConf;
        }
    }
}