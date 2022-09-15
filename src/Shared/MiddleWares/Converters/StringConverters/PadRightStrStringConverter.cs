using System;
using System.Text;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;

namespace Shared.MiddleWares.Converters.StringConverters
{
    /// <summary>
    /// Дополняет строку, строкой _option.PaddingStr, до длинны указанной в _option.Lenght.
    /// Если длинна строки больше или равна _option.Lenght, то дополнение не происходит.
    /// </summary>
    public class PadRightStrStringConverter  : BaseStringConverter
    {
        private readonly PadRightStrStringConverterOption _option;
        public PadRightStrStringConverter(PadRightStrStringConverterOption option)
        {
            _option = option;
        }

        /// <summary>
        /// null- заменяю на String.Empty, и дополняет _option.PaddingStr
        /// </summary>
        protected override string NullHandler()
        {
            return ConvertChild(String.Empty,  1);
        }
        
        protected override string ConvertChild(string inProp, int dataId)
        {
            var diff =_option.Lenght - inProp.Length;
            if (diff <= 0) 
                return inProp;
            
            var sb = new StringBuilder(inProp);
            for (int i = 0; i < diff; i++)
            {
                sb.Append(_option.PaddingStr);
            }
            return sb.ToString();
        }
    }
}