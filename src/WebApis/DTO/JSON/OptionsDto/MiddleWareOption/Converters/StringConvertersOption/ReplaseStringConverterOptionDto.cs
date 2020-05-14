using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption
{
    public class ReplaseStringConverterOptionDto
    {
        [Required(ErrorMessage = "Mapping. Словарь весов не может быть NULL")]
        public Dictionary<string, string> Mapping { get; set; }
        public bool ToLowerInvariant { get; set; }
    }
}