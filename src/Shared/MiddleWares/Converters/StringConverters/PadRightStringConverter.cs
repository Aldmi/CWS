using Shared.MiddleWares.ConvertersOption.StringConvertersOption;

namespace Shared.MiddleWares.Converters.StringConverters
{
    /// <summary>
    /// Дополняет пробелами строку до длинны указанной в _option.Lenght.
    /// Если длинна строки больше или равна _option.Lenght, то дополнение не происходит.
    ///  Дополнение происходит сивмолами _option.PaddingChar
    /// </summary>
    public class PadRightStringConverter : BaseStringConverter
    {
        private readonly PadRightStringConverterOption _option;
        public PadRightStringConverter(PadRightStringConverterOption option)
        {
            _option = option;
        }


        /// <summary>
        /// null- заменяю на PaddingChar, и дополняет PaddingChar
        /// </summary>
        protected override string NullHandler()
        {
            var paddingChar = _option.PaddingChar ?? ' ';
            return ConvertChild(paddingChar.ToString(), 1);
        }

        protected override string ConvertChild(string inProp, int dataId)
        {
            var paddingChar = _option.PaddingChar ?? ' ';
            var res = inProp.PadRight(_option.Lenght, paddingChar);
            return res;
        }
    }
}