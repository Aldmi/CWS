using System;
using System.Linq;
using Infrastructure.Dal.Abstract.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog.Events;


namespace WebApiSwc.Settings
{
    public static class SettingsFactory
    {
        public static string GetDbConnectionString(IHostEnvironment env, IConfiguration conf)
        {
            var connectionStr = env.IsDevelopment() ? conf.GetConnectionString("OptionDbConnectionUseNpgsql")
                                                    : Environment.GetEnvironmentVariable("DbConnection");

            if (string.IsNullOrEmpty(connectionStr))
                throw new NullReferenceException($"Переменная connectionStr (строка подключения к БД) НЕ найденна. IsDevelopment= {env.IsDevelopment()}");

            return connectionStr;
        }


        public static HowCreateDb GetHowCreateDb(IHostEnvironment env, IConfiguration conf)
        {
            var howCreateDbStr = env.IsDevelopment()
                ? conf["HowCreateDb"]
                : Environment.GetEnvironmentVariable("HowCreateDb");

            if (string.IsNullOrEmpty(howCreateDbStr))
                throw new NullReferenceException($"Переменная HowCreateDb (как создавать БД при запуске приложения) НЕ найденна. IsDevelopment= {env.IsDevelopment()}");

            return Enum.TryParse<HowCreateDb>(howCreateDbStr, out var howCreateDb) ? howCreateDb : HowCreateDb.None;
        }



        public static FirewallSettings GetFirewallConfig(IHostEnvironment env, IConfiguration conf)
        {
            FirewallSettings firewallSettings;

            if (env.IsDevelopment())
            {
                var conIpAddress = conf.GetSection("Firewall:IPAddress");
                var conCidrNotation = conf.GetSection("Firewall:CIDRNotation");
                var iPAddress = conIpAddress.GetChildren().Select(section => section.Value).ToList();
                var cidrNotation = conCidrNotation.GetChildren().Select(section => section.Value).ToList();

                if (!iPAddress.Any() && !cidrNotation.Any())
                    return null;

                firewallSettings = new FirewallSettings(iPAddress, cidrNotation);
            }
            else
            {
                var firewallSett = Environment.GetEnvironmentVariable("Firewall");
                if (string.IsNullOrEmpty(firewallSett))
                    return null;

                try
                {
                    firewallSettings = JsonConvert.DeserializeObject<FirewallSettings>(firewallSett);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Исключение при дессериализации настроек фаерволла {ex}");
                }
            }

            return firewallSettings;
        }



        public static LoggerSettings GetLoggerConfig(IHostEnvironment env, IConfiguration conf)
        {
            LoggerSettings loggerSettings;

            if (env.IsDevelopment())
            {
                if (!Enum.TryParse<LogEventLevel>(conf["Logger:MinLevel"], out var minLevel))
                    throw new Exception($"Logger:MinLevel не удалось преобразовать к bool. IsDevelopment= {env.IsDevelopment()}"); //TODO:Исключение, возникающее при неудачном разборе сериализованного формата.

                if (!bool.TryParse(conf["Logger:fileSinkSetting:enable"], out var fileSinkEnabel))
                    throw new Exception($"Logger:fileSinkSetting:enable не удалось преобразовать к bool. IsDevelopment= {env.IsDevelopment()}");

                if (!bool.TryParse(conf["Logger:elasticsearchSinkSetting:enable"], out var elasticsearchSinkEnable))
                    throw new Exception($"Logger:fileSinkSetting:enable не удалось преобразовать к bool. IsDevelopment= {env.IsDevelopment()}");

                var fileSinkSett = new FileSinkSetting(fileSinkEnabel);
                var elasticsearchSinkSetting = new ElasticsearchSinkSetting(elasticsearchSinkEnable);

                loggerSettings = new LoggerSettings(minLevel, fileSinkSett, elasticsearchSinkSetting);
            }
            else
            {
                var loggerSett = Environment.GetEnvironmentVariable("LoggerSetting");
                if (string.IsNullOrEmpty(loggerSett))
                    return null;

                try
                {
                    loggerSettings = JsonConvert.DeserializeObject<LoggerSettings>(loggerSett);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Исключение при дессериализации настроек логера {ex}");
                }
            }

            return loggerSettings;
        }



    }
}