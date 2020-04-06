using ReplaceEmptyStringConverterOption = Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.StringConvertersOption.ReplaceEmptyStringConverterOption;

namespace Domain.Device.MiddleWares.Converters.StringConverters
{

    /// <summary>
    /// Заменяет пустую или NULL строку на указанную в опциях.
    /// </summary>
    public class ReplaceEmptyStringConverter : BaseStringConverter
    {
        private readonly ReplaceEmptyStringConverterOption _option;

        public ReplaceEmptyStringConverter(ReplaceEmptyStringConverterOption option)
        {
            _option = option;
        }


        protected override string ConvertChild(string inProp, int dataId)
        {
            if (string.IsNullOrEmpty(inProp))
                return _option.ReplacementString;

            return string.IsNullOrEmpty(inProp) ? _option.ReplacementString : inProp;
        }
    }
}