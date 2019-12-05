using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Device.Enums;
using Domain.Device.Repository.Entities.MiddleWareOption;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption
{
    public class MiddleWareInDataOptionDto
    {
        public string Description { get; set; }

        public List<StringHandlerMiddleWareOptionDto> StringHandlers { get; set; }
        public List<DateTimeHandlerMiddleWareOptionDto> DateTimeHandlers { get; set; }

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