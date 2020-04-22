using System.ComponentModel.DataAnnotations;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers4InData
{
    public class StringHandlerMiddleWare4InDataOptionDto : StringHandlerMiddleWareOptionDto
    {
        [Required(ErrorMessage = "Укажите PropName")]
        public string PropName { get; set; }
    }
} 