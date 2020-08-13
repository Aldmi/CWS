using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace WebApiSwc.DTO.JSON.OptionsDto.ProduserUnionOption
{
    public class WebClientProduserOptionDto : BaseProduserOptionDto
    {
        [Required(ErrorMessage = "Укажите InitUrl для WebClientProduser")]
        public string InitUrl { get; set; }

        [Required(ErrorMessage = "Укажите BoardDataUrl для WebClientProduser")]
        public string BoardDataUrl { get; set; }

        [Required(ErrorMessage = "Укажите InfoUrl для WebClientProduser")]
        public string InfoUrl { get; set; }

        [Required(ErrorMessage = "Укажите WarningUrl для WebClientProduser")]
        public string WarningUrl { get; set; }


        public HttpMethode HttpMethode { get; set; }
    }
}