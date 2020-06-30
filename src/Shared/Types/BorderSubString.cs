using System;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Shared.Extensions;
using Shared.Helpers;

namespace Shared.Types
{
    /// <summary>
    /// Выделять подстроку
    /// None - не разделять строку
    /// Left - левую часть строки от указаного разделителя
    /// Right - праавую часть строки от указаного разделителя
    /// DeleteDelemiter - всю строку удалив из нее разделитель
    /// </summary>
    public enum DelimiterSign { None, Left, Right, DeleteDelemiter }

    public class BorderSubString
    {
        public string StartCh { get; set; }
        public string EndCh { get; set; }
        public bool StartInclude { get; set; }
        public bool EndInclude { get; set; }
        public DelimiterSign DelimiterSign { get; set; }

        /// <summary>
        /// Вернуть подстроку обозначенную границами BorderSubString
        /// </summary>
        public Result<string> Calc(string str)
        {
            Result<string> result;
            if (string.IsNullOrEmpty(StartCh))
            {
                result = str.SubstringBetweenCharacters(0, EndCh, EndInclude);
            }
            else
            if (string.IsNullOrEmpty(EndCh))
            {
                result = str.SubstringBetweenCharacters(StartCh, str.Length - 1, StartInclude);
            }
            else
            {
                result = str.SubstringBetweenCharacters(StartCh, EndCh, StartInclude, EndInclude);
            }
            var (_, isFailure, value, error) = result;
            return isFailure ? Result.Failure<string>(error) : Result.Ok(value);
        }


        /// <summary>
        /// Вернуть подстроку обозначенную границами BorderSubString использую разделитель строки (delemiter)
        /// </summary>
        public Result<string> Calc(string str, string delemiter)
        {
            if (string.IsNullOrEmpty(delemiter))
                return Result.Failure<string>("BorderSubString.Calc(...)  delemiter не может быть null или пуст");

            //ВЫДЕЛИМ ЗАДАНУЮ DelimiterSign ЧАСТЬ СТРОКИ.
            var resStr = str;
            switch (DelimiterSign)
            {
                case DelimiterSign.Left:
                    var pattern = $"(.*){delemiter}";
                    var match = Regex.Match(str, pattern);
                    if (match.Success)
                    {
                        resStr = match.Groups[1].Value;
                    }
                    else
                    {
                        return Result.Failure<string>($"BorderSubString.Calc(...)  DelimiterSign.Left {delemiter} не выделили ЛЕВУЮ часть строки {str}");
                    }
                    break;

                case DelimiterSign.Right:
                    pattern = $"{delemiter}(.*)";
                    match = Regex.Match(str, pattern);
                    if (match.Success)
                    {
                        resStr = match.Groups[1].Value;
                    }
                    else
                    {
                        return Result.Failure<string>($"BorderSubString.Calc(...)  DelimiterSign.Right {delemiter} не выделили ПРАВУЮ часть строки {str}");
                    }
                    break;

                case DelimiterSign.DeleteDelemiter:
                    resStr= str.ReplaceFirstOccurrence(delemiter, string.Empty);
                    break;
            }

            return Calc(resStr);
        }

    }
}