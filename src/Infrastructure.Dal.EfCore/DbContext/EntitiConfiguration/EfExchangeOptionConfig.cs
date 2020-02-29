using Infrastructure.Dal.EfCore.Entities.Exchange;
using Innofactor.EfCoreJsonValueConverter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Dal.EfCore.DbContext.EntitiConfiguration
{
    public class EfExchangeOptionConfig : IEntityTypeConfiguration<EfExchangeOption>
    {
        public void Configure(EntityTypeBuilder<EfExchangeOption> builder)
        {
            //связать приватно поле _keyTransportMetaData с типом KeyTransportMetaData в БД (типа string).  Для сериализации объекта в JSON.
            //builder.Property<string>("KeyTransportMetaData")
            //    .HasField("_keyTransportMetaData")
            //    .IsRequired();

            //связать приватно поле _providerOptionMetaData с типом Provider в БД (типа string).  Для сериализации объекта в JSON.
            //builder.Property<string>("ProviderOptionMetaData")
            //    .HasField("_providerOptionMetaData")
            //    .IsRequired();

            //связать приватно поле _providerOptionMetaData с типом Provider в БД (типа string).  Для сериализации объекта в JSON.
            //builder.Property<string>("CycleFuncOptionMetaData")
            //    .HasField("_cycleFuncOptionMetaData")
            //    .IsRequired();


            //builder.Property<string>("_keyTransportMetaData").IsRequired();
            //builder.Property<string>("_providerOptionMetaData").IsRequired();
            //builder.Property<string>("_cycleFuncOptionMetaData").IsRequired();




            builder.Property(p => p.KeyTransport)
                .HasJsonValueConversion();

            builder.Property(p => p.Provider)
                .HasJsonValueConversion();

            builder.Property(p => p.CycleFuncOption)
                .HasJsonValueConversion();
        }
    }
}