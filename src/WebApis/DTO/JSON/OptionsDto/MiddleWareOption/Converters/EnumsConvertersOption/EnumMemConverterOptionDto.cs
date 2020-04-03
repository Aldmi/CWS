using System.Collections.Generic;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.EnumsConvertersOption
{
    public class EnumMemConverterOptionDto: BaseConverterOptionDto
    {
        public Dictionary<string, int> DictChain { get; set; }
        public string Path2Type { get; set; }
    }
}