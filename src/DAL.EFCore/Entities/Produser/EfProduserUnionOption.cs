using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Abstract.Entities.Options.MiddleWare;
using DAL.Abstract.Entities.Options.ResponseProduser;
using KafkaProduser.Options;
using Newtonsoft.Json;
using WebClientProduser.Options;

namespace DAL.EFCore.Entities.Produser
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
        public List<KafkaProduserOption> KafkaProduserOptions
        {
            get => string.IsNullOrEmpty(_kafkaProduserOptionsMetaData) ? null : JsonConvert.DeserializeObject<List<KafkaProduserOption>>(_kafkaProduserOptionsMetaData);
            set => _kafkaProduserOptionsMetaData = (value == null) ? null : JsonConvert.SerializeObject(value);
        }

        private string _signalRProduserOptionsMetaData;
        [NotMapped]
        public List<SignalRProduserOption> SignalRProduserOptions
        {
            get => string.IsNullOrEmpty(_signalRProduserOptionsMetaData) ? null : JsonConvert.DeserializeObject<List<SignalRProduserOption>>(_signalRProduserOptionsMetaData);
            set => _signalRProduserOptionsMetaData = (value == null) ? null : JsonConvert.SerializeObject(value);
        }

        private string _webClientProduserOptionsMetaData;
        [NotMapped]
        public List<WebClientProduserOption> WebClientProduserOptions
        {
            get => string.IsNullOrEmpty(_webClientProduserOptionsMetaData) ? null : JsonConvert.DeserializeObject<List<WebClientProduserOption>>(_webClientProduserOptionsMetaData);
            set => _webClientProduserOptionsMetaData = (value == null) ? null : JsonConvert.SerializeObject(value);
        }
    }
}