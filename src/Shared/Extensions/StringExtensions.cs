using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace Shared.Extensions
{
    public static class StringExtensions
    {

        /// <summary>
        /// Вернуть подстроку между символами.
        /// </summary>
        /// <param name="str">строка</param>
        /// <param name="startCh">стартовый символ</param>
        /// <param name="endCh">конечный символ</param>
        /// <param name="startInclude">включать ли стартовый и конечный символ в подстроку</param>
        /// <returns></returns>
        public static Result<string> SubstringBetweenCharacters(this string str, string startCh, string endCh, bool startInclude = false, bool endInclude = false)
        {
            var startIndex = str.IndexOf(startCh, StringComparison.Ordinal);
            var endIndex = str.IndexOf(endCh, StringComparison.Ordinal);

            if (startIndex == -1)
                return Result.Failure<string>($"Not Found startCh= {startCh}");

            if (endIndex == -1)
                return Result.Failure<string>($"Not Found endCh= {endCh}");

            if (!startInclude)
            {
                startIndex += startCh.Length;
            }

            if (endInclude)
            {
                endIndex += endCh.Length - 1;
            }
            else
            {
                endIndex -= 1;
            }

            var subStr = str.Substring(startIndex, endIndex - startIndex + 1);
            return Result.Ok(subStr);
        }


        /// <summary>
        /// Вернуть подстроку между символами.
        /// с явным указанием startIndex.
        /// </summary>
        /// <param name="str">строка</param>
        /// <param name="startIndex">начальный индекс</param>
        /// <param name="endCh">конечный символ</param>
        /// <param name="endInclude"></param>
        /// <returns></returns>
        public static Result<string> SubstringBetweenCharacters(this string str, int startIndex, string endCh, bool endInclude = false)
        {
            var endIndex = str.IndexOf(endCh, StringComparison.Ordinal);
            if (endIndex == -1)
                return Result.Failure<string>($"Not Found endCh= {endCh}");

            if (endInclude)
            {
                endIndex += endCh.Length - 1;
            }
            else
            {
                endIndex -= 1;
            }

            var subStr = str.Substring(startIndex, endIndex - startIndex + 1);
            return Result.Ok(subStr);
        }


        /// <summary>
        /// Вернуть подстроку между символами.
        /// с явным указанием endIndex.
        /// </summary>
        /// <param name="str">строка</param>
        /// <param name="startCh">стартовый символ</param>
        /// <param name="endIndex">конечный индекс</param>
        /// <param name="startInclude">включать ли стартовый и конечный символ в подстроку</param>
        /// <returns></returns>
        public static Result<string> SubstringBetweenCharacters(this string str, string startCh, int endIndex, bool startInclude = false)
        {
            var startIndex = str.IndexOf(startCh, StringComparison.Ordinal);
            if (startIndex == -1)
                return Result.Failure<string>($"Not Found startCh= {startCh}");

            if (!startInclude)
            {
                startIndex += startCh.Length;
            }

            var subStr = str.Substring(startIndex, endIndex - startIndex + 1);
            return Result.Ok(subStr);
        }


        /// <summary>
        /// Заменить подстроки в строке.
        /// </summary>
        /// <param name="str">строка для выполнения цепочки замен</param>
        /// <param name="replacePattern">Паттерн для поиска подстроки для замены</param>
        /// <param name="getValueByKey">делегат возвращает значение для замены</param>
        /// <returns></returns>
        public static Result<string> ExecInlineInseart(this string str,
            string replacePattern,
            Func<string, string> getValueByKey)
        {
            try
            {
                var replacedStr = Regex.Replace(str, replacePattern, match =>
                {
                    var key = match.Value;
                    var repalcement = getValueByKey(key);
                    return repalcement;
                });
                return Result.Ok(replacedStr);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Failure<string>($" ОШИБКА в функции ExecInlineInseart:Ключ не найденн в словаре. '{e.Message}'");
            }
            catch (Exception e)
            {
                return Result.Failure<string>($"НЕИЗВЕСТНАЯ ОШИБКА в функции ExecInlineInseart: '{e.Message}'");
            }
        }
    }
}