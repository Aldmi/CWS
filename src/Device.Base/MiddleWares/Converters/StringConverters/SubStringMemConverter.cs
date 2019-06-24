using System;
using System.Collections.Concurrent;
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
        private readonly ConcurrentDictionary<int, SubStringState> _subStringDict= new ConcurrentDictionary<int, SubStringState>();



        public SubStringMemConverter(SubStringMemConverterOption option)
            : base(option)
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
                return new SubStringState(inProp, _option.Lenght);
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

            throw new Exception($"SubStringMemConverter НЕ СМОГ ИЗВЛЕЧЬ РЕЗУЛЬТАТ ОБРАБОТКИ ИЗ СЛОВАРЯ {inProp}");
        }
    }


    /// <summary>
    /// Разбивает строку на подстроки при создании объекта.
    /// Каждый вызов метода GetNextSubString - циклически перебирает подстроки
    /// </summary>
    public class SubStringState
    {
        public int Ingex { get; private set; }                           //Индекс подстроки для вывода
        public readonly string BaseStr;                                  //Строка
        public readonly IList<string> SubStrings;                        //Список подстрок индексируемых _index



        #region ctor

        public SubStringState(string baseStr, int subStrLenght)
        {
            Ingex = -1;
            BaseStr = baseStr;
            SubStrings = BaseStr.SubstringWithWholeWords(Ingex, subStrLenght).ToList();
        }

        #endregion



        #region Methode

        public bool EqualStr(string str)
        {
            return BaseStr.Equals(str);
        }


        private void IncrementIndex()
        {
            if (++Ingex >= SubStrings.Count)
            {
                Ingex = 0;
            }
        }


        public string GetNextSubString()
        {
            IncrementIndex();
            return SubStrings[Ingex];
        }

        #endregion
    }


}