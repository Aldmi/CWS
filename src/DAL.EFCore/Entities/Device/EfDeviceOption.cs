using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Abstract.Entities.Options.MiddleWare;
using Newtonsoft.Json;

namespace DAL.EFCore.Entities.Device
{
    public class EfDeviceOption : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        //public string NameNew111 { get; set; }

        [Required]
        [MaxLength(256)]
        public string TopicName4MessageBroker { get; set; }         //Название топика для брокера обмена

        [Required]
        public string Description { get; set; }
        public bool AutoBuild { get; set; }                         //Автоматичекое создание Deivice на базе DeviceOption, при запуске сервиса.
        public bool AutoStart { get; set; }                         //Автоматичекий запук Deivice в работу (после AutoBuild), при запуске сервиса.

        private string _exchangeKeysMetaData;
        [NotMapped]
        public string[] ExchangeKeys
        {
            get => _exchangeKeysMetaData.Split(';');
            set => _exchangeKeysMetaData = string.Join($"{';'}", value);
        }

        private string _middleWareInDataMetaData;
        [NotMapped]
        public MiddleWareInDataOption MiddleWareInData
        {
            get => string.IsNullOrEmpty(_middleWareInDataMetaData) ? null : JsonConvert.DeserializeObject<MiddleWareInDataOption>(_middleWareInDataMetaData);
            set => _middleWareInDataMetaData = (value == null) ? null : JsonConvert.SerializeObject(value);
        }
    }
}