using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.EnumsConvertersOption;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers
{  
    public class EnumMiddleWareOptionDto
    {
        [Required(ErrorMessage = "Укажите PropName")]
        public string PropName { get; set; }
        [Required(ErrorMessage = "Укажите Path2Type")]
        public string Path2Type { get; set; }
        public List<UnitEnumConverterOptionDto> Converters { get; set; }
    }

    public class UnitEnumConverterOptionDto
    {
        public EnumMemConverterOptionDto EnumMemConverterOption { get; set; }
    }

}