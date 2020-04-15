using Shared.MiddleWares.ConvertersOption.StringConvertersOption;

namespace Shared.MiddleWares.Converters.StringConverters
{
    public class InseartStringConverter : BaseStringConverter
    {
        private readonly InseartStringConverterOption _option;


        public InseartStringConverter(InseartStringConverterOption option)
        {
            _option = option;
        }


        protected override string ConvertChild(string inProp, int dataId)
        {
            //DEBUG
            return inProp + "After InseartStringConverter";
        }
    }
}