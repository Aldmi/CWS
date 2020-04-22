using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.EnumsConvertersOption;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers
{  
    public class EnumHandlerMiddleWareOptionDto
    {
        public List<UnitEnumConverterOptionDto> Converters { get; set; }
    }

    public class UnitEnumConverterOptionDto
    {
        public EnumMemConverterOptionDto EnumMemConverterOption { get; set; }
    }
}