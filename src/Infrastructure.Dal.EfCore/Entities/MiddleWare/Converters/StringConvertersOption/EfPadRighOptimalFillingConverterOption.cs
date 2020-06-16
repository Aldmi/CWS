using System.Collections.Generic;

namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.StringConvertersOption
{
    public class EfPadRighOptimalFillingConverterOption
    {
        public int Lenght { get; set; }
        public Dictionary<int, string> DictWeight { get; set; }
    }
}