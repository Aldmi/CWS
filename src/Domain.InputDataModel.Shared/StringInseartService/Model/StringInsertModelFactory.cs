using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq.Extensions;

namespace Domain.InputDataModel.Shared.StringInseartService.Model
{
    public static class StringInsertModelFactory
    {
        //@"\{(\w+)(\([\w\{\}\:\\\""\""\[\]\s\-\|\<\>\u0002\u0003]+\))?(:[^{}]+)?\}" //рабочий вариант
        private const string Pattern = @"\{(\w+)(\([^()]+\))?(:[^{}]+)?\}"; // в блоке опций

        /// <summary>
        /// Вернуть словарь вставок.
        /// </summary>
        public static List<StringInsertModel> CreateList(string str) //TODO: ??? Убрать и оставить только ConvertString2StringInsertModels
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str), "Невозможно создать словарь вставок из NULL строки");

            return ConvertString2StringInsertModels(str).ToList();
        }


        /// <summary>
        /// Вернуть словарь вставок.
        /// </summary>
        public static List<StringInsertModel> CreateListDistinctByReplacement(string str) //TODO:Добавить агрумент ReadOnlyDictionary dictInseartModelExt (чтоюы не передавать напрямую Storage)
        {
            return CreateList(str).DistinctBy(insert => insert.Replacement).ToList();
        }




        private static IEnumerable<StringInsertModel> ConvertString2StringInsertModels(string str) //TODO:Добавить агрумент ReadOnlyDictionary dictInseartModelExt
        {
            //TODO: вызывать FindExtenstion и передавать найденное значение
            var matches = Regex.Matches(str, Pattern)
                .Select(match => new StringInsertModel(match.Groups[0].Value, match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value));

            return matches;
        }


        private static StringInsertModelExt FindExtenstion (IReadOnlyDictionary<string, StringInsertModelExt> dictExt, string keyExt)
        {
            return dictExt.TryGetValue(keyExt, out var ext) ? ext : null;
        }
    }
}