using Infrastructure.Dal.EfCore.Entities.ResponseProduser;
using Infrastructure.Dal.EfCore.Entities.StringInsertModelExt;
using Innofactor.EfCoreJsonValueConverter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Dal.EfCore.DbContext.EntitiConfiguration
{
    public class EfStringInseartModelExtConfig : IEntityTypeConfiguration<EfStringInseartModelExt>
    {
        public void Configure(EntityTypeBuilder<EfStringInseartModelExt> builder)
        {
            builder.Property(p => p.BorderSubString)
                .HasJsonValueConversion();

            builder.Property(p => p.StringHandlerMiddleWareOption)
                .HasJsonValueConversion();

            builder.Property(p => p.MathematicFormula)
                .HasJsonValueConversion();
        }
    }
}