using Shared.Enums;

namespace Shared.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        /// Конвертировать Int по формату.
        /// </summary>
        public static string Convert2StrByFormat<T>(this T val, string formatValue) where T : struct
        {
            var format = "{0" + formatValue + "}";
            var formatStr= string.Format(format, val);
            return formatStr;
        }


        /// <summary>
        /// Конвертировать Int по формату. Для HEX представления указать строковый ращзделитель байт строку 0x.
        /// </summary>
        public static string Convert2StrByFormat<T>(this T val, string formatValue, ByteHexDelemiter hexDelemiter) where T : struct
        {
            var format = "{0" + formatValue + "}";
            var formatStr = string.Format(format, val);

            //вставить разделитель байт в получившуюся строку.
            if (hexDelemiter == ByteHexDelemiter.Hex)
            {
                var inseartVal = "0x";
                for (var i = 0; i < formatStr.Length; i += 2 + inseartVal.Length)
                {
                    formatStr = formatStr.Insert(i, inseartVal);
                }
            }
            return formatStr;
        }
    }
}