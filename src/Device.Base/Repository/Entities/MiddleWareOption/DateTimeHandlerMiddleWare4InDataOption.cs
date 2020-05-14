using Shared.MiddleWares.Handlers;
using Shared.MiddleWares.HandlersOption;

namespace Domain.Device.Repository.Entities.MiddleWareOption
{
    public class DateTimeHandlerMiddleWare4InDataOption : DateTimeHandlerMiddleWareOption
    {
        public string PropName { get; set; }                       //Имя свойства для обработки
    }
}