using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.ObjectConvertersOption
{
    public class CreepingLineRunningConvertertOptionDto
    {
        [Required(ErrorMessage = "CreepingLineRunningConvertertOption. String4Reset не может быть NULL", AllowEmptyStrings = true)]
        public string String4Reset { get; set; }

        [Range(1, 1000)]
        public int Length { get; set; }

        [Required(ErrorMessage = "CreepingLineRunningConvertertOption. Separator не может быть NULL")]
        public char Separator { get; set; }
    }
}