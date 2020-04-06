using System.Collections.Generic;
using System.Text;
using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.StringConvertersOption;

namespace Domain.Device.MiddleWares.Converters.StringConverters
{
    /// <summary>
    /// Дополняет пробелами строку до длинны указанной в _option.Lenght.
    /// Если длинна строки больше или равна _option.Lenght, то дополнение не происходит.
    /// </summary>
    public class PadRightCharWeightStringConverter : BaseStringConverter
    {
        private readonly PadRighCharWeightStringConverterOption _option;
        public PadRightCharWeightStringConverter(PadRighCharWeightStringConverterOption option)
        {
            _option = option;
        }


        protected override string ConvertChild(string inProp, int dataId)
        {
            var dictWeightExt= _option.LazyDictWeightExt.Value;
            var strLenghtInPixel = ClacStrLenghtInPixels(inProp, dictWeightExt);
            var padRightCount = _option.Lenght - strLenghtInPixel;

            if (padRightCount <= 0)
                return inProp;

            var res = (strLenghtInPixel >= _option.Lenght) ? inProp : PadRight(inProp, padRightCount, _option.Pixel);
            return res;
        }


        private int ClacStrLenghtInPixels(string str, IReadOnlyDictionary<char, int> dictWeightExt)
        {
            var sumPixel = 0;
            foreach (var ch in str.ToCharArray())
            {
                var weight = dictWeightExt.TryGetValue(ch, out var w) ? w : 1;   //Если симола нет в словаре, его вес принимается за 1 пикесель
                sumPixel += weight;
            }
            if (str.Length > 0)// Добавим по 1 пикселю между симолами
            {
                sumPixel += str.Length - 1; 
            }
            return sumPixel;
        }


        private string PadRight(string str, int padRightCount, char pixel)
        {
            var sb = new StringBuilder(str);
            for (var i = 0; i < padRightCount; i++)
            {
                sb.Append(pixel);
            }
            var res = sb.ToString();
            return res;
        }
    }
}