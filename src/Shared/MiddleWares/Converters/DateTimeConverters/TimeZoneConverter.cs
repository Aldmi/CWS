using System;
using TimeZoneConverterOption = Shared.MiddleWares.ConvertersOption.DateTimeConverterOption.TimeZoneConverterOption;

namespace Shared.MiddleWares.Converters.DateTimeConverters
{
    public class TimeZoneConverter : IConverterMiddleWare<DateTime>
    {
        private readonly TimeZoneConverterOption _option;
        private int? _increaseInTime;

        public TimeZoneConverter(TimeZoneConverterOption option)
        {
            _option = option;
        } 


        public DateTime Convert(DateTime inProp, int dataId)
        {
            _increaseInTime ??= _option.GetIncreaseInTime();
            return inProp.AddHours(_increaseInTime.Value);
        }
    }
}