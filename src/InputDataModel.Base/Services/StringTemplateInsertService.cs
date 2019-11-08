using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NCalc;


namespace Domain.InputDataModel.Base.Services
{
    public static class StringTemplateInsertService
    {
        #region fields
        private static readonly object LockerNCalc = new object();
        #endregion



        /// <summary>
        /// Вставка переменных (по формату) в строку по шаблону. 
        /// </summary>
        /// <param name="template">базовая строка с местозаполнителем</param>
        /// <param name="inserts">словарь переменных key= название переменной val= значение</param>
        /// <param name="pattern">Как выдедить переменную и ее формат, по умолчанию {val:format}</param>
        /// <returns>возвращает  результирующую строку со всеми подстановками и словарь РЕАЛЬНО вставленных переменных</returns>
        public static (string resultStr, Dictionary<string, object> resultDict) InsertByTemplate(string template, IndependentInserts inserts, string pattern = @"\{(.*?)(:.+?)?\}")
        {
            var resultDict = new Dictionary<string, object>(); // найденные в template переменные из inserts и преобразованные по формату pattern
            string Evaluator(Match match)
            {
                string res = null;
                var key = match.Groups[1].Value;

                if (inserts.TryGetValue(key, out string strVal))                 //обработка string значений
                {
                    var formatValue = match.Groups[2].Value;
                    resultDict[key] = strVal;
                    res = StringByFormatHandler(strVal, formatValue);
                }
                else
                if (inserts.TryGetValue(key, out DateTime dateTimeVal))          //обработка DateTime значений
                {
                    var formatValue = match.Groups[2].Value;
                    resultDict[key] = dateTimeVal;
                    res = DateTimeStrHandler(dateTimeVal, formatValue);
                }
                else    
                if (inserts.TryGetValue(key, out int intVal))                    //обработка int значений
                {
                    var formatValue = match.Groups[2].Value;
                    resultDict[key] = intVal;
                    var format = "{0" + formatValue + "}";
                    res = string.Format(format, intVal);
                }
                else
                if (key.Contains("rowNumber"))                                  //обработка специфических значений
                {
                    if (inserts.TryGetValue("rowNumber", out int replacement))  //например rowNumber заданна как формула {(rowNumber+64):X1}
                    {
                        var calcVal = CalculateMathematicFormat(key, (int) replacement);
                        var formatValue = match.Groups[2].Value;
                        var format = "{0" + formatValue + "}";
                        res = string.Format(format, calcVal);
                    }
                }
                else
                {
                    res = match.Value;
                }
                return res;
            }

            var result = Regex.Replace(template, pattern, Evaluator);
            return (result, resultDict);
        }


        /// <summary>
        /// Вставка подстроки в строку по формату.
        /// Если формат задан не верно - возвращается пустая строка.
        /// Если формат не задан- возвращается строка.
        /// Поддерживаемые форматы:
        /// ArrayCharInseart - Несколько вставок по индексу.
        /// EndLineCharInseart - Ограничение длинны строки с разбиением по словам, вставкой символа конца строки.
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="inseartFormat">Формат для вставки (для разных обработчиков может быть разный)</param>
        /// <returns></returns>
        private static string StringByFormatHandler(string str, string inseartFormat)
        {
            string resStr;
            switch (inseartFormat)
            {
                case var format when Regex.Match(format, "ArrayCharInseart").Success:
                    var pattern = "\\[(.*?)\\]";//выделяет данные в [...]
                    var matches = Regex.Matches(inseartFormat, pattern);
                    var options = matches.Cast<Match>().Select(m => m.Groups[1].Value).Distinct().ToList();
                    resStr = ArrayCharInseart(str, options);
                    break;

                case var format when Regex.Match(format, "EndLineCharInseart").Success:
                    pattern = "\\[(.*?)\\]";//выделяет данные в [...]
                    matches = Regex.Matches(inseartFormat, pattern);
                    var option = matches.Cast<Match>().Select(m => m.Groups[1].Value).Distinct().FirstOrDefault();
                    resStr = EndLineCharInseart(str, option);
                    break;

                default:
                    resStr = str;
                    break;
            }
            return resStr;
        }


        private static string ArrayCharInseart(string str, IEnumerable<string> options)
        {
            try
            {
                foreach (var option in options)
                {
                    var val = option.Split('^');
                    if (val.Length != 2)
                        throw new FormatException($"Неверный формат Опций {val}");

                    if (!int.TryParse(val[0], out var tempResult))
                        throw new FormatException($"Неверный формат Опций {val}");

                    var indexRepalcement = tempResult;
                    var string2Insert = val[1];

                    str = str.Insert(indexRepalcement, string2Insert);
                }
            }
            catch (Exception)
            {
                str = String.Empty;
            }

            return str;
        }


        private static string EndLineCharInseart(string str, string option)
        {
            try
            {
                var val = option.Split('^');
                if (val.Length != 2)
                    throw new FormatException($"Неверный формат Опций {val}");

                if (!int.TryParse(val[0], out var tempResult))
                    throw new FormatException($"Неверный формат Опций {val}");

                var lenghtLine = tempResult;
                var endLineChar = val[1];
                str = EndLineCharInseartCalc(str, lenghtLine, endLineChar);
            }
            catch (Exception)
            {
                str = String.Empty;
            }
            return str;
        }


        private static string EndLineCharInseartCalc(string str, int lenghtLine, string endLineChar)
        {
            List<string> resultList = new List<string>();
            var wordChanks = str.Split(' ');
            var sumWord = new StringBuilder();
            for (var i = 0; i < wordChanks.Length; i++)
            {
                var word = wordChanks[i];
                var checkStr = sumWord + word;
                if (checkStr.Length >= lenghtLine)                         //Вставить симол конца строки (endLineChar)
                {
                    string endedLine;
                    if (sumWord.Length == 0)                               //Единичное слово слишком длинное, endLineChar вставляется в него
                    {
                        endedLine = word.Insert(lenghtLine, endLineChar);
                    }
                    else
                    {                                                       //Накопивашаяся строка завершается символом endLineChar
                        endedLine = sumWord.ToString().TrimEnd(' ') + endLineChar;
                        sumWord.Clear();
                        i--;                                                // Вернуться к строке которая не влезла
                    }
                    resultList.Add(endedLine);
                }
                else
                {
                    sumWord.Append(word).Append(" ");                      //Сумировать строку                 
                }
            }

            if (sumWord.Length != 0)                                  //Последняя накопленная строка добавляется как есть (без endLineChar в конце)
            {
                resultList.Add(sumWord.ToString());
            }

            var res = resultList.Aggregate((a, b) => a + b);
            return res;
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
        /// Возможно CuncurrencyException при многопоточной работе с Expression.
        /// </summary>
        private static int CalculateMathematicFormat(string str, int row)
        {
            lock (LockerNCalc)
            {
                var expr = new Expression(str)
                {
                    Parameters = { ["rowNumber"] = row }
                };
                var func = expr.ToLambda<int>();
                var arithmeticResult = func();
                return arithmeticResult;
            }
        }
    }
}