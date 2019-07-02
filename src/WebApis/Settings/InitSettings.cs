using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.Enums;

namespace WebApiSwc.Settings
{
    public static class InitSettings
    {
        public static string GetDbConnectionString(IHostingEnvironment env, IConfiguration conf)
        {
           var connectionStr= env.IsDevelopment() ? conf.GetConnectionString("OptionDbConnectionUseNpgsql")
                                                   : Environment.GetEnvironmentVariable("DbConnection");

            if (string.IsNullOrEmpty(connectionStr))
                throw new NullReferenceException($"Переменная connectionStr (строка подключения к БД) НЕ найденна. IsDevelopment= {env.IsDevelopment()}");

            return connectionStr;
        }


        public static HowCreateDb GetHowCreateDb(IHostingEnvironment env, IConfiguration conf)
        {
            var howCreateDbStr = env.IsDevelopment()
                ? conf["HowCreateDb"]
                : Environment.GetEnvironmentVariable("HowCreateDb");

            if (string.IsNullOrEmpty(howCreateDbStr))
                throw new NullReferenceException($"Переменная HowCreateDb (как создавать БД при запуске приложения) НЕ найденна. IsDevelopment= {env.IsDevelopment()}");

            return Enum.TryParse<HowCreateDb>(howCreateDbStr, out var howCreateDb) ? howCreateDb : HowCreateDb.None;
        }


        public static string GetLoggerMinlevel(IHostingEnvironment env, IConfiguration conf)
        {
            var minLevelStr= env.IsDevelopment()
                ? conf["Logger:MinLevel"]
                : Environment.GetEnvironmentVariable("Logger_MinLevel");

            if (string.IsNullOrEmpty(minLevelStr))
                throw new NullReferenceException($"Задание минимального уровня логировангия не найдено. IsDevelopment= {env.IsDevelopment()}");

            return minLevelStr;
        }


        public static FirewallSettings GetFirewallConfig(IHostingEnvironment env, IConfiguration conf)
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

                firewallSettings= new FirewallSettings(iPAddress, cidrNotation);
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


            //DEBUG-----------------
            //var firewallSett1 = Environment.GetEnvironmentVariable("Firewall");

            //try
            //{
            //    var jsonResp = JsonConvert.DeserializeObject<FirewallSettings>(firewallSett1);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}
            //DEBUG----------------
        }

    }
}