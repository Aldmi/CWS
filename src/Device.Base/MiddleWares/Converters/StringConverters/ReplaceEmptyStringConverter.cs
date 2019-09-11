using DAL.Abstract.Entities.Options.MiddleWare.Converters.StringConvertersOption;

namespace Domain.Device.MiddleWares.Converters.StringConverters
{

    /// <summary>
    /// Заменяет пустую или NULL строку на указанную в опциях.
    /// </summary>
    public class ReplaceEmptyStringConverter : BaseStringConverter
    {
        private readonly ReplaceEmptyStringConverterOption _option;

        public ReplaceEmptyStringConverter(ReplaceEmptyStringConverterOption option)
            : base(option)
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