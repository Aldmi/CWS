using Infrastructure.Dal.EfCore.Entities.Device;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Dal.EfCore.DbContext.EntitiConfiguration
{
    public class EfDeviceOptionConfig : IEntityTypeConfiguration<EfDeviceOption>
    {
        public void Configure(EntityTypeBuilder<EfDeviceOption> builder)
        {
            //связать приватно поле _exchangeKeysMetaData с типом ExchangeKeysCollection в БД (типа string).  Для сериализации объекта в JSON.
            builder.Property<string>("ExchangeKeysCollection")
                   .HasField("_exchangeKeysMetaData");

            builder.Property<string>("MiddleWareInDataMetaData")
                .HasField("_middleWareInDataMetaData");
        }
    }
}