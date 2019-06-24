using System;
using DAL.Abstract.Entities.Options.MiddleWare.Converters.DateTimeConverterOption;

namespace DeviceForExchange.MiddleWares.Converters.DateTimeConverters
{
    public class TimeZoneConverter : IConverterMiddleWare<DateTime>
    {
        private readonly TimeZoneConverterOption _option;

        public TimeZoneConverter(TimeZoneConverterOption option)
        {
            _option = option;
        }


        public int Priority { get; }

        public DateTime Convert(DateTime inProp, int dataId)
        {
            //DEBUG
            return inProp.AddHours(10);
        }
    }
}