using System;
using System.Spatial;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace WebServer.Extensions
{
    public static class SerilogExtensions
    {
        private static LoggingLevelSwitch LevelSwitch { get; set; }
        public static LogEventLevel GetMinimumLevel => LevelSwitch.MinimumLevel;



        #region ctor

        public static void ChangeLogEventLevel(LogEventLevel minLevel)
        {
            LevelSwitch.MinimumLevel = minLevel;
        }

        #endregion



        #region Methode

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
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(LogEventLevel.Information)
                .WriteTo.File("logs/Main_Log.txt", LogEventLevel.Information, rollingInterval: RollingInterval.Day)
                .WriteTo.File("logs/Error_Log.txt", LogEventLevel.Error, rollingInterval: RollingInterval.Day);
              //.WriteTo.Seq("http://localhost:5341", compact: true);

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == EnvironmentName.Development)
            {
                loggerConf.WriteTo.File("logs/Debug/Debug_Log.txt", LogEventLevel.Debug, rollingInterval: RollingInterval.Hour);
            }
            return loggerConf;
        }
    }
}