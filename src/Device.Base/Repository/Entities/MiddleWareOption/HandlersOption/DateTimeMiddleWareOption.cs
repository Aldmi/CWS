using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Device.MiddleWares.Converters;
using Domain.Device.MiddleWares.Converters.DateTimeConverters;
using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.DateTimeConverterOption;

namespace Domain.Device.Repository.Entities.MiddleWareOption.HandlersOption
{
    public class DateTimeMiddleWareOption
    {
        public string PropName { get; set; }                       //Имя свойства для обработки
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