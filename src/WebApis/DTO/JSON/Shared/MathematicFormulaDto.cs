using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSwc.DTO.JSON.Shared
{
    public class MathematicFormulaDto
    {
        [Required(ErrorMessage = "Укажите математическое выражение с переменной 'x'")]
        public string Expr { get; set; }
    }
}
