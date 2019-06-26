using System.Collections.Generic;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption
{
    public class SubStringMemConverterOptionDto : BaseConverterOptionDto
    {
        public int Lenght { get; set; }
        public List<string> InitPharases { get; set; }
    }
}