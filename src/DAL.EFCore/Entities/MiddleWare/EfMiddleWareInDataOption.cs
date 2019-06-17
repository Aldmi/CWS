using System.Collections.Generic;
using DAL.Abstract.Entities;
using DAL.Abstract.Entities.Options.MiddleWare;
using DAL.EFCore.Entities.MiddleWare.Handlers;

namespace DAL.EFCore.Entities.MiddleWare
{
    public class EfMiddleWareInDataOption : EntityBase
    {
        public string Description { get; set; }

        public List<EfStringHandlerMiddleWareOption> StringHandlers { get; set; }
        public List<EfDateTimeHandlerMiddleWareOption> DateTimeHandlers { get; set; }

        public EfInvokerOutput InvokerOutput { get; set; }
    }


    public class EfInvokerOutput
    {
        public InvokerOutputMode Mode { get; set; }
        public int Time { get; set; } 
    }
}