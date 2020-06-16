using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption
{
    public class PadRighOptimalFillingConverterOptionDto
    {
        [Range(1, 1000)]
        public int Lenght { get; set; }

        [Required(ErrorMessage = "DictWeight. Словарь весов не может быть NULL")]
        public Dictionary<string, string> DictWeight { get; set; }
    }
}