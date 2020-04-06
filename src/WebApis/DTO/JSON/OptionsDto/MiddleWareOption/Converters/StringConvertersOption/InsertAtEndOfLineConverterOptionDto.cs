using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption
{
    public class InsertAtEndOfLineConverterOptionDto
    {
        [Required(ErrorMessage = "InsertAtEndOfLineConverterOption. Строка для вставки не может быть NULL")]
        public string EndLine { get; set; }
    }
}