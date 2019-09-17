using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.StringConvertersOption;

namespace Domain.Device.Repository.Entities.MiddleWareOption.HandlersOption
{
    public class StringHandlerMiddleWareOption
    {
        public string PropName { get; set; }                       //Имя свойства для обработки

        public InseartStringConverterOption InseartStringConverterOption { get; set; }
        public LimitStringConverterOption LimitStringConverterOption { get; set; }
        public ReplaceEmptyStringConverterOption ReplaceEmptyStringConverterOption { get; set; }
        public SubStringMemConverterOption SubStringMemConverterOption { get; set; }
        public InseartEndLineMarkerConverterOption InseartEndLineMarkerConverterOption { get; set; }
        public InsertAtEndOfLineConverterOption InsertAtEndOfLineConverterOption { get; set; }
    }
}