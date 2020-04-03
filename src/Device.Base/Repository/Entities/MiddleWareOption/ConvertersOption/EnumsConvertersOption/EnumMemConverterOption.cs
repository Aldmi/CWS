using System.Collections.Generic;

namespace Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.EnumsConvertersOption
{
    public class EnumMemConverterOption : EnumConverterOption
    {
        /// <summary>
        /// Цепочка значений для enum.
        /// Key= значение enum в string
        /// Value= кол-во вызовов, до смены этого варианта.
        /// </summary>
        public Dictionary<string, int> DictChain { get; set; }
    }
}