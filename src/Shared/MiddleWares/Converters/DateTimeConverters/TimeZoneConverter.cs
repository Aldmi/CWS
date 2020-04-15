using System;
using TimeZoneConverterOption = Shared.MiddleWares.ConvertersOption.DateTimeConverterOption.TimeZoneConverterOption;

namespace Shared.MiddleWares.Converters.DateTimeConverters
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


        public void SendCommand(MemConverterCommand command)
        {
            //NOT IMPLEMENTED
        }

        public void Mem()
        {
            throw new NotImplementedException();
        }
    }
}