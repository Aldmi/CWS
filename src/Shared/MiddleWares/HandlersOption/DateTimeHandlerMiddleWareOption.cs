using System;
using System.Collections.Generic;
using System.Linq;
using Shared.MiddleWares.Converters;
using Shared.MiddleWares.Converters.DateTimeConverters;
using Shared.MiddleWares.ConvertersOption.DateTimeConverterOption;

namespace Shared.MiddleWares.HandlersOption
{
    public class DateTimeHandlerMiddleWareOption
    {
        public List<UnitDateTimeConverterOption> Converters { get; set; }

        public IList<IConverterMiddleWare<DateTime>> CreateConverters()
        {
            return Converters.Select(c => c.CreateConverter()).ToList();
        }
    }


    /// <summary>
    /// Единица обработки DateTime конвертором
    /// </summary>
    public class UnitDateTimeConverterOption
    {
        public TimeZoneConverterOption TimeZoneConverterOption { get; set; }

        public IConverterMiddleWare<DateTime> CreateConverter()
        {
            if (TimeZoneConverterOption != null) return new TimeZoneConverter(TimeZoneConverterOption);

            throw new NotSupportedException("В UnitDateTimeConverterOption необходимо указать хотя бы одну опцию");
        }
    }
}