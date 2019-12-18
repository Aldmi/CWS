using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq.Extensions;

namespace Shared.Helpers
{
    public static class HelperStringFormatInseart
    {
        /// <summary>
        /// Вернуть словарь вставок.
        /// </summary>
        /// <param name="str">строка</param>
        /// <param name="pattern">должно быть 2 местазаменителя * </param>
        /// <returns></returns>
        public static Dictionary<string, StringInsertModel> CreateInseartDictDistinctByReplacement(string str, string pattern)
        {
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
        public static Dictionary<string, StringInsertModel> CreateInseartDict(string str, string pattern)
        {
            return ConvertString2StringInsertModels(str, pattern)
                .ToDictionary(insert => insert.VarName, insert => insert);
        }



        private static IEnumerable<StringInsertModel> ConvertString2StringInsertModels(string str, string pattern)
        {
            var matches = Regex.Matches(str, pattern)
                .Select(match => new StringInsertModel(match.Groups[0].Value, match.Groups[1].Value, match.Groups[2].Value));

            return matches;
        }
    }


    public class StringInsertModel
    {
        public string Replacement { get;  }
        public string VarName { get; }
        public string Format { get; }

        public StringInsertModel(string replacement, string varName, string format)
        {
            Replacement = replacement;
            VarName = varName;
            Format = format;
        }
    }
}