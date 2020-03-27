using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core.Exceptions;
using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.EnumsConvertersOption;
using Microsoft.EntityFrameworkCore.Internal;

namespace Domain.Device.MiddleWares.Converters.EnumsConverters
{
    public class EnumerateConverter : BaseEnumConverter
    {
        private readonly EnumerateConverterOption _option;
        private string _currentKey;
        private readonly Dictionary<string, int> _currentDictionary;
        private readonly IReadOnlyList<string> keys;

        public EnumerateConverter(EnumerateConverterOption option)
            : base(option)
        {
            _option = option;
            _currentKey = _option.DictChain.First().Key;
            _currentDictionary= new Dictionary<string, int>(_option.DictChain);
            keys = _currentDictionary.Keys.Select(k => k.ToString()).ToList();
        }



        public override Enum Convert(Enum inProp, int dataId)
        {
            Enum newValue;
            try
            {
                newValue = (Enum)Enum.Parse(objectType, _currentKey);
            }
            catch (Exception)
            {
                throw new ParseException($"В типе Enum {objectType} не найдено значение перечисления {_currentKey}",0);
            }
            if (--_currentDictionary[_currentKey] == 0)
            {
               _currentDictionary[_currentKey] = _option.DictChain[_currentKey]; // установили Дефолтное состояние счетчика
               _currentKey= GetSecondKeyCycled(keys, _currentKey);               // след значение ключа
            }
            return newValue;
        }



        private string GetSecondKeyCycled(IReadOnlyList<string> keys, string key)
        {
            var index = keys.IndexOf(key);
            if (++index >= keys.Count)
            {
                return keys[0];
            }
            return keys[index];
        }
    }
}