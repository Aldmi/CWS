using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Shared.Helpers;

namespace Domain.InputDataModel.Shared.StringInseartService.Model
{
    public class StringInsertModel
    {
        public string Replacement { get; }
        public string VarName { get; }
        public string Format { get; }
        public IReadOnlyList<string> Options { get; }


        public StringInsertModel(string replacement, string varName, string options, string format)
        {
            Replacement = replacement;
            VarName = varName;
            Format = format;
            Options = FindPartsOptions(options, '|');
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
        /// Части отделены partSeparator.
        /// </summary>
        private IReadOnlyList<string> FindPartsOptions(string options, char partSeparator)
        {
            if (string.IsNullOrEmpty(options))
                return null;
            
            options = options.ReplaceFirstOccurrence("(", String.Empty).ReplaceLastOccurrence(")", String.Empty);
            var split = options.Split(partSeparator);
            return split;
        }
    }
}