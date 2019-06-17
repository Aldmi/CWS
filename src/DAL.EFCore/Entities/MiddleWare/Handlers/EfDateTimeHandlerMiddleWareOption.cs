using DAL.EFCore.Entities.MiddleWare.Converters.DateTimeConverterOption;

namespace DAL.EFCore.Entities.MiddleWare.Handlers
{
    public class EfDateTimeHandlerMiddleWareOption
    {
        public string PropName { get; set; }                       //Имя свойства для обработки

        public EfTimeZoneConverterOption TimeZoneConverterOption { get; set; }
    }
}