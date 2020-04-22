using System.Collections.Generic;
using Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.DateTimeConverterOption;

namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Handlers
{
    public class EfDateTimeHandlerMiddleWare4InDataOption : EfDateTimeHandlerMiddleWareOption
    {
        public string PropName { get; set; }
    }


}