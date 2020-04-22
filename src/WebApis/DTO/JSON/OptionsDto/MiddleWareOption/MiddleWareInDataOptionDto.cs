using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Device.Enums;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers4InData;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption
{
    public class MiddleWareInDataOptionDto
    {
        public string Description { get; set; }

        public List<StringHandlerMiddleWare4InDataOptionDto> StringHandlers { get; set; }
        public List<DateTimeHandlerMiddleWare4InDataOptionDto> DateTimeHandlers { get; set; }
        public List<EnumHandlerMiddleWare4InDataOptionDto> EnumHandlers { get; set; }

        [Required(ErrorMessage = "Укажите InvokerOutput")]
        public InvokerOutputDto InvokerOutput { get; set; }
    }


    public class InvokerOutputDto
    {
        public InvokerOutputMode Mode { get; set; }

        [Range(1000, 3600000, ErrorMessage = "Time не попал в диапазон 1000...3600000 мсек")]
        public int Time { get; set; }      
    }
}