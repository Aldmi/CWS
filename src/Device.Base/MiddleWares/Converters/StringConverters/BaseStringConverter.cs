namespace DeviceForExchange.MiddleWares.Converters.StringConverters
{
    public abstract class BaseStringConverter : IConverterMiddleWare<string>
    {
        public string Convert(string inProp)
        {
            if (inProp == null)
                return null;

            return ConvertChild(inProp);
        }


        protected abstract string ConvertChild(string inProp);
    }
}