using Infrastructure.Dal.EfCore.Entities.ResponseProduser;
using Innofactor.EfCoreJsonValueConverter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Dal.EfCore.DbContext.EntitiConfiguration
{
    public class EfProduserUnionOptionConfig : IEntityTypeConfiguration<EfProduserUnionOption>
    {
        public void Configure(EntityTypeBuilder<EfProduserUnionOption> builder)
        {
            //builder.Property<string>("KafkaProduserOptionsMetaData")
            //    .HasField("_kafkaProduserOptionsMetaData");

            //builder.Property<string>("SignalRProduserOptionsMetaData")
            //    .HasField("_signalRProduserOptionsMetaData");

            //builder.Property<string>("WebClientProduserOptionsMetaData")
            //    .HasField("_webClientProduserOptionsMetaData");


            //builder.Property<string>("_kafkaProduserOptionsMetaData");
            //builder.Property<string>("_signalRProduserOptionsMetaData");
            //builder.Property<string>("_webClientProduserOptionsMetaData");


            builder.Property(p => p.KafkaProduserOptions)
                .HasJsonValueConversion();

            builder.Property(p => p.SignalRProduserOptions)
                .HasJsonValueConversion();

            builder.Property(p => p.WebClientProduserOptions)
                .HasJsonValueConversion();
        }
    }
}