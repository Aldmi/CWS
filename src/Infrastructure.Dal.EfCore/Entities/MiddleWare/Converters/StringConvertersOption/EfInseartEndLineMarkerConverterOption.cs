namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.StringConvertersOption
{
    public class EfInseartEndLineMarkerConverterOption : EfBaseConverterOption
    {
        public int LenghtLine { get; set; }  //Длинна строки, после которой вставляется маркер конца строки
        public string Marker { get; set; }  //Маркер конца строки
    }
}