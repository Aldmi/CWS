using Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.StringConvertersOption;

namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Handlers
{
    public class EfStringHandlerMiddleWareOption
    {
        public string PropName { get; set; }                       //Имя свойства для обработки

        public EfInseartStringConverterOption InseartStringConverterOption { get; set; }
        public EfLimitStringConverterOption LimitStringConverterOption { get; set; }
        public EfReplaceEmptyStringConverterOption ReplaceEmptyStringConverterOption { get; set; }
        public EfReplaceSpecStringConverterOption ReplaceSpecStringConverterOption { get; set; }
        public EfSubStringMemConverterOption SubStringMemConverterOption { get; set; }
        public EfInseartEndLineMarkerConverterOption InseartEndLineMarkerConverterOption { get; set; }
        public EfInsertAtEndOfLineConverterOption InsertAtEndOfLineConverterOption { get; set; }
        public EfPadRightStringConverterOption PadRightStringConverterOption { get; set; }
        public EfPadRighCharWeightStringConverterOption PadRighCharWeightStringConverterOption { get; set; }
    }
}