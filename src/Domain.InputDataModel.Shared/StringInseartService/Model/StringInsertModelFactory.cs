using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq.Extensions;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;
using Shared.MiddleWares.HandlersOption;

namespace Domain.InputDataModel.Shared.StringInseartService.Model
{
    public static class StringInsertModelFactory
    {
        //@"\{(\w+)(\([\w\{\}\:\\\""\""\[\]\s\-\|\<\>\u0002\u0003]+\))?(:[^{}]+)?\}" //рабочий вариант
        private const string Pattern = @"\{(\w+)(\([^{}:]+\))?(:[^{}]+)?\}"; // в блоке опций


        /// <summary>
        /// Вернуть словарь вставок убирая повторы по переменной Replacement.
        /// </summary>
        public static List<StringInsertModel> CreateListDistinctByReplacement(string str, IReadOnlyDictionary<string, StringInsertModelExt> extDict)
        {
            return CreateList(str, extDict).DistinctBy(insert => insert.Replacement).ToList();
        }


        /// <summary>
        /// Вернуть словарь вставок.
        /// </summary>
        public static List<StringInsertModel> CreateList(string str, IReadOnlyDictionary<string, StringInsertModelExt> extDict)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str), "Невозможно создать словарь вставок из NULL строки");

            return ConvertString2StringInsertModels(str, extDict).ToList();
        }

        
        private static IEnumerable<StringInsertModel> ConvertString2StringInsertModels(string str, IReadOnlyDictionary<string, StringInsertModelExt> extDict)
        {
            var matches = Regex.Matches(str, Pattern)
                .Select(match => new StringInsertModel(
                      match.Groups[0].Value,
                         match.Groups[1].Value,
                          match.Groups[2].Value,
                            new StringInsertModelExtFactory(extDict, match.Groups[3].Value.TrimStart(':'))));

            return matches;
        }

    }
}