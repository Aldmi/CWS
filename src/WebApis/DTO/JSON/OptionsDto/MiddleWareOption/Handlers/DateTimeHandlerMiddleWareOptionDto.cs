using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.DateTimeConverterOption;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers
{
    public class DateTimeHandlerMiddleWareOptionDto
    {
        public List<UnitDateTimeConverterOptionDto> Converters { get; set; }
    }

    public class UnitDateTimeConverterOptionDto
    {
        public TimeZoneConverterOptionDto TimeZoneConverterOption { get; set; }
    }
}