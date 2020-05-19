using System.Collections.Generic;

namespace Shared.MiddleWares.ConvertersOption.StringConvertersOption
{
    /// <summary>
    /// Словарь соответсвия симола строке.
    /// </summary>
    public class ReplaseCharStringConverterOption
    {
        public Dictionary<char, string> Mapping { get; set; }
        public bool ToLowerInvariant { get; set; }
    }
}