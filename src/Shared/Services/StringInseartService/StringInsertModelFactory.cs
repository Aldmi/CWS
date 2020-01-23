using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq.Extensions;

namespace Shared.Services.StringInseartService
{
    public static class StringInsertModelFactory
    {
        /// <summary>
        /// Вернуть словарь вставок.
        /// </summary>
        /// <param name="str">строка</param>
        /// <param name="pattern">должно быть 2 местазаменителя * </param>
        /// <returns></returns>
        public static Dictionary<string, StringInsertModel> CreateDistinctByReplacement(string str, string pattern)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str), "Невозможно создать словарь вставок из NULL строки");

            return ConvertString2StringInsertModels(str, pattern)
                .DistinctBy(insert => insert.Replacement)
                .ToDictionary(insert => insert.VarName, insert => insert);
        }

        /// <summary>
        /// Вернуть словарь вставок.
        /// </summary>
        /// <param name="str">строка</param>
        /// <param name="pattern">должно быть 2 местазаменителя * </param>
        /// <returns></returns>
        public static List<StringInsertModel> CreateList(string str, string pattern)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str), "Невозможно создать словарь вставок из NULL строки");

            return ConvertString2StringInsertModels(str, pattern).ToList();
        }


        private static IEnumerable<StringInsertModel> ConvertString2StringInsertModels(string str, string pattern)
        {
            var matches = Regex.Matches(str, pattern)
                .Select(match => new StringInsertModel(match.Groups[0].Value, match.Groups[1].Value, match.Groups[2].Value));

            return matches;
        }
    }
}