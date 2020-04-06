using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption
{
    public class ReplaceEmptyStringConverterOptionDto
    {
        [Required(ErrorMessage = "ReplaceEmptyStringConverterOption. Строка для замены не может быть NULL")]
        public string ReplacementString { get; set; }
    }
}