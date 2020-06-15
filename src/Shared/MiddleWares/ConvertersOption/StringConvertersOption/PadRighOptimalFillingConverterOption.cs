using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.MiddleWares.ConvertersOption.StringConvertersOption
{
    public class PadRighOptimalFillingConverterOption
    {
        public PadRighOptimalFillingConverterOption()
        {
            LazyDictWeightOrdered = new Lazy<Dictionary<int, string>>(() =>
            {
                var resDict =  DictWeight.OrderByDescending(pair => pair.Key).ToDictionary(pair=> pair.Key, pair=> pair.Value);
                return resDict;
            });
        }


        /// <summary>
        /// Длинна строки до которой нужно дополнить
        /// </summary>
        public int Lenght { get; set; }

        /// <summary>
        /// словарь весов.
        /// key- длинна отрезка строки для заполнения.
        /// value- строка дополнения. 
        /// </summary>
        public Dictionary<int, string> DictWeight { get; set; }

        /// <summary>
        /// На базе DictWeight лениво создаем словарь всех char с весами 
        /// </summary>
        public Lazy<Dictionary<int, string>> LazyDictWeightOrdered { get; }
    }
}