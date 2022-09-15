using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption
{
    public class InsertAtEndOfLineConverterOptionDto
    {
        [Required(AllowEmptyStrings = true)]
        [StringLength(50, MinimumLength = 1)]
        public string EndLine { get; set; }
    }
}