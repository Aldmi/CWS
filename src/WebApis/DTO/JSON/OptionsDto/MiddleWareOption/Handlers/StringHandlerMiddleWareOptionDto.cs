﻿using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers
{
    public class StringHandlerMiddleWareOptionDto
    {
        public string PropName { get; set; }                       //Имя свойства для обработки

        public InseartStringConverterOptionDto InseartStringConverterOption { get; set; }
        public LimitStringConverterOptionDto LimitStringConverterOption { get; set; }
        public ReplaceEmptyStringConverterOptionDto ReplaceEmptyStringConverterOption { get; set; }
        public SubStringMemConverterOptionDto SubStringMemConverterOption { get; set; }
        public InseartEndLineMarkerConverterOptionDto InseartEndLineMarkerConverterOption { get; set; }
    }
}