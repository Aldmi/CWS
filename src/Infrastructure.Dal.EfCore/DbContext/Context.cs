using System.Threading.Tasks;
using Infrastructure.Dal.Abstract.Enums;
using Infrastructure.Dal.EfCore.DbContext.EntitiConfiguration;
using Infrastructure.Dal.EfCore.Entities.Device;
using Infrastructure.Dal.EfCore.Entities.Exchange;
using Infrastructure.Dal.EfCore.Entities.ResponseProduser;
using Infrastructure.Dal.EfCore.Entities.Transport;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dal.EfCore.DbContext
{
    public sealed class Context : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly string _connStr;  // строка подключенния


        #region Reps

        public DbSet<EfSerialOption> SerialPortOptions { get; set; }
        public DbSet<EfTcpIpOption> TcpIpOptions { get; set; }
        public DbSet<EfHttpOption> HttpOptions { get; set; }
        public DbSet<EfDeviceOption> DeviceOptions { get; set; }
        public DbSet<EfExchangeOption> ExchangeOptions { get; set; }
        public DbSet<EfProduserUnionOption> ProduserUnionOptions { get; set; }

        #endregion



        #region ctor

        public Context(string connStr)
        {
            _connStr = connStr;
           // ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;//Отключение Tracking для всего контекста
        }

        #endregion



        #region Config

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connStr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.ApplyConfiguration(new EfDeviceOptionConfig());
           modelBuilder.ApplyConfiguration(new EfExchangeOptionConfig());
           modelBuilder.ApplyConfiguration(new EfHttpOptionConfig());
           modelBuilder.ApplyConfiguration(new EfProduserUnionOptionConfig());
           base.OnModelCreating(modelBuilder);
        }

        #endregion



        #region Methode

        public async Task CreateDb(HowCreateDb howCreateDb)
        {
            switch (howCreateDb)
            {
                case HowCreateDb.Migrate:
                   await Database.MigrateAsync();       //Если БД нет, то создать по схемам МИГРАЦИИ.
                    break;
                case HowCreateDb.EnsureCreated:
                    Database.EnsureCreated();           //Если БД нет, то создать. (ОТКЛЮЧАТЬ ПРИ МИГРАЦИИ)
                    break;
            }
        }

        #endregion
    }
}