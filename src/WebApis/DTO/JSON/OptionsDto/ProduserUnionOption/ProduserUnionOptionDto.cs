using System.Collections.Generic;
using DAL.Abstract.Entities.Options.ResponseProduser;
using KafkaProduser.Options;
using WebClientProduser.Options;

namespace WebApiSwc.DTO.JSON.OptionsDto.ProduserUnionOption
{
    public class ProduserUnionOptionDto
    {

        public string Key { get; set; }
        public string ConverterName { get; set; }

        public List<KafkaProduserOption> KafkaProduserOptions { get; set; } = new List<KafkaProduserOption>();
        public List<SignalRProduserOption> SignalRProduserOptions { get; set; } = new List<SignalRProduserOption>();
        public List<WebClientProduserOption> WebClientProduserOptions { get; set; } = new List<WebClientProduserOption>();
    }
}