using System;
using DAL.Abstract.Entities.Options.MiddleWare.Handlers;
using Domain.Device.MiddleWares.Converters.DateTimeConverters;

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