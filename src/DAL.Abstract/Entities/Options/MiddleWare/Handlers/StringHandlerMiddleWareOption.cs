using DAL.Abstract.Entities.Options.MiddleWare.Converters.StringConvertersOption;

namespace DAL.Abstract.Entities.Options.MiddleWare.Handlers
{
    public class StringHandlerMiddleWareOption //TODO: вынести функционал в базовый класс
    {
        public string PropName { get; set; }                       //Имя свойства для обработки

        public InseartStringConverterOption InseartStringConverterOption { get; set; }
        public LimitStringConverterOption LimitStringConverterOption { get; set; }
        public ReplaceEmptyStringConverterOption ReplaceEmptyStringConverterOption { get; set; }
        public SubStringMemConverterOption SubStringMemConverterOption { get; set; }
    }
}