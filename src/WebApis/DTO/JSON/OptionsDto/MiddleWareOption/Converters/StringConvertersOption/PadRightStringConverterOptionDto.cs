using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption
{
    public class PadRightStringConverterOptionDto : BaseConverterOptionDto
    {
        [Range(1, 1000)]
        public int Lenght { get; set; }
    }
}