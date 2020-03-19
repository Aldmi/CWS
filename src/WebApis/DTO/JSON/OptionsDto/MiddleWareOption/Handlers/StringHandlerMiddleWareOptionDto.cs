using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.StringConvertersOption;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers
{
    public class StringHandlerMiddleWareOptionDto
    {
        public string PropName { get; set; }                       //Имя свойства для обработки

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