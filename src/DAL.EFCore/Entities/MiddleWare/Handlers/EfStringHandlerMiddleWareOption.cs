using DAL.EFCore.Entities.MiddleWare.Converters.StringConvertersOption;

namespace DAL.EFCore.Entities.MiddleWare.Handlers
{
    public class EfStringHandlerMiddleWareOption
    {
        public string PropName { get; set; }
        public EfInseartStringConverterOption InseartStringConverterOption { get; set; }
        public EfLimitStringConverterOption LimitStringConverterOption { get; set; }
        public EfReplaceEmptyStringConverterOption ReplaceEmptyStringConverterOption { get; set; }
        public EfSubStringMemConverterOption SubStringMemConverterOption { get; set; }
    }
}