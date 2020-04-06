using System.Collections.Generic;

namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.StringConvertersOption
{
    public class EfPadRighCharWeightStringConverterOption
    {
        public int Lenght { get; set; }
        public Dictionary<string, int> DictWeight { get; set; }
        public char Pixel { get; set; }
    }
}