using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Dal.EfCore.Entities.ResponseProduser.Produssers;
using Innofactor.EfCoreJsonValueConverter;
using Newtonsoft.Json;

namespace Infrastructure.Dal.EfCore.Entities.ResponseProduser
{
    public class EfProduserUnionOption : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Key { get; set; }
        public string ConverterName { get; set; }

        [JsonField]
        public List<EfKafkaProduserOption> KafkaProduserOptions { get; set; }

        [JsonField]
        public List<EfSignalRProduserOption> SignalRProduserOptions { get; set; }

        [JsonField]
        public List<EfWebClientProduserOption> WebClientProduserOptions { get; set; }
    }
}