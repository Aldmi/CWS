using System.Collections.Generic;

namespace Shared.MiddleWares.ConvertersOption.StringConvertersOption
{
    /// <summary>
    /// Замена строкового значения, по словарю Mapping.
    /// ключ значения по умолчанию "_";
    /// </summary>
    public class ReplaseStringConverterOption
    {
        public Dictionary<string, string> Mapping { get; set; }
        public bool ToLowerInvariant { get; set; }
    }
}