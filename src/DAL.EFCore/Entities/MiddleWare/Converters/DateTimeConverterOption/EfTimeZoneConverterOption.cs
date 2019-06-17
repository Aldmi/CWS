namespace DAL.EFCore.Entities.MiddleWare.Converters.DateTimeConverterOption
{
    /// <summary>
    /// корректировка DateTime по часовому поясу TimeZone
    /// </summary>
    public class EfTimeZoneConverterOption
    {
        public string TimeZone { get; set; }
    }
}