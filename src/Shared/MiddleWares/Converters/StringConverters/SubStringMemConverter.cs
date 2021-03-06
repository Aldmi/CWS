﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MoreLinq.Extensions;
using Shared.Extensions;
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
    public class SubStringMemConverter : BaseStringConverter, IMemConverterMiddleWare, IDisposable //TODO: переименовать на PagingMemConverter
    {
        private readonly SubStringMemConverterOption _option;     //Хранит длину подстроки
        private readonly ConcurrentDictionary<int, SubStringStateImmutable> _stateDict= new ConcurrentDictionary<int, SubStringStateImmutable>();


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
            SubStringStateImmutable SetState() => new SubStringStateImmutable(inProp, _option.Lenght, _option.InitPharases, _option.Separator, _option.BanTime);
            var state = _stateDict.GetOrAddExt(dataId, SetState);
            if (!state.EqualStr(inProp))
            {
                state.Dispose();
                state = SetState();
                _stateDict[dataId] = state;
            }
            var subStr = state.GetState();
            return subStr;
        }



        public void SendCommand(MemConverterCommand command)
        {
            //not implemented т.к. сброс осуществляется приходом новой строки inProp и перезаписыванием объекта SubStringState созданного на базе этой строки
        }



        /// <summary>
        /// Разбивает строку на подстроки при создании объекта.
        /// Каждый вызов метода GetNextSubString - циклически перебирает подстроки
        /// </summary>
        private class SubStringStateImmutable : BaseState4MemConverter
        {
            private int _ingex;                             //Индекс подстроки для вывода
            private readonly string _baseStr;               //Строка
            private readonly IList<string> _subStrings;     //Список подстрок индексируемых _index


            #region ctor
            public SubStringStateImmutable(string baseStr, int subStrLenght, IEnumerable<string> phrases, char separator, int banTime) : base(banTime)
            {
                _ingex = 0;
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


            public string GetState()
            {
                var subStr= _subStrings[_ingex];
                if (TriggerDisabledOrIsFire)
                {
                    IncrementIndex();
                    RestartTrigger();
                }
                return subStr;
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


        public void Dispose()
        {
            _stateDict.ForEach(s => s.Value.Dispose());
        }
    }
}