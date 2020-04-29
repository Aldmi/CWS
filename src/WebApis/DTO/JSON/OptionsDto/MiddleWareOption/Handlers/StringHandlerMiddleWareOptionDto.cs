﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers
{
    public class StringHandlerMiddleWareOptionDto
    {
        public List<UnitStringConverterOptionDto> Converters { get; set; }
    }

    public class UnitStringConverterOptionDto
    {
        public InseartStringConverterOptionDto InseartStringConverterOption { get; set; }
        public LimitStringConverterOptionDto LimitStringConverterOption { get; set; }
        public ReplaceEmptyStringConverterOptionDto ReplaceEmptyStringConverterOption { get; set; }
        public ReplaceSpecStringConverterOptionDto ReplaceSpecStringConverterOption { get; set; }
        public SubStringMemConverterOptionDto SubStringMemConverterOption { get; set; }
        public InseartEndLineMarkerConverterOptionDto InseartEndLineMarkerConverterOption { get; set; }
        public InsertAtEndOfLineConverterOptionDto InsertAtEndOfLineConverterOption { get; set; }
        public PadRightStringConverterOptionDto PadRightStringConverterOption { get; set; }
        public PadRighCharWeightStringConverterOptionDto PadRighCharWeightStringConverterOption { get; set; }
    }
} 