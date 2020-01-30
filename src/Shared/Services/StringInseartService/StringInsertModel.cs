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
    }
}