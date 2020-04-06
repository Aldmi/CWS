using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption
{
    public class InseartEndLineMarkerConverterOptionDto
    {
        [Range(1, 1000)]
        public int LenghtLine { get; set; }  //Длинна строки, после которой вставляется маркер конца строки

        [Required(ErrorMessage = " InseartEndLineMarkerConverterOption. Marker не может быть NULL")]
        public string Marker { get; set; }  //Маркер конца строки
    }
}