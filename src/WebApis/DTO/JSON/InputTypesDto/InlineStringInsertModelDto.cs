using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.InputTypesDto
{
    public class InlineStringInsertModelDto
    {
        [Required(ErrorMessage = "Укажите Key для InlineStringInsertModel")]
        public string Key { get; set; }

        [Required(ErrorMessage = "Укажите InlineStr для InlineStringInsertModel")]
        public string InlineStr { get; set; }

        public string Description { get; set; }
    }
}