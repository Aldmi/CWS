using System.ComponentModel.DataAnnotations;

namespace WebServer.DTO.JSON.OptionsDto.ExchangeOption.ProvidersOption
{
    public class ProviderOptionDto
    {  
        [Required(ErrorMessage = "Name для Provider не может быть NULL")]
        public string Name { get; set; } 
        public ByRulesProviderOptionDto ByRulesProviderOption { get; set; }
        public ManualProviderOptionDto ManualProviderOption { get; set; }
    }
}