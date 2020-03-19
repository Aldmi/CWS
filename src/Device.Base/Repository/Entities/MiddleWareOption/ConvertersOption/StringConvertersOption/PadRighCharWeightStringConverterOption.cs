using System;
using System.Collections.Generic;

namespace Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.StringConvertersOption
{
    public class PadRighCharWeightStringConverterOption : BaseConverterOption
    {
        public PadRighCharWeightStringConverterOption()
        {
            LazyDictWeightExt = new Lazy<Dictionary<char, int>>(() =>
            {
                var resDict = new Dictionary<char, int>();
                foreach (var (key, value) in DictWeight)
                {
                    foreach (var ch in key.ToCharArray())
                    {
                        resDict.TryAdd(ch, value);
                    }
                }
                return resDict;
            });
        }

        /// <summary>
        /// Длинна в пикселях
        /// </summary>
        public int Lenght { get; set; }

        /// <summary>
        /// словарь весов
        /// </summary>
        public Dictionary<string, int> DictWeight { get; set; }

        /// <summary>
        /// Символ одного пикселя, вставляемого в строку
        /// </summary>
        public char Pixel { get; set; }

        /// <summary>
        /// На базе DictWeight лениво создаем словарь всех char с весами 
        /// </summary>
        public Lazy<Dictionary<char, int>> LazyDictWeightExt { get; }
    }
}