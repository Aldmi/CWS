using DAL.Abstract.Entities.Options.MiddleWare.Converters.DateTimeConverterOption;

namespace DAL.Abstract.Entities.Options.MiddleWare.Handlers
{
    public class DateTimeHandlerMiddleWareOption
    {
        public string PropName { get; set; }                       //Имя свойства для обработки

        public TimeZoneConverterOption TimeZoneConverterOption { get; set; }
    }
}