﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Abstract.Entities.Options.MiddleWare;
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
        public int Time { get; set; }      
    }
}