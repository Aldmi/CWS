using System.Collections.Generic;
using DAL.Abstract.Entities.Options.MiddleWare.Hadlers.StringConvertersOption;
using SerilogTimings;

namespace DAL.Abstract.Entities.Options.MiddleWare.Hadlers
{
    public class StringHandlerMiddleWareOption //TODO: вынести функционал в базовый класс
    {
        public string PropName { get; set; }                       //Имя свойства для обработки

        public InseartStringConverterOption InseartStringConverterOption { get; set; }
        public LimitStringConverterOption LimitStringConverterOption { get; set; }
        public ReplaceEmptyStringConverterOption ReplaceEmptyStringConverterOption { get; set; }
    }
}