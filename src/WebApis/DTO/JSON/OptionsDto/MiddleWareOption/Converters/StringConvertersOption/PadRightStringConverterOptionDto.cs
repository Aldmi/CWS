﻿using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption
{
    public class PadRightStringConverterOptionDto
    {
        [Range(1, 1000)]
        public int Lenght { get; set; }
        
        //[RegularExpression(@"^\w$", ErrorMessage = "PaddingChar Должен быть одним символом")]
        public char? PaddingChar { get; set; }
    }
}