using System.Collections.Generic;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.ObjectConvertersOption;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers
{
    public class ObjectHandlerMiddleWareOptionDto
    {
        public List<UnitObjectConverterOptionDto> Converters { get; set; }
    }

    public class UnitObjectConverterOptionDto
    {
        public CreepingLineRunningConvertertOptionDto CreepingLineRunningConvertertOption { get; set; }
    }
}