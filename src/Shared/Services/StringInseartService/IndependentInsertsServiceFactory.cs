using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq.Extensions;
using Serilog;


namespace Shared.Services.StringInseartService
{
    public static class IndependentInsertsServiceFactory
    {
        /// <summary>
        /// Фабрика созданет сервис независмых всавок в строку
        /// IndependentInsertsService
        /// </summary>
        /// <param name="str">строка для выполнения вставок</param>
        /// <param name="pattern">паттерн выделения переменной для замены в строке</param>
        /// <param name="handlerFactorys">список фабрик по созданию нужные обработчиков</param>
        /// <returns>сервис независимых вставок</returns>
        public static IndependentInsertsService CreateIndependentInsertsService(string str, string pattern, List<Func<StringInsertModel, IIndependentInsertsHandler>> handlerFactorys, ILogger logger)
        {
            var repDict= CreateInseartDictDistinctByReplacement(str, pattern);
            var insHandlers = CreateListIndependentInseartHandlers(repDict.Values, handlerFactorys);;
            var service = new IndependentInsertsService(str, logger, insHandlers.ToArray());
            return service;
        }



        /// <summary>
        /// Вернуть словарь вставок.
        /// </summary>
        /// <param name="str">строка</param>
        /// <param name="pattern">должно быть 2 местазаменителя * </param>
        /// <returns></returns>
        public static Dictionary<string, StringInsertModel> CreateInseartDictDistinctByReplacement(string str, string pattern)
        {
            if(str == null)
                throw new ArgumentNullException(nameof(str), "Невозможно создать словарь вставок из NULL строки");

            return ConvertString2StringInsertModels(str, pattern)
                .DistinctBy(insert => insert.Replacement)
                .ToDictionary(insert => insert.VarName, insert => insert);
        }


        /// <summary>
        /// Вернуть словарь вставок.
        /// </summary>
        /// <param name="str">строка</param>
        /// <param name="pattern">должно быть 2 местазаменителя * </param>
        /// <returns></returns>
        public static Dictionary<string, StringInsertModel> CreateInseartDict(string str, string pattern)
        {
            return ConvertString2StringInsertModels(str, pattern)
                .ToDictionary(insert => insert.VarName, insert => insert);
        }



        private static IEnumerable<StringInsertModel> ConvertString2StringInsertModels(string str, string pattern)
        {
            var matches = Regex.Matches(str, pattern)
                .Select(match => new StringInsertModel(match.Groups[0].Value, match.Groups[1].Value, match.Groups[2].Value));

            return matches;
        }



        /// <summary>
        /// Создать список обработчиков независимых вставок.
        /// handlerFactorys - источник обработчиков вставок.
        /// </summary>
        /// <param name="insertModels">модель вставки</param>
        /// <param name="handlerFactorys">фабрики обработчиков вставок.</param>
        /// <returns></returns>
        public static List<IIndependentInsertsHandler> CreateListIndependentInseartHandlers(IEnumerable<StringInsertModel> insertModels, List<Func<StringInsertModel, IIndependentInsertsHandler>> handlerFactorys)
        {
            var handlers = new List<IIndependentInsertsHandler>();
            foreach (var insertModel in insertModels)
            {
                foreach (var create in handlerFactorys)
                {
                    var handler = create(insertModel);
                    if (handler != null)
                    {
                        handlers.Add(handler);
                        break;
                    }
                }
            }
            return handlers;
        }

    }
}