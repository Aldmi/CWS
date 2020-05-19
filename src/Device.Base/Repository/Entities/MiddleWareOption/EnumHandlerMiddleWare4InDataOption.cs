using Shared.MiddleWares.Handlers;
using Shared.MiddleWares.HandlersOption;

namespace Domain.Device.Repository.Entities.MiddleWareOption
{
    public class EnumHandlerMiddleWare4InDataOption : EnumHandlerMiddleWareOption
    {
        public string PropName { get; set; }                       //Имя свойства для обработки
    }
}