using System.Collections.Generic;
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
        public SubStringMemConverterOptionDto SubStringMemConverterOption { get; set; }
        public InseartEndLineMarkerConverterOptionDto InseartEndLineMarkerConverterOption { get; set; }
        public InsertAtEndOfLineConverterOptionDto InsertAtEndOfLineConverterOption { get; set; }
        public PadRightStringConverterOptionDto PadRightStringConverterOption { get; set; }
        public PadRighCharWeightStringConverterOptionDto PadRighCharWeightStringConverterOption { get; set; }
        public ReplaseStringConverterOptionDto ReplaseStringConverterOption { get; set; }
        public ReplaseCharStringConverterOptionDto ReplaseCharStringConverterOption { get; set; }
        public ToLowerConverterOptionDto ToLowerConverterOption { get; set; }
        public ToUpperConverterOptionDto ToUpperConverterOption { get; set; }
        public PadRighOptimalFillingConverterOptionDto PadRighOptimalFillingConverterOption { get; set; }
    }
} 