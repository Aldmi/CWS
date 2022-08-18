using System.Collections.Generic;
using Infrastructure.Dal.EfCore.Entities.MiddleWare.Handlers4InData;

namespace Infrastructure.Dal.EfCore.Entities.MiddleWare
{
    public class EfMiddleWareMediatorOption
    {
        public string Description { get; set; }

        public List<EfStringHandlerMiddleWare4InDataOption> StringHandlers { get; set; }
        public List<EfDateTimeHandlerMiddleWare4InDataOption> DateTimeHandlers { get; set; }
        public List<EfEnumHandlerMiddleWare4InDataOption> EnumHandlers { get; set; }
        public List<EfObjectHandlerMiddleWare4InDataOption> ObjectHandlers { get; set; }

        public EfInvokerOutput InvokerOutput { get; set; }
    }


    public class EfInvokerOutput
    {
        public EfInvokerOutputMode Mode { get; set; }
        public int Time { get; set; }                        //Время сработки события отправки данных
    }


    public enum EfInvokerOutputMode : byte
    {
        Instantly,
        ByTimer
    }
}