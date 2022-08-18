using System.Collections.Generic;
using Domain.Device.MiddleWares4InData;
using Infrastructure.Dal.Abstract;
using Shared.Paged;

namespace Domain.Device.Repository.Entities
{
    public class DeviceOption : EntityBase
    {
        public string Name { get; set; }
        public string ProduserUnionKey { get; set; }                 //Название топика для брокера обмена
        public string Description { get; set; }
        public bool AutoBuild { get; set; }                         //Автоматичекое создание Deivice на базе DeviceOption, при запуске сервиса.
        //public bool AutoStart{ get; set; }                        //Автоматичекий запук Deivice в работу (после AutoBuild), при запуске сервиса.
        public List<string> ExchangeKeys { get; set; }
        public MiddleWareMediatorOption MiddleWareMediator { get; set; }
        public PagedOption Paging { get; set; }
    }
}