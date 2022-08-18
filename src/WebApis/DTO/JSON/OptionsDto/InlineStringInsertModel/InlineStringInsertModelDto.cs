using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.InlineStringInsertModel
{
    public class InlineStringInsertModelDto
    {
        [Required(ErrorMessage = "Укажите Key для InlineStringInsertModel")]
        [RegularExpression(@"\{\$[^{}:$]+\}", ErrorMessage = "Key не соответсвует формату {$Key}")]
        public string Key { get; set; }

        [Required(ErrorMessage = "Укажите InlineStr для InlineStringInsertModel")]
        public string InlineStr { get; set; }

        public string Description { get; set; }
    }
}