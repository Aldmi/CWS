using System.Collections.Generic;
using DAL.Abstract.Entities.Options.MiddleWare.Hadlers;
using SerilogTimings;

namespace DAL.Abstract.Entities.Options.MiddleWare
{
    public class MiddleWareInDataOption : EntityBase
    {
        public string Name { get; set; }

        List<StringHandlerMiddleWareOption> StringHandlers { get; set; }
        List<DateTimeHandlerMiddleWareOption> DateTimeHandlers { get; set; }

        public InvokerOutput InvokerOutput { get; set; }
    }







    public class InvokerOutput
    {
        public InvokerOutputMode Mode { get; set; }
        public int Time { get; set; }                        //Время сработки события отправки данных
    }
    public enum InvokerOutputMode {Instantly, ByTimer}
}