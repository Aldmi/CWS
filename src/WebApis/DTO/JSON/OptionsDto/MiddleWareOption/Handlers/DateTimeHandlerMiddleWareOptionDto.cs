using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.DateTimeConverterOption;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers
{
    public class DateTimeHandlerMiddleWareOptionDto
    {
        public string PropName { get; set; }                       //Имя свойства для обработки

        public TimeZoneConverterOptionDto TimeZoneConverterOption { get; set; }
    }
}