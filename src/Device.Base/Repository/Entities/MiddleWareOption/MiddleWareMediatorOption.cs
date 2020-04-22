using System.Collections.Generic;
using Domain.Device.Enums;

namespace Domain.Device.Repository.Entities.MiddleWareOption
{
    public class MiddleWareMediatorOption
    {
        public string Description { get; set; }

        public List<StringHandlerHandlerMiddleWare4InDataOption> StringHandlers { get; set; }
        public List<DateTimeHandlerHandlerMiddleWare4InDataOption> DateTimeHandlers { get; set; }
        public List<EnumHandlerHandlerMiddleWare4InDataOption> EnumHandlers { get; set; }

        public InvokerOutput InvokerOutput { get; set; }
    }


    public class InvokerOutput
    {
        public InvokerOutputMode Mode { get; set; }
        public int Time { get; set; }                        //Время сработки события отправки данных
    }
}