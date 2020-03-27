using System;
using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption;

namespace Domain.Device.MiddleWares.Converters.EnumsConverters
{
    public abstract class BaseEnumConverter : IConverterMiddleWare<Enum>
    {
        public int Priority { get; }
        protected BaseEnumConverter(BaseConverterOption baseOption)
        {
            Priority = baseOption.Priority;
        }

        public abstract Enum Convert(Enum inProp, int dataId);
    }
}