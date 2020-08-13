using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;

namespace Domain.InputDataModel.Shared.StringInseartService
{
    /// <summary>
    /// Опредедляет методы расширения вычичления подстроки из строки.
    /// /// </summary>
    public static class CalcSubStringExtensions
    {
        /// <summary>
        /// Выделить подстроку обознгаченную значенимем Replacement из двух моделей всатвки.
        /// </summary>
        public static Result<string> CalcSubStringBeetween2Models(this StringInsertModel startModel, StringInsertModel endModel, string str)
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
        /// Выделить подстроку от startModel.Replacement до конца строки.
        /// </summary>
        public static Result<string> CalcSubStringBeetweenModelAndEndString(this StringInsertModel startModel, string str)
        {
            var pattern = $"{startModel.Replacement}(.*)";
            var match = Regex.Match(str, pattern);
            if (match.Success)
            {
                var res = match.Groups[1].Value;
                return Result.Ok(res);
            }
            return Result.Failure<string>($"Невозможно выделить подстроку из строки {str} используя паттерн {pattern}");
        }


        /// <summary>
        /// Выделить подстроку от начала до startModel.Replacement
        /// </summary>
        public static Result<string> CalcSubStringBeetweenStartStringAndModel(this StringInsertModel startModel, string str)
        {
            var pattern = $"(.*){startModel.Replacement}";
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