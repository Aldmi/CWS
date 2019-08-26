using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.ProduserUnionOption
{
    public class SignalRProduserOptionDto : BaseProduserOptionDto
    {
        [Required(ErrorMessage = "Укажите MethodeName для SignalRProduser")]
        public string MethodeName { get; set; }
    }
}