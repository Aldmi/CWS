using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core.Exceptions;
using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.EnumsConvertersOption;
using MoreLinq;
using Shared.Extensions;


namespace Domain.Device.MiddleWares.Converters.EnumsConverters
{
    public class EnumerateConverter : BaseEnumConverter, IMemConverterMiddleWare
    {
        private readonly EnumMemConverterOption _option;
        private readonly ConcurrentDictionary<int, EnumState> _enumStateDict = new ConcurrentDictionary<int, EnumState>();

        public EnumerateConverter(EnumMemConverterOption option)
            : base(option)
        {
            _option = option;
        }

        public override Enum Convert(Enum inProp, int dataId)
        {
            var enumState = _enumStateDict.GetOrAdd(dataId, new EnumState(ObjectType, _option.DictChain));
            var nextState = enumState.GetNextState();
            return nextState;
        }

        public void SendCommand(MemConverterCommand command)
        {
            if (command == MemConverterCommand.Reset)
            {
                _enumStateDict.Values.ForEach(state => state.ResetState());
            }
        }
    }



    /// <summary>
    /// Управляет состоянием перечислителя.
    /// В заданной последовательности перечисляет состояние Enum.
    /// Вариант перечисления вычисляется вызовом метода GetNextState(), в котором счетчик определяет долю того или иного состояния.
    /// </summary>
    public class EnumState
    {
        private readonly Type _type;                                           //Тип элемента enum
        private readonly IReadOnlyDictionary<string, int> _defaultDictionary;  //Базовый Словарь Счетчиков повторов элемена Enum (для сброса счетчиков в начальное значение)
        private readonly Dictionary<string, int> _currentDictionary;           //Словарь Счетчиков повторов элемена Enum
        private string _currentKey;                                            //Ключ для словаря, определяет текущий счетчик повторов
        private readonly ReadOnlyCollection<string> _keys;                     //Коллекция ключей словаря


        #region prop
        /// <summary>
        /// Счетчик повторов
        /// </summary>
        private int RetryCounter
        {
            get => _currentDictionary[_currentKey];
            set => _currentDictionary[_currentKey] = value;
        }
        #endregion


        #region ctor
        public EnumState(Type type, IDictionary<string, int> dict)
        {
            _type = type;
            _defaultDictionary = new ReadOnlyDictionary<string, int>(dict);
            _currentDictionary = new Dictionary<string, int>(dict);
            _currentKey = _currentDictionary.First().Key;
            _keys = new ReadOnlyCollection<string>(_currentDictionary.Keys.Select(k => k.ToString()).ToArray());
        }
        #endregion


        #region Methode

        public Enum GetNextState()
        {
            Enum value;
            try
            {
                value = (Enum)Enum.Parse(_type, _currentKey);
            }
            catch (Exception)
            {
                throw new ParseException($"В типе Enum {_type} не найдено значение перечисления {_currentKey}", 0);
            }
            if (--RetryCounter == 0)
            {
                RetryCounter = _defaultDictionary[_currentKey];        // установили Дефолтное состояние счетчика повторов
                _currentKey = _keys.GetSecondItemInCycle(_currentKey); // след значение ключа
            }
            return value;
        }

        public void ResetState()
        {
            foreach (var key in _keys)
            {
                _currentDictionary[key] = _defaultDictionary[key];
                _currentKey = _currentDictionary.First().Key;
            }
        }
        #endregion
    }
}