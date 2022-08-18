using System.ComponentModel.DataAnnotations;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Handlers4InData
{
    public class ObjectHandlerMiddleWare4InDataOptionDto : ObjectHandlerMiddleWareOptionDto
    {
        [Required(ErrorMessage = "Укажите PropName")]
        public string PropName { get; set; }
    }
}