using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters
{
    public abstract class BaseConverterOptionDto
    {
        [Range(1, 100, ErrorMessage = "Priority Должен быть задан в диапазоне 1...100")]
        public int Priority { get; set; }
    }
}