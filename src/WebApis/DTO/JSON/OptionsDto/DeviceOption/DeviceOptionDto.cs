using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption;

namespace WebApiSwc.DTO.JSON.OptionsDto.DeviceOption
{
    public class DeviceOptionDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите название устройства")]
        public string Name { get; set; }

        public string TopicName4MessageBroker { get; set; }   
        public string Description { get; set; }
        public bool AutoBuild { get; set; }                        
        //public bool AutoStart{ get; set; }                       

        [Required(ErrorMessage = "Список ключей ExchangeKeys не может быть пуст")]
        public List<string> ExchangeKeys { get; set; }
        public MiddleWareInDataOptionDto MiddleWareInData { get; set; }
    }
}