﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
        /// Возвращает подстроку из базовой строки. Подстрока включает только целые слова или целые фразы отделяемые wordSeparator
        /// Если указанна initPhrase, то она добавляется в начало каждой новой подстроки
        /// </summary>
        /// <param name="str">Строка для разбиения на подстроки</param>
        /// <param name="lenght">длинна подстроки</param>
        /// <param name="initPhrase">строка которая добавляется в начало каждой новой подстроки</param>
        /// <param name="wordSeparator">Разделитель на слова (по умолчанию) или на фразы (например по ',')</param>
        /// <returns></returns>
        public static IEnumerable<string> SubstringWithWholeWords(this string str, int lenght, string initPhrase = null, char wordSeparator = ' ')
        {
            if (string.IsNullOrEmpty(str))
                return new List<string> { string.Empty };

            if (lenght <= 0)
                throw new Exception($"При разбиении строки на подстроки {str} lenght= {lenght} не может быть <= 0 ");

            initPhrase = initPhrase ?? string.Empty;

            var resultList = new List<string>();
            var wordChanks = str.Split(wordSeparator);
            var sumWord = new StringBuilder();
            for (var i = 0; i < wordChanks.Length; i++)
            {
                var word = wordChanks[i];
                var checkStr = sumWord + word;
                if (checkStr.Length >= lenght)                              //Сохранить накопленную в sumWord подстроку
                {
                    if (sumWord.Length == 0)                                //Единичное слово слишком длинное, разобъем его на подстроки и возьмем первую подстроку!!!
                    {
                        var firstWord = BreakWordIntoLines(word, lenght).FirstOrDefault().RemovingExtraSpaces();
                        resultList.Add(firstWord);
                    }
                    else
                    {
                        sumWord.Insert(0, initPhrase);
                        var line = sumWord.ToString().RemovingExtraSpaces();//Удалять повторяющиеся пробелы
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
                        sumWord.Append(word).Append(wordSeparator);
                    }
                }
            }

            if (sumWord.Length != 0)                                      //Последняя накопленная строка добавляется как есть
            {
                sumWord.Insert(0, initPhrase);
                resultList.Add(sumWord.ToString().RemovingExtraSpaces());
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


        /// <summary>
        /// Удаление лишних пробелов из строки.
        /// В начале и конце строки, также повторяющииеся пробелы в самой строке
        /// </summary>
        public static string RemovingExtraSpaces(this string str)
        {
            return string.IsNullOrEmpty(str) ? str : Regex.Replace(str, @"\s+", " ").Trim();
        }


        public static string GetSpaceOrString(this string str)
        {
            return string.IsNullOrEmpty(str) ? " " : str;
        }


        /// <summary>
        /// Ограничить длинну строки
        /// </summary>
        public static (bool res, int OutOfLimit) CheckLimitLenght(this string str, int maxLenght)
        {
            var diff= maxLenght - str.Length;
            return diff >= 0 ? (res: false, 0) : (res: true, diff * -1);
        }


        /// <summary>
        /// Вернуть подстроку между символами.
        /// </summary>
        /// <param name="str">строка</param>
        /// <param name="startCh">стартовый символ</param>
        /// <param name="endCh">конечный символ</param>
        /// <param name="includeBorder">включать ли стартовый и конечный символ в подстроку</param>
        /// <returns></returns>
        public static string SubstringBetweenCharacters(this string str, string startCh, string endCh, bool includeBorder = false)
        {
            var startIndex = str.IndexOf(startCh, StringComparison.Ordinal); 
            var endIndex = str.IndexOf(endCh, StringComparison.Ordinal);

            if(startIndex == -1)
                throw new ArgumentOutOfRangeException($"Not Found startCh= {startCh}");  //TODO: zamenit na Result<T>

            if(endIndex == -1)
                throw new ArgumentOutOfRangeException($"Not Found endCh= {endCh}");      //TODO: zamenit na Result<T>

            if (includeBorder)
            {
                endIndex += endCh.Length-1;
            }
            else
            {
                startIndex += startCh.Length;
                endIndex -= 1;
            }
            var subStr = str.Substring(startIndex, endIndex - startIndex + 1);
            return subStr;
        }
    }
}