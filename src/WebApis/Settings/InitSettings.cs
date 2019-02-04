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
            return env.IsDevelopment() ? conf.GetConnectionString("OptionDbConnectionUseNpgsql") 
                                       : Environment.GetEnvironmentVariable("DbConnection");

        }


        public static HowCreateDb GetHowCreateDb(IHostingEnvironment env, IConfiguration conf)
        {
            var howCreateDbStr = env.IsDevelopment()
                ? conf["HowCreateDb"]
                : Environment.GetEnvironmentVariable("HowCreateDb");

            return Enum.TryParse<HowCreateDb>(howCreateDbStr, out var howCreateDb) ? howCreateDb : HowCreateDb.None;
        }
    }
}