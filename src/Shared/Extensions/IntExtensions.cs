namespace Shared.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        /// Конвертировать Int по формату
        /// </summary>
        public static string Convert2StrByFormat<T>(this T val, string formatValue) where T: struct
        {
            var format = "{0" + formatValue + "}";
            return string.Format(format, val);
        }
    }
}