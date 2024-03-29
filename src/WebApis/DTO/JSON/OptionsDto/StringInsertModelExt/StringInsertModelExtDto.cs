﻿using System.ComponentModel.DataAnnotations;
using Shared.Types;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers;
using WebApiSwc.DTO.JSON.Shared;

namespace WebApiSwc.DTO.JSON.OptionsDto.StringInsertModelExt
{
    public class StringInsertModelExtDto
    {
        [Required(ErrorMessage = "Укажите Key для StringInsertModelExt")]
        public string Key { get; set; }
        public string Format { get; set; }

        public StringHandlerMiddleWareOptionDto StringHandlerMiddleWareOption { get; set; }

        public BorderSubString BorderSubString { get; set; }

        public MathematicFormulaDto MathematicFormula { get; set; }
    }
}