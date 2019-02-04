using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace WebServer.Settings
{
    public static class InitSettings
    {
        public static string GetDbConnectionString(IHostingEnvironment env, IConfiguration conf)
        {
            return env.IsDevelopment() ? conf.GetConnectionString("OptionDbConnectionUseNpgsql") 
                                       : Environment.GetEnvironmentVariable("DbConnection");

        }
    }
}