﻿using System.Collections.Generic;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.EnumsConvertersOption
{
    public class EnumerateConverterOptionDto: BaseConverterOptionDto
    {
        public Dictionary<string, int> DictChain { get; set; }
    }
}