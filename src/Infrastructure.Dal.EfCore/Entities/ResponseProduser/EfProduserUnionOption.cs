using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Dal.EfCore.Entities.ResponseProduser.Produssers;
using Newtonsoft.Json;

namespace Infrastructure.Dal.EfCore.Entities.Produser
{
    public class EfProduserUnionOption : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Key { get; set; }
        public string ConverterName { get; set; }

        private string _kafkaProduserOptionsMetaData;
        [NotMapped]
        public List<EfKafkaProduserOption> KafkaProduserOptions
        {
            get => string.IsNullOrEmpty(_kafkaProduserOptionsMetaData) ? null : JsonConvert.DeserializeObject<List<EfKafkaProduserOption>>(_kafkaProduserOptionsMetaData);
            set => _kafkaProduserOptionsMetaData = (value == null) ? null : JsonConvert.SerializeObject(value);
        }

        private string _signalRProduserOptionsMetaData;
        [NotMapped]
        public List<EfSignalRProduserOption> SignalRProduserOptions
        {
            get => string.IsNullOrEmpty(_signalRProduserOptionsMetaData) ? null : JsonConvert.DeserializeObject<List<EfSignalRProduserOption>>(_signalRProduserOptionsMetaData);
            set => _signalRProduserOptionsMetaData = (value == null) ? null : JsonConvert.SerializeObject(value);
        }

        private string _webClientProduserOptionsMetaData;
        [NotMapped]
        public List<EfWebClientProduserOption> WebClientProduserOptions
        {
            get => string.IsNullOrEmpty(_webClientProduserOptionsMetaData) ? null : JsonConvert.DeserializeObject<List<EfWebClientProduserOption>>(_webClientProduserOptionsMetaData);
            set => _webClientProduserOptionsMetaData = (value == null) ? null : JsonConvert.SerializeObject(value);
        }
    }
}