using System;
using System.Spatial;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;

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


        public static IServiceCollection AddSerilogServices(this IServiceCollection services, string minLevel)
        {
            if (Enum.TryParse<LogEventLevel>(minLevel, out var result))
            {
                LevelSwitch = new LoggingLevelSwitch(result);
                var loggerConf = ConfigLogger();
                Log.Logger = loggerConf.CreateLogger();
                AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
                return services.AddSingleton(Log.Logger);
            }
            throw new ParseErrorException($"minLevel для логрования задан не верно {minLevel}");
        }


        public static IServiceCollection AddSerilogServices(this IServiceCollection services, LogEventLevel minLevel)
        {  
                LevelSwitch = new LoggingLevelSwitch(minLevel);
                var loggerConf = ConfigLogger();
                Log.Logger = loggerConf.CreateLogger();
                AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
                return services.AddSingleton(Log.Logger);          
        }

        #endregion


        private static LoggerConfiguration ConfigLogger()
        {
            var loggerConf = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(LevelSwitch)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error) 
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .WriteTo.Console(LogEventLevel.Information)

                .WriteTo.File("logs/Main_Log.txt",
                    LogEventLevel.Information,
                    rollingInterval: RollingInterval.Day,             //за 10 последних дней хранится Information лог (100МБ лимит размера файла)
                    retainedFileCountLimit: 10,
                    fileSizeLimitBytes: 100000000,                   
                    rollOnFileSizeLimit: true)

                .WriteTo.File("logs/Error_Log.txt",                  //за 10 последних дней хранится Error лог (100МБ лимит размера файла)
                    LogEventLevel.Error,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 10,
                    fileSizeLimitBytes: 100000000, //100МБ
                    rollOnFileSizeLimit: true)

                .WriteTo.File("logs/Debug/Debug_Log.txt",          //За 5 последних часов переписывается Debug лог (100МБ лимит размера файла).
                    LogEventLevel.Debug,
                    rollingInterval: RollingInterval.Hour,
                    retainedFileCountLimit: 5,
                    fileSizeLimitBytes: 100000000,
                    rollOnFileSizeLimit: true,                      
                    shared: true);

            //.WriteTo.Seq("http://localhost:5341", compact: true);

            return loggerConf;
        }
    }
}