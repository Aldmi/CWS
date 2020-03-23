﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Domain.Device.MiddleWares.Converters.Exceptions;
using Domain.Device.MiddleWares.Handlers;
using Domain.Device.MiddleWares.Invokes;
using Domain.InputDataModel.Base.InData;
using FastDeepCloner;
using Serilog;
using Shared.ReflectionServices;
using MiddleWareInDataOption = Domain.Device.Repository.Entities.MiddleWareOption.MiddleWareInDataOption;

namespace Domain.Device.MiddleWares
{
    /// <summary>
    /// Промежуточный обработчик входных данных.
    /// Сервис является иммутабельным. Он создается на базе MiddleWareInDataOption и его State не меняется
    /// </summary>
    /// <typeparam name="TIn">Входные данные для обработки</typeparam>
    public class MiddleWareInData<TIn> : IMiddlewareInData<TIn> where TIn : InputTypeBase
    {
        #region fields

        private readonly MiddleWareInDataOption _option;
        private readonly ILogger _logger;

        private readonly PropertyMutationsServise<string> _mutationsServiseStr = new PropertyMutationsServise<string>();    //Сервис изменения совойства типа string, по имени, через рефлексию.
        private readonly PropertyMutationsServise<DateTime> _mutationsServiseDt = new PropertyMutationsServise<DateTime>(); //Сервис изменения совойства типа DateTime, по имени, через рефлексию.

        private readonly List<StringHandlerMiddleWare> _stringHandlers;
        private readonly List<DateTimeHandlerMiddleWare> _dateTimeHandlers;

        #endregion



        #region prop

        public string Description => _option.Description;

        #endregion



        #region ctor

        public MiddleWareInData(MiddleWareInDataOption option, ILogger logger)
        {
            _option = option;
            _logger = logger;

            _stringHandlers= option.StringHandlers?.Select(handlerOption => new StringHandlerMiddleWare(handlerOption)).ToList();
            _dateTimeHandlers = option.DateTimeHandlers?.Select(handlerOption => new DateTimeHandlerMiddleWare(handlerOption)).ToList();
        }

        #endregion



        #region Methods

        /// <summary>
        /// Вызов обработчиков для преобразования данных.
        /// </summary>
        public Result<InputData<TIn>, ErrorResultMiddleWareInData> HandleInvoke(InputData<TIn> inData)
        {
            var inDataClone = inData.Clone(FieldType.Both);
            string error;
            var errorHandlerWrapper= new ErrorResultMiddleWareInData();

            foreach (var data in inDataClone.Data)
            {
                Parallel.ForEach(_stringHandlers, (stringHandler) =>
                {
                    var propName = stringHandler.PropName;
                    var resultGet = _mutationsServiseStr.GetPropValue(data, propName);
                    if (resultGet.IsSuccess)
                    {
                        var tuple = resultGet.Value;
                        try
                        {
                            var newValue= stringHandler.Convert(tuple.val, data.Id);
                            tuple.val = newValue;
                            var resultSet = _mutationsServiseStr.SetPropValue(tuple);
                            if (resultSet.IsFailure)
                            {
                                error =$"MiddlewareInvokeService.HandleInvoke.StringConvert. Ошибка установки свойства:  {resultSet.Error}";
                                errorHandlerWrapper.AddError(error);
                            }
                        }
                        catch (StringConverterException ex)
                        {
                            error = $"MiddlewareInvokeService.HandleInvoke.StringConvert. Exception в конверторе:  {ex}";
                            errorHandlerWrapper.AddError(error);
                        }
                        catch (Exception e)
                        {
                            error = $"MiddlewareInvokeService.HandleInvoke.StringConvert. НЕИЗВЕСТНОЕ ИСКЛЮЧЕНИЕ:  {e}";
                            errorHandlerWrapper.AddError(error);
                        }
                    }
                    else
                    {
                        error =$"MiddlewareInvokeService.HandleInvoke.StringConvert.  Ошибка получения стркового свойства:  {resultGet.Error}";
                        errorHandlerWrapper.AddError(error);
                    }
                });
            }

            var res = errorHandlerWrapper.IsEmpty ?
                Result.Ok<InputData<TIn>, ErrorResultMiddleWareInData>(inDataClone) :
                Result.Failure<InputData<TIn>, ErrorResultMiddleWareInData>(errorHandlerWrapper);

            return res;
        }

        //СТАРАЯ ВЕРСИЯ ПАРАЛЛЕЛЬНОЙ ОБРАБОТКИ
        //public Result<InputData<TIn>, ErrorResultMiddleWareInData> HandleInvoke(InputData<TIn> inData)
        //{
        //    var inDataClone = inData.Clone(FieldType.Both);
        //    string error;
        //    var errorHandlerWrapper = new ErrorResultMiddleWareInData();
        //    Parallel.ForEach(inDataClone.Data, (data) =>
        //    {
        //        Parallel.ForEach(_stringHandlers, (stringHandler) =>
        //        {
        //            var propName = stringHandler.PropName;
        //            var resultGet = _mutationsServiseStr.GetPropValue(data, propName);
        //            if (resultGet.IsSuccess)
        //            {
        //                var tuple = resultGet.Value;
        //                try
        //                {
        //                    var newValue = stringHandler.Convert(tuple.val, data.Id);
        //                    tuple.val = newValue;
        //                    var resultSet = _mutationsServiseStr.SetPropValue(tuple);
        //                    if (resultSet.IsFailure)
        //                    {
        //                        error = $"MiddlewareInvokeService.HandleInvoke.StringConvert. Ошибка установки свойства:  {resultSet.Error}";
        //                        errorHandlerWrapper.AddError(error);
        //                    }
        //                }
        //                catch (StringConverterException ex)
        //                {
        //                    error = $"MiddlewareInvokeService.HandleInvoke.StringConvert. Exception в конверторе:  {ex}";
        //                    errorHandlerWrapper.AddError(error);
        //                }
        //                catch (Exception e)
        //                {
        //                    error = $"MiddlewareInvokeService.HandleInvoke.StringConvert. НЕИЗВЕСТНОЕ ИСКЛЮЧЕНИЕ:  {e}";
        //                    errorHandlerWrapper.AddError(error);
        //                }
        //            }
        //            else
        //            {
        //                error = $"MiddlewareInvokeService.HandleInvoke.StringConvert.  Ошибка получения стркового свойства:  {resultGet.Error}";
        //                errorHandlerWrapper.AddError(error);
        //            }
        //        });
        //    });

        //    var res = errorHandlerWrapper.IsEmpty ?
        //        Result.Ok<InputData<TIn>, ErrorResultMiddleWareInData>(inDataClone) :
        //        Result.Failure<InputData<TIn>, ErrorResultMiddleWareInData>(errorHandlerWrapper);

        //    return res;
        //}

        #endregion
    }


    public class ErrorResultMiddleWareInData
    {
        private readonly ConcurrentDictionary<Guid, string> _errorsDict = new ConcurrentDictionary<Guid, string>();

        public List<string> GetErrors => _errorsDict.Select(pair => pair.Value).ToList();
        public bool IsEmpty => _errorsDict.IsEmpty;
        public string GetErrorsArgegator => GetErrors.Aggregate((s, s1) => s + "  " + s1);


        public void AddError(string error)
        {
            _errorsDict.TryAdd(Guid.NewGuid(), error);
        }
    }

}