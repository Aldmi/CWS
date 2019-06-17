using System;
using DAL.Abstract.Entities.Options.MiddleWare.Handlers;
using DeviceForExchnage.Benchmark.Shared.Converters.DateTimeConverters;

namespace DeviceForExchnage.Benchmark.Shared.Handlers
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