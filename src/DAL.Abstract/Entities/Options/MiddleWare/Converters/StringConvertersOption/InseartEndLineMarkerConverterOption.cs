namespace DAL.Abstract.Entities.Options.MiddleWare.Converters.StringConvertersOption
{
    public class InseartEndLineMarkerConverterOption : BaseConverterOption
    {
        public int LenghtLine { get; set; }  //Длинна строки, после которой вставляется маркер конца строки
        public string Marker { get; set; }  //Маркер конца строки
    }
}