using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace Shared.Services.StringInseartService
{
    public class StringInsertModel
    {
        public string Replacement { get; }
        public string VarName { get; }
        public string Format { get; }
        public string Options { get; }


        public StringInsertModel(string replacement, string varName, string options, string format)
        {
            Replacement = replacement;
            VarName = varName;
            Format = format;
            Options = options;
        }


        public static Result<string> CalcSubStringBeetween2Models(string str, StringInsertModel startModel, StringInsertModel endModel)
        {
            var pattern = $"{startModel.Replacement}(.*){endModel.Replacement}";
            var match = Regex.Match(str, pattern);
            if (match.Success)
            {
                var res = match.Groups[1].Value;
                return Result.Ok(res);
            }
            return Result.Failure<string>($"Невозможно выделить подстроку из строки {str} используя паттерн {pattern}");
        }


        /// <summary>
        /// Выделить из Options части.
        /// Части отделены пробелом.
        /// Если частей более одной, ТО ВСЕ ОПЦИИ УКАЗЫВАЮТСЯ В КРУГЛЫХ СКОБКАХ (...)
        /// </summary>
        public IReadOnlyList<string> FindPartsOptions()
        {   var parts= new List<string>();
            if (!string.IsNullOrEmpty(Options))
            {
                var split = Options.Split(' ');
                switch (split.Length)
                {
                    case 1:
                        parts.Add(split[0]);
                        break;

                    case 2:
                        parts.Add(split[0].Remove(0, 1));
                        parts.Add(split[1].Remove(split[1].Length - 1, 1));
                        break;
                }
            }
            return parts;
        }
    }
}