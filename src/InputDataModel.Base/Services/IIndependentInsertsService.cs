using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoreLinq;

namespace Domain.InputDataModel.Base.Services
{
    /// <summary>
    /// Интерфейс задает правила создания словаря ВСТАВОК независимых данных (данных самого типа). 
    /// </summary>
    public interface IIndependentInsertsService
    {
        /// <summary>
        /// Создать словарь вставок
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        IndependentInserts CreateIndependentInserts(object inData);
    }


    /// <summary>
    /// Класс хранит в 3-ех типизированных словарях, значения для вставки.
    /// чтобы не использовать  Dictionary<string, object> для избежания BOXING valueType
    /// </summary>
    public class IndependentInserts
    {
        #region field
        private readonly Dictionary<string, string> _strDict = new Dictionary<string, string>();
        private readonly Dictionary<string, int> _intDict= new Dictionary<string, int>();
        private readonly Dictionary<string, DateTime> _dateTimeDict  = new Dictionary<string, DateTime>();
        #endregion


        #region Methode
        public bool TryAddValue(string key, int value)
        {
            return _intDict.TryAdd(key, value);
        }
        public bool TryAddValue(string key, string value)
        {
            return _strDict.TryAdd(key, value);
        }
        public bool TryAddValue(string key, DateTime value)
        {
            return _dateTimeDict.TryAdd(key, value);
        }

        public bool TryGetValue(string key, out int value)
        {
            return _intDict.TryGetValue(key, out value);
        }
        public bool TryGetValue(string key, out string value)
        {
            return _strDict.TryGetValue(key, out value);
        }
        public bool TryGetValue(string key, out DateTime value)
        {
            return _dateTimeDict.TryGetValue(key, out value);
        }


        public Dictionary<string, object> Map2BoxeDictionary()
        {
            var boxeDictionary = new Dictionary<string, object>();
            foreach (var (key, value) in _strDict)
            {
                boxeDictionary[key] = value;
            }
            foreach (var (key, value) in _intDict)
            {
                boxeDictionary[key] = value;
            }
            foreach (var (key, value) in _dateTimeDict)
            {
                boxeDictionary[key] = value;
            }
            return boxeDictionary;
        }
        #endregion
    }
}