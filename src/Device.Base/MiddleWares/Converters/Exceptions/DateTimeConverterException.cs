using System;
using System.Runtime.Serialization;

namespace Domain.Device.MiddleWares.Converters.Exceptions
{
    public class DateTimeConverterException : Exception
    {
        public DateTimeConverterException() { }
        public DateTimeConverterException(string message) : base(message) { }
        public DateTimeConverterException(string message, Exception inner) : base(message, inner) { }
        protected DateTimeConverterException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}