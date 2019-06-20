namespace DAL.Abstract.Entities.Options.MiddleWare.Converters.DateTimeConverterOption
{
    /// <summary>
    /// корректировка DateTime по часовому поясу TimeZone
    /// </summary>
    public class TimeZoneConverterOption : BaseConverterOption
    {
        public string TimeZone { get; set; }
    }
}