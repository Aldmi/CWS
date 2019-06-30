using System;
using System.Runtime.Serialization;

namespace DeviceForExchange.MiddleWares.Converters.Exceptions
{
    public class StringConverterException : Exception
    {
        public StringConverterException() { }
        public StringConverterException(string message) : base(message) { }
        public StringConverterException(string message, Exception inner) : base(message, inner) { }
        protected StringConverterException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}