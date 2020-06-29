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
        /// Вернуть подстроку обозначенную границами BorderSubString.
        /// Если BorderSubString == null. то подстрока выделяется от начала до строки nativeBorder.
        /// </summary>
        public static Result<string> CalcSubStringUsingBorder(this StringInsertModelExt ext, string str, string nativeBorder)
        {
            if (ext.BorderSubString == null)
            {
                var matchString = Regex.Match(str, $"(.*){nativeBorder}").Groups[1].Value;
                return Result.Ok(matchString);
            }
            var (_, isFailure, value, error) = ext.BorderSubString.Calc(str);
            return isFailure ? Result.Failure<string>(error) : Result.Ok(value);
        }


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
        /// Выделить подстроку обознгаченную значенимем Replacement  модели всатвки и до конца строки.
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
    }
}