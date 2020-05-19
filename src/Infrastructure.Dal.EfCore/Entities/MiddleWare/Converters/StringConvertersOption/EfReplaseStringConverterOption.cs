using System.Collections.Generic;

namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.StringConvertersOption
{
    public class EfReplaseStringConverterOption
    {
        public Dictionary<string, string> Mapping { get; set; }
        public bool ToLowerInvariant { get; set; }
    }
}