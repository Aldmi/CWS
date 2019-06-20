using System.Collections.Generic;
using System.Linq;
using DAL.Abstract.Entities.Options.MiddleWare.Converters.StringConvertersOption;
using Shared.Helpers;

namespace DeviceForExchange.MiddleWares.Converters.StringConverters
{

    /// <summary>
    /// 
    /// </summary>
    public class SubStringMemConverter : BaseStringConverter
    {
        private readonly SubStringMemConverterOption _option;     //Хранит длину подстроки
        private int _index;                                       //Индекс подстроки для вывода
        private string _str;                                      //Строка
        private IList<string> _subStrings;                        //Список подстрок индексируемых _index



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
            //СБРОС
            if (_str == null || !_str.Equals(inProp))
            {
                _str = inProp;
                _index = -1;
                _subStrings = _str.SubstringWithWholeWords(_index, _option.Lenght).ToList();
            }

            if (++_index >= _subStrings.Count)
            {
                _index = 0;
            }

            var subStr = _subStrings[_index];
            return subStr;
        }
    }
}