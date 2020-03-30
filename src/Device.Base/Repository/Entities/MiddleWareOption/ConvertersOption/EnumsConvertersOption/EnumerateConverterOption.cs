using System.Collections.Generic;

namespace Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.EnumsConvertersOption
{
    public class EnumerateConverterOption : BaseConverterOption
    {
        /// <summary>
        /// Цепочка значений для enum.
        /// Key= значение enum в string
        /// Value= кол-во вызовов, до смены этого варианта.
        /// </summary>
        public Dictionary<string, int> DictChain { get; set; }

        /// <summary>
        /// Полный путь до типа в сборки.
        /// Вида: "Domain.InputDataModel.Autodictor.Entities.Lang, Domain.InputDataModel.Autodictor"
        /// </summary>
        public string Path2Type { get; set; }
    }
}