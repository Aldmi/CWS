namespace Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.DateTimeConverterOption
{
    /// <summary>
    /// корректировка DateTime по часовому поясу TimeZone
    /// </summary>
    public class TimeZoneConverterOption : BaseConverterOption
    {
        public string TimeZone { get; set; }
    }
}