﻿using System;
using System.Collections.Generic;
using Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Serilog;

namespace Domain.InputDataModel.Shared.StringInseartService.IndependentInseart
{
    public static class IndependentInsertsServiceFactory
    {
        /// <summary>
        /// Фабрика создает сервис независмых всавок в строку
        /// IndependentInsertsService
        /// </summary>
        /// <param name="str">строка для выполнения вставок</param>
        /// <param name="handlerFactorys">список фабрик по созданию нужные обработчиков</param>
        /// <param name="stringInsertModelExtDict">словарь расширений для stringInsertModel</param>
        /// <returns>сервис независимых вставок</returns>
        public static IndependentInsertsService CreateIndependentInsertsService(
            string str,
            List<Func<StringInsertModel, IIndependentInsertsHandler>> handlerFactorys,
            IReadOnlyDictionary<string, StringInsertModelExt> stringInsertModelExtDict,
            ILogger logger)
        {
            var list= StringInsertModelFactory.CreateListDistinctByReplacement(str, stringInsertModelExtDict);
            var insHandlers = CreateListIndependentInseartHandlers(list, handlerFactorys);
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