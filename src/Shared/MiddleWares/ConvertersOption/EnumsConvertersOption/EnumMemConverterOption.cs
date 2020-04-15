using System.Collections.Generic;

namespace Shared.MiddleWares.ConvertersOption.EnumsConvertersOption
{
    public class EnumMemConverterOption
    {
        /// <summary>
        /// Цепочка значений для enum.
        /// Key= значение enum в string
        /// Value= кол-во вызовов, до смены этого варианта.
        /// </summary>
        public Dictionary<string, int> DictChain { get; set; }
    }
}