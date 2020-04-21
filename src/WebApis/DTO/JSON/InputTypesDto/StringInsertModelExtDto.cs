using System.ComponentModel.DataAnnotations;
using Shared.Types;

namespace WebApiSwc.DTO.JSON.InputTypesDto
{
    public class StringInsertModelExtDto
    {
        [Required(ErrorMessage = "Укажите VarName для  StringInsertModelExt")]
        public string VarName { get; set; }

        public string Format { get; set; }

        public BorderSubString BorderSubString { get; set; }
    }
}