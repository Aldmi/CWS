using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.DateTimeConverterOption;

namespace Domain.Device.Repository.Entities.MiddleWareOption.HandlersOption
{
    public class DateTimeHandlerMiddleWareOption
    {
        public string PropName { get; set; }                       //Имя свойства для обработки

        public TimeZoneConverterOption TimeZoneConverterOption { get; set; }
    }
}