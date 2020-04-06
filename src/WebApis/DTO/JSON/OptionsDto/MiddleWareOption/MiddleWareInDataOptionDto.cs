﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Device.Enums;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption
{
    public class MiddleWareInDataOptionDto
    {
        public string Description { get; set; }

        public List<StringMiddleWareOptionDto> StringHandlers { get; set; }
        public List<DateTimeMiddleWareOptionDto> DateTimeHandlers { get; set; }
        public List<EnumMiddleWareOptionDto> EnumHandlers { get; set; }

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