using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption
{
    public class ReplaseCharStringConverterOptionDto
    {
        [Required(ErrorMessage = "Mapping. Словарь весов не может быть NULL")]
        public Dictionary<char, string> Mapping { get; set; }
        public bool ToLowerInvariant { get; set; }
    }
}