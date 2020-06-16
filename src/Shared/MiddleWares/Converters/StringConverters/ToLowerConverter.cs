using Shared.MiddleWares.ConvertersOption.StringConvertersOption;


namespace Shared.MiddleWares.Converters.StringConverters
{
    /// <summary>
    /// Переводит строку в нижний регистр.
    /// </summary>
    public class ToLowerConverter : BaseStringConverter
    {
        private readonly ToLowerConverterOption _option;


        public ToLowerConverter(ToLowerConverterOption option)
        {
            _option = option;
        }


        protected override string ConvertChild(string inProp, int dataId)
        {
            var res = inProp.ToLowerInvariant();
            return res;
        }
    }
}