using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption
{
    public class ReplaceSpecStringConverterOptionDto : BaseConverterOptionDto
    {
        [Required(ErrorMessage = "ReplaceSpecStringConverterOptionDto. SpecString не может быть NULL")]
        public string SpecString { get; set; } 

        [Required(ErrorMessage = "ReplaceSpecStringConverterOptionDto. ReplacementString не может быть NULL")]
        public string ReplacementString { get; set; }
    }
}