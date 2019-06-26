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
        /// <param name="str">Строка для разбиения на подстроки</param>
        /// <param name="lenght">длинна подстроки</param>
        /// <param name="initPhrase">строка которая добавляется в начало каждой новой подстроки</param>
        /// <returns></returns>
        public static IEnumerable<string> SubstringWithWholeWords(this string str, int lenght, string initPhrase = null)
        {
            if (string.IsNullOrEmpty(str))
                return new List<string> { string.Empty };

            if (lenght <= 0)
                throw new Exception($"При разбиении строки на подстроки {str} lenght= {lenght} не может быть <= 0 ");

            initPhrase = initPhrase ?? string.Empty;

            var resultList = new List<string>();
            var wordChanks = str.Split(' ');
            var sumWord = new StringBuilder();
            for (var i = 0; i < wordChanks.Length; i++)
            {
                var word = wordChanks[i];
                var checkStr = sumWord + word;
                if (checkStr.Length >= lenght)                              //Сохранить накопленную в sumWord подстроку
                {
                    if (sumWord.Length == 0)                                //Единичное слово слишком длинное, разобъем его на подстроки
                    {
                        var words = BreakWordIntoLines(word, lenght);
                        resultList.AddRange(words);
                    }
                    else
                    {
                        sumWord.Insert(0, initPhrase);
                        var line = sumWord.ToString().TrimEnd(' ');
                        resultList.Add(line);
                        sumWord.Clear();
                        i--;                                                // Вернуться к строке которая не влезла
                    }
                }
                else
                {
                    //Сумировать строку   
                    if (i == wordChanks.Length - 1)                         //Последняя подстрока добавляется без пробела
                    {
                        sumWord.Append(word);
                    }
                    else
                    {
                        sumWord.Append(word).Append(" ");
                    }
                }
            }

            if (sumWord.Length != 0)                                      //Последняя накопленная строка добавляется как есть
            {
                sumWord.Insert(0, initPhrase);
                resultList.Add(sumWord.ToString());
            }

            return resultList;
        }


        public static (string pharse, string resStr) SearchPhrase(string str, IEnumerable<string> phrases)
        {
            var tuple = (pharse: string.Empty, resStr: str);
            if (phrases == null)
                return tuple;
            foreach (var phrase in phrases)
            {
                if (str.Contains(phrase))
                {
                    var resStr = str.Replace(phrase, string.Empty);
                    tuple.pharse = phrase;
                    tuple.resStr = resStr;
                }
            }
            return tuple;
        }


        private static IEnumerable<string> BreakWordIntoLines(string word, int lenght)
        {
            if (word.Length <= lenght)
                return new List<string> { word };

            var words = new List<string>();
            for (int i = 0; i < word.Length; i += lenght)
            {
                string subStr = null;
                subStr = ((i + lenght) > word.Length) ? word.Substring(i) : word.Substring(i, lenght);
                words.Add(subStr);
            }

            return words;
        }
    }
}