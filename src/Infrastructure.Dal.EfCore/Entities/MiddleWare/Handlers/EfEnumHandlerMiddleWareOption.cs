using Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.EnumsConvertersOption;
using Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.StringConvertersOption;

namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Handlers
{
    public class EfEnumHandlerMiddleWareOption
    {
        public string PropName { get; set; }                       //Имя свойства для обработки

        public EfEnumerateConverterOption EnumerateConverterOption { get; set; }

    }
}