using System;
using System.Collections.Generic;
using Serilog;

namespace Shared.Services.StringInseartService.IndependentInseart
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
            var repDict= InseartDictFactory.CreateDistinctByReplacement(str, pattern);
            var insHandlers = CreateListIndependentInseartHandlers(repDict.Values, handlerFactorys);;
            var service = new IndependentInsertsService(str, logger, insHandlers.ToArray());
            return service;
        }


        /// <summary>
        /// Создать список обработчиков независимых вставок.
        /// handlerFactorys - источник обработчиков вставок.
        /// </summary>
        /// <param name="insertModels">модель вставки</param>
        /// <param name="handlerFactorys">фабрики обработчиков вставок.</param>
        /// <returns></returns>
        private static List<IIndependentInsertsHandler> CreateListIndependentInseartHandlers(IEnumerable<StringInsertModel> insertModels, List<Func<StringInsertModel, IIndependentInsertsHandler>> handlerFactorys)
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