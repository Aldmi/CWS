using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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
    }
}