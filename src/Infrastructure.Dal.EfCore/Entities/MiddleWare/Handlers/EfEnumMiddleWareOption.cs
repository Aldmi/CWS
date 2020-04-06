using System.Collections.Generic;
using Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.EnumsConvertersOption;

namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Handlers
{
    public class EfEnumMiddleWareOption
    {
        public string PropName { get; set; }
        public string Path2Type { get; set; }
        public List<EfUnitEnumConverterOption> Converters { get; set; }
    }

    public class EfUnitEnumConverterOption
    {
        public EfEnumMemConverterOption EnumMemConverterOption { get; set; }
    }
}