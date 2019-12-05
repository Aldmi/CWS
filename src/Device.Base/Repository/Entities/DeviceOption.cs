using System.Collections.Generic;
using Domain.Device.Repository.Entities.MiddleWareOption;
using Infrastructure.Dal.Abstract;

namespace Domain.Device.Repository.Entities
{
    public class DeviceOption : EntityBase
    {
        public string Name { get; set; }
        public string ProduserUnionKey { get; set; }         //Название топика для брокера обмена
        public string Description { get; set; }
        public bool AutoBuild { get; set; }                         //Автоматичекое создание Deivice на базе DeviceOption, при запуске сервиса.
        //public bool AutoStart{ get; set; }                        //Автоматичекий запук Deivice в работу (после AutoBuild), при запуске сервиса.
        public List<string> ExchangeKeys { get; set; }
        public MiddleWareInDataOption MiddleWareInData { get; set; }
    }
}