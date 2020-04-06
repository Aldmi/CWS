using LimitStringConverterOption = Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.StringConvertersOption.LimitStringConverterOption;

namespace Domain.Device.MiddleWares.Converters.StringConverters
{
    /// <summary>
    /// Обрезает строку
    /// </summary>
    public class LimitStringConverter : BaseStringConverter
    {
        private readonly LimitStringConverterOption _option;

        public LimitStringConverter(LimitStringConverterOption option)
        {
            _option = option;
        }


        protected override string ConvertChild(string inProp, int dataId)
        {
            return (inProp.Length <= _option.Limit) ? inProp : inProp.Remove(_option.Limit);
        }
    }
}