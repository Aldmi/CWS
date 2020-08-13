using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.ProduserUnionOption
{
    public class SignalRProduserOptionDto : BaseProduserOptionDto
    {
        [Required(ErrorMessage = "Укажите InitMethodeName для SignalRProduser")]
        public string InitMethodeName { get; set; }

        [Required(ErrorMessage = "Укажите BoardDataMethodeName для SignalRProduser")]
        public string BoardDataMethodeName { get; set; }

        [Required(ErrorMessage = "Укажите InfoMethodeName для SignalRProduser")]
        public string InfoMethodeName { get; set; }

        [Required(ErrorMessage = "Укажите WarningMethodeName для SignalRProduser")]
        public string WarningMethodeName { get; set; }
    }
}