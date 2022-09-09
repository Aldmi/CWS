using Shared.MiddleWares.Converters.Exceptions;

namespace Shared.MiddleWares.ConvertersOption.DateTimeConverterOption
{
    /// <summary>
    /// корректировка DateTime по часовому поясу TimeZone
    /// </summary>
    public class TimeZoneConverterOption
    {
        public string TimeZone { get; set; }


        public int GetIncreaseInTime()
        {
            if (int.TryParse(TimeZone, out var res))
            {
                return res;
            }
            throw new DateTimeConverterException("TimeZone не может быть интерпретировано");
        }
    }
}