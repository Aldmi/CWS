using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption
{
    public class SubStringMemConverterOptionDto
    {
        [Range(1, 1000)]
        public int Lenght { get; set; }

        public List<string> InitPharases { get; set; }

        [Required(ErrorMessage = " SubStringMemConverterOption. Separator не может быть NULL")]
        public char Separator { get; set; }
    }
}