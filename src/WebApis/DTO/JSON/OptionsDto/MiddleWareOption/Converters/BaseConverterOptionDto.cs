using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters
{
    public abstract class BaseConverterOptionDto
    {
        [Range(0, 100, ErrorMessage = "Priority Должен быть заданн в диапазоне 0...100")]
        public int Priority { get; set; }
    }
}