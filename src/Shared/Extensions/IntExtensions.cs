namespace Shared.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        /// Вставка Int по формату
        /// </summary>
        public static string Convert2StrByFormat(this int val, string formatValue)
        {
            var format = "{0" + formatValue + "}";
            return string.Format(format, val);
        }
    }
}