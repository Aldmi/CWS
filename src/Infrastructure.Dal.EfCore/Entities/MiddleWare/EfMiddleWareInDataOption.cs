using System.Collections.Generic;
using Infrastructure.Dal.EfCore.Entities.MiddleWare.Handlers;

namespace Infrastructure.Dal.EfCore.Entities.MiddleWare
{
    public class EfMiddleWareInDataOption
    {
        public string Description { get; set; }

        public List<EfStringMiddleWareOption> StringHandlers { get; set; }
        public List<EfDateTimeMiddleWareOption> DateTimeHandlers { get; set; }
        public List<EfEnumMiddleWareOption> EnumHandlers { get; set; }

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