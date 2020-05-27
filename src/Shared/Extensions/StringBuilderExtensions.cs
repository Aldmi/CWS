using System;
using System.Text;
using CSharpFunctionalExtensions;

namespace Shared.Extensions
{
    public static class StringBuilderExtensions
    {

        /// <summary>
        /// Замена первого вхождения подстроки в строке
        /// </summary>
        /// <param name="source"></param>
        /// <param name="find"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static StringBuilder ReplaceFirstOccurrence (this StringBuilder source, string find, string replace)
        {
            if (source == null)
                return null;

            int place = source.IndexOf(find, 0, false);
            if (place == -1)
            {
                return source;
            }
            var result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

        
        /// <summary>
        /// Проверяет входит ли подстрока в строку
        /// </summary>
        /// <param name="source"></param>
        /// <param name="find"></param>
        /// <returns></returns>
        public static bool Contains(this StringBuilder source, string find)
        {
            return source.IndexOf(find, 0, false) >= 0;
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="ignoreCase">игнгоорировать регистр для поиска</param>
        /// <returns></returns>
        public static int IndexOf(this StringBuilder sb, string value, int startIndex, bool ignoreCase)
        {
            int num3;
            int length = value.Length;
            int num2 = (sb.Length - length) + 1;

            if (ignoreCase == false)
            {
                for (int i = startIndex; i < num2; i++)
                {
                    if (sb[i] == value[0])
                    {
                        num3 = 1;

                        while ((num3 < length) && (sb[i + num3] == value[num3]))
                        {
                            num3++;
                        }

                        if (num3 == length)
                        {
                            return i;
                        }
                    }
                }
            }
            else
            {
                for (int j = startIndex; j < num2; j++)
                {
                    if (char.ToLower(sb[j]) == char.ToLower(value[0]))
                    {
                        num3 = 1;

                        while ((num3 < length) && (char.ToLower(sb[j + num3]) == char.ToLower(value[num3])))
                        {
                            num3++;
                        }

                        if (num3 == length)
                        {
                            return j;
                        }
                    }
                }
            }

            return -1;
        }


        /// <summary>
        /// Заменить строки Bracket на newValue.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="startBracket"></param>
        /// <param name="endBracket"></param>
        /// <param name="newValue"></param>
        /// <returns>кортеж: измененная строка, и кол-во символов между startBracket и endBracket</returns>
        public static (StringBuilder res, int numberOfCharactersBetweenBrackets) ReplaceBrackets(this StringBuilder sb, string startBracket, string endBracket, string newValue)
        {
            if (sb == null 
                || string.IsNullOrEmpty(startBracket) 
                || string.IsNullOrEmpty(endBracket))
                return (sb, 0);
            
            if(newValue == null)
                throw new ArgumentNullException("newValue НЕ может быть null");
            
            var startIndex = sb.IndexOf(startBracket, 0, false);
            var endIndex = sb.IndexOf(endBracket, startIndex + startBracket.Length, false);
            if (startIndex == -1 || endIndex == -1)
                return (sb, 0);
            
            sb.Replace(startBracket, newValue, startIndex, startBracket.Length)
                .Replace(endBracket, newValue, endIndex-startBracket.Length+newValue.Length, endBracket.Length);

            var delta = endIndex - startIndex - startBracket.Length;
            return (sb, delta);
        }
        
        
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

            if(endIndex == -1)
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
    }
}