using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq.Extensions;

namespace Domain.InputDataModel.Shared.StringInseartService.Model
{
    public static class StringInsertModelFactory
    {
        //TODO: pattern перенести сюда. В нес нет смысла отдельно, т.к. кол-во групп захардкоженно тут в ConvertString2StringInsertModels.

        /// <summary>
        /// Вернуть словарь вставок.
        /// </summary>
        public static List<StringInsertModel> CreateList(string str, string pattern) //TODO: ??? Убрать и оставить только ConvertString2StringInsertModels
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str), "Невозможно создать словарь вставок из NULL строки");

            return ConvertString2StringInsertModels(str, pattern).ToList();
        }


        /// <summary>
        /// Вернуть словарь вставок.
        /// </summary>
        public static List<StringInsertModel> CreateListDistinctByReplacement(string str, string pattern) //TODO:Добавить агрумент ReadOnlyDictionary dictInseartModelExt (чтоюы не передавать напрямую Storage)
        {
            return CreateList(str, pattern).DistinctBy(insert => insert.Replacement).ToList();
        }




        private static IEnumerable<StringInsertModel> ConvertString2StringInsertModels(string str, string pattern) //TODO:Добавить агрумент ReadOnlyDictionary dictInseartModelExt
        {
            //TODO: вызывать FindExtenstion и передавать найденное значение
            var matches = Regex.Matches(str, pattern)
                .Select(match => new StringInsertModel(match.Groups[0].Value, match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value));

            return matches;
        }


        private static StringInsertModelExt FindExtenstion (IReadOnlyDictionary<string, StringInsertModelExt> dictExt, string keyExt)
        {
            return dictExt.TryGetValue(keyExt, out var ext) ? ext : null;
        }
    }
}