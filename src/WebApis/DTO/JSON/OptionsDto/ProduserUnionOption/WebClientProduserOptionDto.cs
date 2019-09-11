using System.ComponentModel.DataAnnotations;
using WebClientProduser.Options;

namespace WebApiSwc.DTO.JSON.OptionsDto.ProduserUnionOption
{
    public class WebClientProduserOptionDto : BaseProduserOptionDto
    {
        [Required(ErrorMessage = "Укажите Url для WebClientProduser")]
        public string Url { get; set; }
        public HttpMethode HttpMethode { get; set; }
    }
}