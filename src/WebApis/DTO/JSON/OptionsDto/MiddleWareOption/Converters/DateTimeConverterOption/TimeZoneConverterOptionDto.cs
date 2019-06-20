namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.DateTimeConverterOption
{
    /// <summary>
    /// корректировка DateTime по часовому поясу TimeZone
    /// </summary>
    public class TimeZoneConverterOptionDto : BaseConverterOptionDto
    {
        public string TimeZone { get; set; }
    }
}