using System;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Shared.ForConfigFiles;

namespace DAL.EFCore.DbContext
{
    /// <summary>
    /// Получение контекста для системы миграции (если конструктор контекста принимает парметры)
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            Console.WriteLine("Migration Started>>>>>>>>>>>>>>>>>>>>>>>>>");
            var path = @"D:\\Git\\CWS\\src\\CoreWithAutoFack";
            var config = JsonConfigLib.GetConfiguration(path);
            var connectionString = config.GetConnectionString("OptionDbConnectionUseNpgsql");
            Console.WriteLine($"Connection string = {connectionString}");
            return new Context(connectionString, HowCreateDb.None);
        }
    }
}