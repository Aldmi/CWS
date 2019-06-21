using DAL.Abstract.Entities.Options.MiddleWare.Converters.StringConvertersOption;

namespace DeviceForExchange.MiddleWares.Converters.StringConverters
{
    public class InseartStringConverter : BaseStringConverter
    {
        private readonly InseartStringConverterOption _option;


        public InseartStringConverter(InseartStringConverterOption option)
            : base(option)
        {
            _option = option;
        }


        protected override string ConvertChild(string inProp)
        {
            //DEBUG
            return inProp + "After InseartStringConverter";
        }
    }
}