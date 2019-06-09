using DAL.Abstract.Entities.Options.MiddleWare.Hadlers.DateTimeConverterOption;

namespace DAL.Abstract.Entities.Options.MiddleWare.Hadlers
{
    public class DateTimeHandlerMiddleWareOption
    {
        public string PropName { get; set; }                       //Имя свойства для обработки

        public TimeZoneConverterOption TimeZoneConverterOption { get; set; }
    }
}