using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Timers;
using CSharpFunctionalExtensions;
using DAL.Abstract.Entities.Options.MiddleWare;
using DeviceForExchange.MiddleWares.Handlers;
using DeviceForExchange.MiddleWares.Invokes;
using InputDataModel.Base;
using Serilog;
using Shared.ReflectionServices;

namespace DeviceForExchange.MiddleWares
{
    /// <summary>
    /// Промежуточный обработчик входных данных.
    /// Сервис является иммутабельным. Он создается на базе MiddleWareInDataOption и его State не меняется
    /// </summary>
    /// <typeparam name="TIn">Входные данные для обработки</typeparam>
    public class MiddleWareInData<TIn> : ISupportMiddlewareInvoke<TIn>
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
        public Result<InputData<TIn>, ErrorMiddleWareInDataWrapper> HandleInvoke(InputData<TIn> inData)
        {
            string error;
            var errorHandlerWrapper= new ErrorMiddleWareInDataWrapper();
            Parallel.ForEach(inData.Data, (data) =>
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
                            var newValue= stringHandler.Convert(tuple.val); //TODO: Создать новый тип искобчений для конверторов.
                            tuple.val = newValue;
                            var resultSet = _mutationsServiseStr.SetPropValue(tuple);
                            if (resultSet.IsFailure)
                            {
                                error =$"MiddlewareInvokeService.HandleInvoke Ошибка установки стркового свойства.  {resultSet.Error}";
                                errorHandlerWrapper.AddError(error);
                                //_logger.Error(error);
                            }
                        }
                        catch (Exception e)
                        {
                            error = $"MiddlewareInvokeService.HandleInvoke Exception в String конверторе. {e}";
                            errorHandlerWrapper.AddError(error);
                            //_logger.Error(error);
                        }
                    }
                    else
                    {
                        error =$"MiddlewareInvokeService.HandleInvoke Ошибка получения стркового свойства.  {resultGet.Error}";
                        errorHandlerWrapper.AddError(error);
                        //_logger.Error(error);
                    }
                });
            });

            var res = errorHandlerWrapper.IsEmpty ?
                Result.Ok<InputData<TIn>, ErrorMiddleWareInDataWrapper>(inData) :
                Result.Fail<InputData<TIn>, ErrorMiddleWareInDataWrapper>(errorHandlerWrapper);

            return res;
        }

        #endregion
    }


    //public class ErrorMiddleWareInDataWrapper
    //{
    //    private readonly ConcurrentDictionary<int, string> _errorsDict = new ConcurrentDictionary<int, string>();

    //    public List<string> GetErrors => _errorsDict.Select(pair => pair.Value).ToList();
    //    public bool IsEmpty => _errorsDict.IsEmpty;


    //    public void AddError(string error)
    //    {

    //        var key = _errorsDict.Count;
    //        _errorsDict.TryAdd(key, error);
    //    }
    //}



    public class ErrorMiddleWareInDataWrapper
    {
        private readonly ConcurrentDictionary<Guid, string> _errorsDict = new ConcurrentDictionary<Guid, string>();

        public List<string> GetErrors => _errorsDict.Select(pair => pair.Value).ToList();
        public bool IsEmpty => _errorsDict.IsEmpty;


        public void AddError(string error)
        {
            _errorsDict.TryAdd(Guid.NewGuid(), error);
        }
    }

}