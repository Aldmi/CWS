using Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.DateTimeConverterOption;

namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Handlers
{
    public class EfDateTimeHandlerMiddleWareOption
    {
        public string PropName { get; set; }                       //Имя свойства для обработки

        public EfTimeZoneConverterOption TimeZoneConverterOption { get; set; }
    }
}