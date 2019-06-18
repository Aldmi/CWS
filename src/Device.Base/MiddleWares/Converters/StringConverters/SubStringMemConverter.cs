using DAL.Abstract.Entities.Options.MiddleWare.Converters.StringConvertersOption;
using Shared.Helpers;

namespace DeviceForExchange.MiddleWares.Converters.StringConverters
{

    /// <summary>
    /// 
    /// </summary>
    public class SubStringMemConverter : BaseStringConverter
    {
        private readonly SubStringMemConverterOption _option; //Хранит длинну подстроки
        private int _startIndex; //Индекс начального симовола в строке 
        private string _str;     //строка



        public SubStringMemConverter(SubStringMemConverterOption option)
        {
            _option = option;
        }



        /// <summary>
        /// Перезаписывает строку и сбрасывает _startIndex когда приходит новая строка 
        /// </summary>
        /// <param name="inProp"></param>
        /// <returns></returns>
        protected override string ConvertChild(string inProp)
        {
            if (!_str.Equals(inProp))
            {
                _str = inProp;
                _startIndex = 0;
            }

            if (_startIndex >= _str.Length)
            {
                _startIndex = 0;
            }

            var subStr = HelperString.SubstringWithWholeWords(_str, _startIndex, _option.Lenght);
            _startIndex+= subStr.Length;
            return subStr;
        }
    }
}