using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.ProduserUnionOption
{
    public class KafkaProduserOptionDto : BaseProduserOptionDto
    {
        [Required(ErrorMessage = "Укажите BrokerEndpoints для KafkaProduser")]
        public string BrokerEndpoints { get; set; }
        [Required(ErrorMessage = "Укажите TopicName для KafkaProduser")]
        public string TopicName { get; set; }
    }
}