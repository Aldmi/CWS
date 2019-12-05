using System;
using Domain.Device.MiddleWares.Converters.DateTimeConverters;
using DateTimeHandlerMiddleWareOption = Domain.Device.Repository.Entities.MiddleWareOption.HandlersOption.DateTimeHandlerMiddleWareOption;

namespace Domain.Device.MiddleWares.Handlers
{
    public class DateTimeHandlerMiddleWare : BaseHandlerMiddleWare<DateTime>
    {
        public DateTimeHandlerMiddleWare(DateTimeHandlerMiddleWareOption option)
        {
            PropName = option.PropName;

            if (option.TimeZoneConverterOption != null)
            {
                Converters.Add(new TimeZoneConverter(option.TimeZoneConverterOption));
            }
        }
    }
}