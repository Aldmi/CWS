using System.Collections.Generic;
using Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.EnumsConvertersOption;

namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Handlers
{
    public class EfEnumHandlerMiddleWare4InDataOption : EfEnumHandlerMiddleWareOption
    {
        public string PropName { get; set; }
    }
}