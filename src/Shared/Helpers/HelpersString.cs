using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NCalc;

namespace Shared.Helpers
{
    public static class HelpersString
    {
        /// <summary>
        /// Вставка переменных (по формату) в строку по шаблону. 
        /// </summary>
        /// <param name="template">базовая строка с местозаполнителем</param>
        /// <param name="dict">словарь переменных key= название переменной val= значение</param>
        /// <param name="pattern">Как выдедить переменную и ее формат, по умолчанию {val:format}</param>
        /// <returns></returns>
        public static string StringTemplateInsert(string template, Dictionary<string, object> dict, string pattern = @"\{(.*?)(:.+?)?\}")
        {
            string Evaluator(Match match)
            {
                string res;
                var key = match.Groups[1].Value;
                if (dict.ContainsKey(key))
                {
                    var replacement = dict[key];
                    var formatValue = match.Groups[2].Value;
                    switch (replacement)
                    {
                        case DateTime time:
                            res = DateTimeStrHandler(time, formatValue);
                            break;

                        case string str:
                            res = StringIndexInseartByFormat(str, formatValue);
                            break;

                        default:
                            var format = "{0" + formatValue + "}";
                            res = string.Format(format, replacement);
                            break;
                    }
                }
                else
                if (key.Contains("rowNumber"))
                {
                    var replacement = dict["rowNumber"];
                    var calcVal = CalculateMathematicFormat(key, (int)replacement);
                    var formatValue = match.Groups[2].Value;
                    var format = "{0" + formatValue + "}";
                    res = string.Format(format, calcVal);
                }
                else
                {
                    res = match.Value;
                }
                return res;
            }

            var result = Regex.Replace(template, pattern, Evaluator);
            return result;
        }


        /// <summary>
        /// Вставка данных в строку по индексу.
        /// Формат вставки задается строкой.
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="inseartFormat">Формат вставки [index^value][...]  index- ползиция для вставки. value- значение для вставки</param>
        /// <returns>Конечная строка со всеми вставками</returns>
        private static string StringIndexInseartByFormat(string str, string inseartFormat)
        {
            try
            {
                var pattern = "\\[(.*?)\\]";//выделяет данные в [...]
                var matches = Regex.Matches(inseartFormat, pattern);
                var results = matches.Cast<Match>().Select(m => m.Groups[1].Value).Distinct().ToList();
                foreach (var result in results)
                {
                    var val = result.Split('^');
                    if (val.Length != 2)
                        throw new FormatException($"Неверный формат Note {val}");

                    if (!int.TryParse(val[0], out var tempResult))
                        throw new FormatException($"Неверный формат Note {val}");

                    var indexRepalcement = tempResult;
                    var string2Insert = val[1];

                    str = str.Insert(indexRepalcement, string2Insert);
                }
            }
            catch (Exception ex)
            {
                //_loger.Warning(ex)
                str = String.Empty;
            }

            return str;
        }




        /// <summary>
        /// Обработчик времени по формату
        /// </summary>
        private static string DateTimeStrHandler(DateTime val, string formatValue)
        {
            const string defaultStr = " ";
            if (val == DateTime.MinValue)
                return defaultStr;

            object resVal;
            if (formatValue.Contains("Sec")) //формат задан в секундах
            {
                resVal = (val.Hour * 3600 + val.Minute * 60);
                formatValue = Regex.Match(formatValue, @"\((.*)\)").Groups[1].Value;
            }
            else
            if (formatValue.Contains("Min")) //формат задан в минутах
            {
                resVal = (val.Hour * 60 + val.Minute);
                formatValue = Regex.Match(formatValue, @"\((.*)\)").Groups[1].Value;
            }
            else
            {
                resVal = val;
            }
            var format = "{0" + formatValue + "}";
            return string.Format(format, resVal);
        }


        /// <summary>
        /// Математическое вычисление формулы с участием переменной rowNumber
        /// </summary>
        private static int CalculateMathematicFormat(string str, int row)
        {
            var expr = new Expression(str)
            {
                Parameters = { ["rowNumber"] = row }
            };
            var func = expr.ToLambda<int>();
            var arithmeticResult = func();
            return arithmeticResult;
        }


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
    }
}