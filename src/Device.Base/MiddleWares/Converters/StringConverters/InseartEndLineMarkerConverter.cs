using System.Linq;
using System.Text;
using DAL.Abstract.Entities.Options.MiddleWare.Converters.StringConvertersOption;
using Shared.Helpers;

namespace DeviceForExchange.MiddleWares.Converters.StringConverters
{
    /// <summary>
    /// Разбивает строку, вставкой символа переноса строки Marker.
    /// </summary>
    public class InseartEndLineMarkerConverter : BaseStringConverter
    {
        private readonly InseartEndLineMarkerConverterOption _option;


        public InseartEndLineMarkerConverter(InseartEndLineMarkerConverterOption option) 
            : base(option)
        {
            _option = option;
        }


        /// <summary>
        /// Если слово слишком длинное оно обрезается (берется начальные LenghtLine символов слова)
        /// </summary>
        protected override string ConvertChild(string inProp, int dataId)
        {
            var subStrings = inProp.SubstringWithWholeWords( _option.LenghtLine).ToList();

            var sumStr= new StringBuilder();
            for (var i = 0; i < subStrings.Count; i++)
            {
                var subStr = subStrings[i];
                if (i == subStrings.Count - 1)
                {
                    sumStr.Append(subStr);
                }
                else
                {
                    sumStr.Append(subStr).Append(_option.Marker);
                }
            }

            var res= sumStr.ToString();
            return res;
        }
    }
}