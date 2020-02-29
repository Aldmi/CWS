namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.StringConvertersOption
{
    public class EfReplaceSpecStringConverterOption : EfBaseConverterOption
    {
        public string SpecString { get; set; }                       
        public string ReplacementString { get; set; }        
    }
}