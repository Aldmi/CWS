using Infrastructure.Dal.EfCore.Entities.Transport;
using Innofactor.EfCoreJsonValueConverter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Dal.EfCore.DbContext.EntitiConfiguration
{
    public class EfHttpOptionConfig : IEntityTypeConfiguration<EfHttpOption>
    {
        public void Configure(EntityTypeBuilder<EfHttpOption> builder)
        {
            //связать приватно поле _headersMetaData с типом HeadersCollection в БД (типа string). Для сериализации объекта в JSON.
            //builder.Property<string>("HeadersCollection")
            //       .HasField("_headersMetaData");

            //builder.Property<string>("_headersMetaData");

            builder.Property(p => p.Headers)
                .HasJsonValueConversion();
        }
    }
}