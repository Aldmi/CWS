using BaseConverterOption = Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.BaseConverterOption;

namespace Domain.Device.MiddleWares.Converters.StringConverters
{
    public abstract class BaseStringConverter : IConverterMiddleWare<string>
    {
        public int Priority { get; }


        protected BaseStringConverter(BaseConverterOption baseOption)
        {
            Priority = baseOption.Priority;
        }

        public string Convert(string inProp, int dataId)
        {
            return inProp == null ? null : ConvertChild(inProp, dataId);
        }


        protected abstract string ConvertChild(string inProp, int dataId);
    }
} 