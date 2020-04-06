﻿using System.Collections.Generic;

namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.EnumsConvertersOption
{
    public class EfEnumMemConverterOption : EfBaseConverterOption
    {
        public Dictionary<string, int> DictChain { get; set; }
        public string Path2Type { get; set; }
    }
}