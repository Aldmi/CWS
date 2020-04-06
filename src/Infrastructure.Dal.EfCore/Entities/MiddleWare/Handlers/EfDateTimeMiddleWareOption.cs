using System.Collections.Generic;
using Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.DateTimeConverterOption;

namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Handlers
{
    public class EfDateTimeMiddleWareOption
    {
        public string PropName { get; set; }
        public List<EfUnitDateTimeConverterOption> Converters { get; set; }
    }

    public class EfUnitDateTimeConverterOption
    {
        public EfTimeZoneConverterOption TimeZoneConverterOption { get; set; }
    }
}