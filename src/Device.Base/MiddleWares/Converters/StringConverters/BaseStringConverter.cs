using DAL.Abstract.Entities.Options.MiddleWare.Converters;

namespace DeviceForExchange.MiddleWares.Converters.StringConverters
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