

namespace Shared.MiddleWares.Converters.StringConverters
{
    public abstract class BaseStringConverter : IConverterMiddleWare<string>
    {
        public string Convert(string inProp, int dataId)
        {
            return inProp == null ? null : ConvertChild(inProp, dataId);
        }


        protected abstract string ConvertChild(string inProp, int dataId);
    }
} 