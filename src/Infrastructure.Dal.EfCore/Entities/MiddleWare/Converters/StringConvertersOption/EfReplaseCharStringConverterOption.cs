using System.Collections.Generic;

namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.StringConvertersOption
{
    public class EfReplaseCharStringConverterOption
    {
        public Dictionary<char, string> Mapping { get; set; }
        public bool ToLowerInvariant { get; set; }
    }
}