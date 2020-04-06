using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption
{
    public class PadRighCharWeightStringConverterOptionDto
    {
        [Range(1, 1000)]
        public int Lenght { get; set; }

        [Required(ErrorMessage = "DictWeight. Словарь весов не может быть NULL")]
        public Dictionary<string, int> DictWeight { get; set; }

        public char Pixel { get; set; }
    }
}