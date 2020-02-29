using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Dal.EfCore.Entities.MiddleWare;
using Innofactor.EfCoreJsonValueConverter;
using Newtonsoft.Json;

namespace Infrastructure.Dal.EfCore.Entities.Device
{
    public class EfDeviceOption : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        //DEBUG migrations--------------------
        //public string NameNew111 { get; set; }

        //[Required]
        //[MaxLength(256)]
        //public string NameNew222 { get; set; }

        //[Required]
        //[MaxLength(256)]
        //public string NameNew333 { get; set; }
        //DEBUG migrations--------------------

        [Required]
        [MaxLength(256)]
        public string ProduserUnionKey { get; set; }            

        [Required]
        public string Description { get; set; }
        public bool AutoBuild { get; set; }                         //Автоматичекое создание Deivice на базе DeviceOption, при запуске сервиса.
       // public bool AutoStart { get; set; }                       //Автоматичекий запук Deivice в работу (после AutoBuild), при запуске сервиса.

        [JsonField]
        public string[] ExchangeKeys { get; set; }

        [JsonField]
        public EfMiddleWareInDataOption MiddleWareInData { get; set; }
    }
}