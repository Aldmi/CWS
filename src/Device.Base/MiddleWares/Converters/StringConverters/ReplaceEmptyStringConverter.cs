using DAL.Abstract.Entities.Options.MiddleWare.Hadlers.StringConvertersOption;

namespace DeviceForExchange.MiddleWares.Converters.StringConverters
{
    public class ReplaceEmptyStringConverter : IConverterMiddleWare<string>
    {

        private readonly ReplaceEmptyStringConverterOption _option;

        public ReplaceEmptyStringConverter(ReplaceEmptyStringConverterOption option)
        {
            _option = option;
        }


        public string Convert(string inProp)
        {
            //DEBUG
            return inProp + "After ReplaceEmptyStringConverter";
        }
    }
}