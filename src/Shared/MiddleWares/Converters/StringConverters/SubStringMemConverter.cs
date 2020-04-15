using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Shared.Helpers;
using Shared.MiddleWares.Converters.Exceptions;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;

namespace Shared.MiddleWares.Converters.StringConverters
{
    /// <summary>
    /// Выделяет подстроку из строки.
    /// StateFull - хранит начало подстроки между вызовами.
    /// Длинна подстроки вычисляется с учетом константной фразы.
    /// </summary>
    public class SubStringMemConverter : BaseStringConverter, IMemConverterMiddleWare
    {
        private readonly SubStringMemConverterOption _option;     //Хранит длину подстроки
        private readonly ConcurrentDictionary<int, SubStringState> _subStringDict= new ConcurrentDictionary<int, SubStringState>();



        public SubStringMemConverter(SubStringMemConverterOption option)
        {
            _option = option;
        }


        /// <summary>
        /// Перезаписывает строку и сбрасывает _startIndex когда приходит новая строка 
        /// </summary>
        /// <param name="inProp"></param>
        /// <param name="dataId"></param>
        /// <returns></returns>
        protected override string ConvertChild(string inProp, int dataId)
        {
            SubStringState GetResetState()
            {
                return new SubStringState(inProp, _option.Lenght, _option.InitPharases, _option.Separator);
            }
          
            //Если данных нет в словаре. Добавить в словарь новые данные.
            if (!_subStringDict.ContainsKey(dataId))
            {
                _subStringDict.TryAdd(dataId, GetResetState());
            }

            //Если Строка по заданному индексу поменялась, сбросим состояние.
            if (_subStringDict.TryGetValue(dataId, out var value))
            {
                if (!value.EqualStr(inProp))        
                {
                    _subStringDict[dataId] = GetResetState();
                }
            }

            //Вернуть следующую подстроку
            if (_subStringDict.TryGetValue(dataId, out var resultValue))
            {
                var subStr = resultValue.GetNextSubString();
                return subStr;
            }

            throw new StringConverterException($"SubStringMemConverter НЕ СМОГ ИЗВЛЕЧЬ РЕЗУЛЬТАТ ОБРАБОТКИ ИЗ СЛОВАРЯ {inProp}");
        }

        public void SendCommand(MemConverterCommand command)
        {
            //NotImplemented
        }
    }


    /// <summary>
    /// Разбивает строку на подстроки при создании объекта.
    /// Каждый вызов метода GetNextSubString - циклически перебирает подстроки
    /// </summary>
    public class SubStringState  //TODO: попробовать поменять на структуру
    {
        private int _ingex;                             //Индекс подстроки для вывода
        private readonly string _baseStr;               //Строка
        private readonly IList<string> _subStrings;     //Список подстрок индексируемых _index



        #region ctor

        public SubStringState(string baseStr, int subStrLenght, IEnumerable<string> phrases, char separator)
        {
            _ingex = -1;
            _baseStr = baseStr;

            var (initPhrase, resStr) = HelperString.SearchPhrase(baseStr, phrases);
            subStrLenght -= initPhrase.Length;
            _subStrings = resStr.SubstringWithWholeWords(subStrLenght, initPhrase, separator).ToList();
        }

        #endregion


        #region Methode

        public bool EqualStr(string str)
        {
            return _baseStr.Equals(str);
        }


        public string GetNextSubString()
        {
            IncrementIndex();
            return _subStrings[_ingex];
        }


        private void IncrementIndex()
        {
            if (++_ingex >= _subStrings.Count)
            {
                _ingex = 0;
            }
        }

        #endregion
    }
}