using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NCalc;

namespace Shared.Helpers
{
    public static class HelperString
    {

        /// <summary>
        /// Разбиение строки на подстроки.
        /// </summary>
        /// <param name="text">строка</param>
        /// <param name="size">кол-во символов в подстроке</param>
        /// <returns></returns>
        public static IEnumerable<string> Split(this string text, int size) 
        {
            for (var i = 0; i < text.Length; i += size)
                yield return text.Substring(i, Math.Min(size, text.Length - i));
        }


        /// <summary>
        /// Возвращает подстроку из базовой строки. Подстрока включает только целые слова.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="lenght"></param>
        /// <returns></returns>
        public static string SubstringWithWholeWords(string str, int startIndex, int lenght)
        {
            throw new System.NotImplementedException();
        }
    }
}