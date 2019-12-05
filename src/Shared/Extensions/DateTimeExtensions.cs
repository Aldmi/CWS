using System;
using System.Text.RegularExpressions;

namespace Shared.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Вставка DateTime по формату
        /// </summary>
        public static string DateTime2StrByFormat(this DateTime val, string formatValue)
        {
            const string defaultStr = " ";
            if (val == DateTime.MinValue)
                return defaultStr;

            object resVal;
            if (formatValue.Contains("Sec")) //формат задан в секундах
            {
                resVal = (val.Hour * 3600 + val.Minute * 60);
                formatValue = Regex.Match(formatValue, @"\((.*)\)").Groups[1].Value;
            }
            else
            if (formatValue.Contains("Min")) //формат задан в минутах
            {
                resVal = (val.Hour * 60 + val.Minute);
                formatValue = Regex.Match(formatValue, @"\((.*)\)").Groups[1].Value;
            }
            else
            {
                resVal = val;
            }
            var format = "{0" + formatValue + "}";
            return string.Format(format, resVal);
        }

    }
}