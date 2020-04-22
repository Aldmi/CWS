using System.Collections.Generic;
using Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.StringConvertersOption;

namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Handlers
{
    public class EfStringHandlerMiddleWare4InDataOption : EfStringHandlerMiddleWareOption
    {
        public string PropName { get; set; }
    }
}