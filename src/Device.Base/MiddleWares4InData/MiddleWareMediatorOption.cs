using System.Collections.Generic;
using Domain.Device.Enums;
using Domain.Device.MiddleWares4InData.Handlers4InData;

namespace Domain.Device.MiddleWares4InData
{
    public class MiddleWareMediatorOption
    {
        public string Description { get; set; }

        public List<StringHandlerMiddleWare4InDataOption> StringHandlers { get; set; }
        public List<DateTimeHandlerMiddleWare4InDataOption> DateTimeHandlers { get; set; }
        public List<EnumHandlerMiddleWare4InDataOption> EnumHandlers { get; set; }
        public List<ObjectHandlerMiddleWare4InDataOption> ObjectHandlers { get; set; }

        public InvokerOutput InvokerOutput { get; set; }
    }


    public class InvokerOutput
    {
        public InvokerOutputMode Mode { get; set; }
        public int Time { get; set; }                        //Время сработки события отправки данных
    }
}