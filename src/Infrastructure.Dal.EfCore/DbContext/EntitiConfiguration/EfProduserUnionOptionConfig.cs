using Infrastructure.Dal.EfCore.Entities.ResponseProduser;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Dal.EfCore.DbContext.EntitiConfiguration
{
    public class EfProduserUnionOptionConfig : IEntityTypeConfiguration<EfProduserUnionOption>
    {
        public void Configure(EntityTypeBuilder<EfProduserUnionOption> builder)
        {
            builder.Property<string>("KafkaProduserOptionsMetaData")
                .HasField("_kafkaProduserOptionsMetaData");

            builder.Property<string>("SignalRProduserOptionsMetaData")
                .HasField("_signalRProduserOptionsMetaData");

            builder.Property<string>("WebClientProduserOptionsMetaData")
                .HasField("_webClientProduserOptionsMetaData");
        }
    }
}