using System;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Shared.ForConfigFiles;

namespace Infrastructure.Dal.EfCore.DbContext
{
    /// <summary>
    /// Получение контекста для системы миграции (если конструктор контекста принимает парметры)
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            Console.WriteLine(args.Length);//DEBUG

            var path = @"F:\\Git\\CWS\\src\\WebApis";
            var config = JsonConfigLib.GetConfiguration(path);
            var connectionString = config.GetConnectionString("OptionDbConnectionUseNpgsql");
            return new Context(connectionString);
        }
    }
}