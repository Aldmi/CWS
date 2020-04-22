using System.ComponentModel.DataAnnotations;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers4InData
{  
    public class EnumHandlerMiddleWare4InDataOptionDto : EnumHandlerMiddleWareOptionDto
    {
        [Required(ErrorMessage = "Укажите PropName")]
        public string PropName { get; set; }
        [Required(ErrorMessage = "Укажите Path2Type")]
        public string Path2Type { get; set; }
    }

}