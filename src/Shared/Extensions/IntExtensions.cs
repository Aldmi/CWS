using System;
using Shared.Enums;

namespace Shared.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        /// Конвертировать Int по формату.
        /// </summary>
        public static string Convert2StrByFormat<T>(this T val, string formatValue) where T : struct  //notnull
        {
            var format = "{0" + formatValue + "}";
            var formatStr= string.Format(format, val);
            return formatStr;
        }
    }
}