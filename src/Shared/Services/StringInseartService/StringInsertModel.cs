using System;
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

        public StringInsertModel(string replacement, string varName, string format)
        {
            Replacement = replacement;
            VarName = varName;
            Format = format;
            Options = ParseOptionOfVarName(varName);
            if (!string.IsNullOrEmpty(Options))
            {
                //Удалить из varName опции
                VarName = VarName.Replace(Options, String.Empty);
            }
        }


        /// <summary>
        /// Опции после имени пекременной указанные в [] или () скобках
        /// </summary>
        private static string ParseOptionOfVarName(string varName)
        {
            var match = Regex.Match(varName, "\\[(.*)\\]");
            if (match.Success)
            {
                return match.Groups[0].Value;
            }

            match = Regex.Match(varName, "\\((.*)\\)");
            return match.Success ? match.Groups[0].Value : null;
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
            return Result.Failure<string>($"Невозможно выделить подстроку использую паттерн {pattern}");
        }
    }
}