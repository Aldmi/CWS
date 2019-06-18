using DAL.Abstract.Entities.Options.MiddleWare.Converters.StringConvertersOption;

namespace DeviceForExchange.MiddleWares.Converters.StringConverters
{
    public class LimitStringComverter : BaseStringConverter
    {
        private readonly LimitStringConverterOption _option;

        public LimitStringComverter(LimitStringConverterOption option)
        {
            _option = option;
        }



        protected override string ConvertChild(string inProp)
        {
            //DEBUG
            return inProp + "After LimitStringComverter";
        }
    }
}