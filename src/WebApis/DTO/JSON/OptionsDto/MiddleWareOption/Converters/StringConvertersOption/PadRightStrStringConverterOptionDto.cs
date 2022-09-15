using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption
{
    public class PadRightStrStringConverterOptionDto
    {
        [Range(1, 1000)]
        public int Lenght { get; set; }
        
        [Required]
        public string PaddingStr { get; set; }
    }
}