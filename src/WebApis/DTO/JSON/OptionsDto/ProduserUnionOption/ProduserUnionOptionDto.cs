using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Device.Repository.Entities.ResponseProduser;
using KafkaProduser.Options;
using WebClientProduser.Options;

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

        public List<KafkaProduserOption> KafkaProduserOptions { get; set; } = new List<KafkaProduserOption>();
        public List<SignalRProduserOption> SignalRProduserOptions { get; set; } = new List<SignalRProduserOption>();
        public List<WebClientProduserOption> WebClientProduserOptions { get; set; } = new List<WebClientProduserOption>();
    }
}