using System;
using System.Linq;
using Domain.Device.MiddleWares.Converters.DateTimeConverters;
using DateTimeHandlerMiddleWareOption = Domain.Device.Repository.Entities.MiddleWareOption.HandlersOption.DateTimeHandlerMiddleWareOption;

namespace Domain.Device.MiddleWares.Handlers
{
    public class DateTimeHandlerMiddleWare : BaseHandlerMiddleWare<DateTime>
    {
        public DateTimeHandlerMiddleWare(string propName, params DateTimeHandlerMiddleWareOption[] options)
        {
            PropName = propName;
            foreach (var option in options)
            {

                if (option.TimeZoneConverterOption != null)
                {
                    Converters.Add(new TimeZoneConverter(option.TimeZoneConverterOption));
                }
            }
            var orderedConverters = Converters.OrderBy(c => c.Priority).ToList();
            Converters.Clear();
            Converters.AddRange(orderedConverters);
        }
    }
}