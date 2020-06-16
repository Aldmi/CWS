using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shared.MiddleWares.Converters.Exceptions;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;

namespace Shared.MiddleWares.Converters.StringConverters
{
    public class PadRighOptimalFillingConverter : BaseStringConverter
    {
        private readonly PadRighOptimalFillingConverterOption _option;
        public PadRighOptimalFillingConverter(PadRighOptimalFillingConverterOption option)
        {
            _option = option;
        }


        protected override string ConvertChild(string inProp, int dataId)
        {
            if (inProp.Length >= _option.Lenght)
                return inProp;

            var sbRes= new StringBuilder(inProp);
            var currentLenght = inProp.Length;
            while (currentLenght < _option.Lenght)
            {
                var delta = _option.Lenght - currentLenght;
                var (weight, inseart) = FindOptimalInseart(delta);
                currentLenght += weight;
                sbRes.Append(inseart);
            }
            return sbRes.ToString();
        }


        private (int weightstring, string inseart) FindOptimalInseart(int delta)
        {
            var dictWeight= _option.LazyDictWeightOrderedByDescending.Value;

            //В СЛОВАРЕ НАЙДЕННО ЗНАЧЕНИЕ РАВНОЕ delta.
            if (dictWeight.TryGetValue(delta, out var value))
                return (delta, value);

            //НАЙТИ БЛИЖАЙШИЙ (МАКСИМАЛЬНЫЙ) КОЭФФИЦИЕНТ К delta.
            var firstOptimalPair = dictWeight.FirstOrDefault(pair => pair.Key < delta);
            var def = default(KeyValuePair<int, string>);//DEBUG
            if (firstOptimalPair.Equals(def))
                throw new StringConverterException($"PadRighOptimalFillingConverter ОПЦИИ заданы не верно. DictWeight не содержит элементов с коэфицентами меньше {delta}.");

            return (firstOptimalPair.Key, firstOptimalPair.Value);
        }
    }
}