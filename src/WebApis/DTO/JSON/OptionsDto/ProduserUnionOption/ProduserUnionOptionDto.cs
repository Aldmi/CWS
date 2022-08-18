using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Device.Repository.Entities.ResponseProduser;
using Infrastructure.Produser.KafkaProduser.Options;
using Infrastructure.Produser.WebClientProduser.Options;

namespace WebApiSwc.DTO.JSON.OptionsDto.ProduserUnionOption
{
    public class ProduserUnionOptionDto
    {
        [Range(1, 10000, ErrorMessage = "Id не попал в диапазон 1...10000")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите Key для ProduserUnionOption")]
        public string Key { get; set; }

        [Required(ErrorMessage = "Укажите ConverterName для ProduserUnionOption")]
        public string ConverterName { get; set; }

        public List<KafkaProduserOptionDto> KafkaProduserOptions { get; set; } = new List<KafkaProduserOptionDto>();
        public List<SignalRProduserOptionDto> SignalRProduserOptions { get; set; } = new List<SignalRProduserOptionDto>();
        public List<WebClientProduserOptionDto> WebClientProduserOptions { get; set; } = new List<WebClientProduserOptionDto>();
    }
}