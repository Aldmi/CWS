using System.Collections.Generic;
using Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.EnumsConvertersOption;

namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Handlers
{
    public class EfEnumHandlerMiddleWareOption
    {
        public string Path2Type { get; set; }
        public List<EfUnitEnumConverterOption> Converters { get; set; }
    }

    public class EfUnitEnumConverterOption
    {
        public EfEnumMemConverterOption EnumMemConverterOption { get; set; }
    }
}