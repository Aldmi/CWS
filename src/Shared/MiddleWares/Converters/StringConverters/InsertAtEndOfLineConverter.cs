
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;

namespace Shared.MiddleWares.Converters.StringConverters
{
    /// <summary>
    /// Вставка строки _option.EndLine в конец обрабатываемой строки.
    /// </summary>
    public class InsertAtEndOfLineConverter : BaseStringConverter
    {
        private readonly InsertAtEndOfLineConverterOption _option;

        public InsertAtEndOfLineConverter(InsertAtEndOfLineConverterOption option)
        {
            _option = option;
        }



        protected override string ConvertChild(string inProp, int dataId)
        {
            return $"{inProp}{_option.EndLine}";
        }
    }
}