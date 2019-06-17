using DAL.Abstract.Entities.Options.MiddleWare.Converters.StringConvertersOption;

namespace DeviceForExchnage.Benchmark.Shared.Converters.StringConverters
{
    public class LimitStringComverter : IConverterMiddleWare<string>
    {
        private readonly LimitStringConverterOption _option;

        public LimitStringComverter(LimitStringConverterOption option)
        {
            _option = option;
        }


        public string Convert(string inProp)
        {
            //DEBUG
            return inProp + "After LimitStringComverter";
        }
    }
}