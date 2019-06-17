using System.Collections.Generic;
using DAL.Abstract.Entities.Options.MiddleWare.Handlers;

namespace DAL.Abstract.Entities.Options.MiddleWare
{
    public class MiddleWareInDataOption : EntityBase
    {
        public string Description { get; set; }

        public List<StringHandlerMiddleWareOption> StringHandlers { get; set; }
        public List<DateTimeHandlerMiddleWareOption> DateTimeHandlers { get; set; }

        public InvokerOutput InvokerOutput { get; set; }
    }



    public class InvokerOutput
    {
        public InvokerOutputMode Mode { get; set; }
        public int Time { get; set; }                        //Время сработки события отправки данных
    }
    public enum InvokerOutputMode {Instantly, ByTimer}
}