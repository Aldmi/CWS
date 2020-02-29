namespace Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.StringConvertersOption
{
    public class ReplaceSpecStringConverterOption : BaseConverterOption
    {
        public string SpecString { get; set; }                         //Строка для замены
        public string ReplacementString { get; set; }                 //Строка замены
    }
}