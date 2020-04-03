using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.EnumsConvertersOption;
using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.StringConvertersOption;

namespace Domain.Device.Repository.Entities.MiddleWareOption.HandlersOption
{
    public class EnumHandlerMiddleWareOption
    {
        public string PropName { get; set; }                       //Имя свойства для обработки

        public EnumMemConverterOption EnumMemConverterOption { get; set; }
    }
}