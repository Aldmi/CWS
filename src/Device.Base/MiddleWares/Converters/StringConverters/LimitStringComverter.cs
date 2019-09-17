using LimitStringConverterOption = Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.StringConvertersOption.LimitStringConverterOption;

namespace Domain.Device.MiddleWares.Converters.StringConverters
{
    /// <summary>
    /// Обрезает строку
    /// </summary>
    public class LimitStringComverter : BaseStringConverter
    {
        private readonly LimitStringConverterOption _option;

        public LimitStringComverter(LimitStringConverterOption option)
            : base(option)
        {
            _option = option;
        }


        protected override string ConvertChild(string inProp, int dataId)
        {
            return (inProp.Length <= _option.Limit) ? inProp : inProp.Remove(_option.Limit);
        }
    }
}