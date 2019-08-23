using DAL.EFCore.Entities.Produser;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EFCore.DbContext.EntitiConfiguration
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