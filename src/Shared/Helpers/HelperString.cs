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
        public static IEnumerable<string> SubstringWithWholeWords(this string str, int startIndex, int lenght)
        {
            if(string.IsNullOrEmpty(str))
                return new List<string> {string.Empty};

            List<string> resultList = new List<string>();
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
                resultList.Add(sumWord.ToString());
            }

            return resultList;
        }



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="str"></param>
        ///// <param name="lenghtLine"></param>
        ///// <param name="endLineChar"></param>
        ///// <returns></returns>
        //public static string InseartEndLineMarker(this string str, int lenghtLine, string endLineChar)
        //{
        //    List<string> resultList = new List<string>();
        //    var wordChanks = str.Split(' ');
        //    var sumWord = new StringBuilder();
        //    for (var i = 0; i < wordChanks.Length; i++)
        //    {
        //        var word = wordChanks[i];
        //        var checkStr = sumWord + word;
        //        if (checkStr.Length >= lenghtLine)                         //Вставить симол конца строки (endLineChar)
        //        {
        //            string endedLine;
        //            if (sumWord.Length == 0)                               //Единичное слово слишком длинное, endLineChar вставляется в него
        //            {
        //                endedLine = word.Insert(lenghtLine, endLineChar);
        //            }
        //            else
        //            {                                                       //Накопивашаяся строка завершается символом endLineChar
        //                endedLine = sumWord.ToString().TrimEnd(' ') + endLineChar;
        //                sumWord.Clear();
        //                i--;                                                // Вернуться к строке которая не влезла
        //            }
        //            resultList.Add(endedLine);
        //        }
        //        else
        //        {
        //            sumWord.Append(word).Append(" ");                      //Сумировать строку                 
        //        }
        //    }

        //    if (sumWord.Length != 0)                                  //Последняя накопленная строка добавляется как есть (без endLineChar в конце)
        //    {
        //        resultList.Add(sumWord.ToString());
        //    }

        //    var res = resultList.Aggregate((a, b) => a + b);
        //    return res;
        //}





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