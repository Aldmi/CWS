using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Domain.Device.MiddleWares4InData.Handlers4InData;
using Domain.Device.MiddleWares4InData.Invokes;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Base.InData;
using FastDeepCloner;
using Serilog;
using Shared.MiddleWares.Converters;
using Shared.MiddleWares.Converters.Exceptions;
using Shared.ReflectionServices;

namespace Domain.Device.MiddleWares4InData
{
    /// <summary>
    /// Промежуточный обработчик входных данных.
    /// Сервис является иммутабельным. Он создается на базе MiddleWareInDataOption и его State не меняется
    /// </summary>
    /// <typeparam name="TIn">Входные данные для обработки</typeparam>
    public class MiddleWareMediator<TIn> : ISupportMiddlewareInvoke<TIn> where TIn : InputTypeBase
    {
        #region fields
        private readonly MiddleWareMediatorOption _option;
        private readonly ILogger _logger;

        private readonly PropertyMutationsServise<string> _mutationsServiseStr = new PropertyMutationsServise<string>();    //Сервис изменения свойства типа string, по имени, через рефлексию.
        private readonly PropertyMutationsServise<DateTime> _mutationsServiseDt = new PropertyMutationsServise<DateTime>(); //Сервис изменения свойства типа DateTime, по имени, через рефлексию.
        private readonly PropertyMutationsServise<Enum> _mutationsServiseEnum = new PropertyMutationsServise<Enum>();      //Сервис изменения свойства типа enum(byte), по имени, через рефлексию.
        //private readonly PropertyMutationsServise<DateTime?> _mutationsServiseDtN = new PropertyMutationsServise<DateTime?>(); //Сервис изменения свойства типа DateTime, по имени, через рефлексию.
        private readonly PropertyMutationsServise<object> _mutationsServiseObj = new PropertyMutationsServise<object>();       //Сервис изменения свойства типа object (целых объектов), по имени, через рефлексию.

        private readonly List<StringHandlerMiddleWare4InData> _stringHandlers;
        private readonly List<DateTimeHandlerMiddleWare4InData> _dateTimeHandlers;
        private readonly List<EnumHandlerMiddleWare4InData> _enumHandlers;
        private readonly List<ObjectHandlerMiddleWare4InData> _objectHandlers;
        #endregion



        #region prop
        public string Description => _option.Description;
        #endregion



        #region ctor
        public MiddleWareMediator(MiddleWareMediatorOption option, ILogger logger)
        {
            _option = option;
            _logger = logger;

            _stringHandlers = option.StringHandlers?.
                Select(h => new StringHandlerMiddleWare4InData(h)).ToList();

            _dateTimeHandlers = option.DateTimeHandlers?.
                Select(h => new DateTimeHandlerMiddleWare4InData(h)).ToList();

            _enumHandlers = option.EnumHandlers?.
                Select(h => new EnumHandlerMiddleWare4InData(h)).ToList();

            _objectHandlers = option.ObjectHandlers?.
                Select(h => new ObjectHandlerMiddleWare4InData(h)).ToList();
        }
        #endregion



        #region Methods

        /// <summary>
        /// Вызов обработчиков для преобразования данных.
        /// Паралельно обрабатываются входные данные и парллельно свойства внутри единицы данных.
        /// </summary>
        public Result<InputData<TIn>, ErrorResultMiddleWareInData> HandleInvoke(InputData<TIn> inData)
        {
            var inDataClone = inData.Clone(FieldType.Both);
            string error;
            var errorHandlerWrapper = new ErrorResultMiddleWareInData();
            Parallel.ForEach(inDataClone.Data, (data) =>
                {
                    if (_stringHandlers != null)
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
                                    var newValue = stringHandler.Convert(tuple.val, data.Id);
                                    tuple.val = newValue;
                                    var resultSet = _mutationsServiseStr.SetPropValue(tuple);
                                    if (resultSet.IsFailure)
                                    {
                                        error = $"MiddlewareInvokeService.HandleInvoke.StringConvert. Ошибка установки свойства:  {resultSet.Error}";
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
                                error = $"MiddlewareInvokeService.HandleInvoke.StringConvert.  Ошибка получения стркового свойства:  {resultGet.Error}";
                                errorHandlerWrapper.AddError(error);
                            }
                        });
                    }

                    if (_enumHandlers != null)
                    {
                        Parallel.ForEach(_enumHandlers, (enumHandler) =>
                        {
                            var propName = enumHandler.PropName;
                            var resultGet = _mutationsServiseEnum.GetPropValue(data, propName);
                            if (resultGet.IsSuccess)
                            {
                                var tuple = resultGet.Value;
                                try
                                {
                                    var newValue = enumHandler.Convert(tuple.val, data.Id);
                                    tuple.val = newValue;
                                    var resultSet = _mutationsServiseEnum.SetPropValue(tuple);
                                    if (resultSet.IsFailure)
                                    {
                                        error = $"MiddlewareInvokeService.HandleInvoke.EnumConvert. Ошибка установки свойства:  {resultSet.Error}";
                                        errorHandlerWrapper.AddError(error);
                                    }
                                }
                                catch (StringConverterException ex)
                                {
                                    error = $"MiddlewareInvokeService.HandleInvoke.EnumConvert. Exception в конверторе:  {ex}";
                                    errorHandlerWrapper.AddError(error);
                                }
                                catch (Exception e)
                                {
                                    error = $"MiddlewareInvokeService.HandleInvoke.EnumConvert. НЕИЗВЕСТНОЕ ИСКЛЮЧЕНИЕ:  {e}";
                                    errorHandlerWrapper.AddError(error);
                                }
                            }
                            else
                            {
                                error = $"MiddlewareInvokeService.HandleInvoke.EnumConvert.  Ошибка получения стркового свойства:  {resultGet.Error}";
                                errorHandlerWrapper.AddError(error);
                            }
                        });
                    }

                    if (_dateTimeHandlers != null)
                    {
                        Parallel.ForEach(_dateTimeHandlers, (handler) =>
                        {
                            var propName = handler.PropName;
                            var resultGet = _mutationsServiseDt.GetPropValue(data, propName);
                            if (resultGet.IsSuccess)
                            {
                                var tuple = resultGet.Value;
                                try
                                {
                                    var newValue = handler.Convert(tuple.val, data.Id);
                                    tuple.val = newValue;
                                    var resultSet = _mutationsServiseDt.SetPropValue(tuple);
                                    if (resultSet.IsFailure)
                                    {
                                        error = $"MiddlewareInvokeService.HandleInvoke.DateTimeConverter. Ошибка установки свойства:  {resultSet.Error}";
                                        errorHandlerWrapper.AddError(error);
                                    }
                                }
                                catch (StringConverterException ex)
                                {
                                    error = $"MiddlewareInvokeService.HandleInvoke.DateTimeConverter. Exception в конверторе:  {ex}";
                                    errorHandlerWrapper.AddError(error);
                                }
                                catch (Exception e)
                                {
                                    error = $"MiddlewareInvokeService.HandleInvoke.DateTimeConverter. НЕИЗВЕСТНОЕ ИСКЛЮЧЕНИЕ:  {e}";
                                    errorHandlerWrapper.AddError(error);
                                }
                            }
                            else
                            {
                                error = $"MiddlewareInvokeService.HandleInvoke.DateTimeConverter.  Ошибка получения строкового свойства:  {resultGet.Error}";
                                errorHandlerWrapper.AddError(error);
                            }
                        });
                    }

                    if (_objectHandlers != null)
                    {
                        Parallel.ForEach(_objectHandlers, (objHandler) =>
                        {
                            var propName = objHandler.PropName;
                            var resultGet = _mutationsServiseObj.GetPropValue(data, propName);
                            if (resultGet.IsSuccess)
                            {
                                var tuple = resultGet.Value;
                                try
                                {
                                    var newValue = objHandler.Convert(tuple.val, data.Id);
                                    tuple.val = newValue;
                                    var resultSet = _mutationsServiseObj.SetPropValue(tuple);
                                    if (resultSet.IsFailure)
                                    {
                                        error = $"MiddlewareInvokeService.HandleInvoke.ObjectConvert. Ошибка установки свойства:  {resultSet.Error}";
                                        errorHandlerWrapper.AddError(error);
                                    }
                                }
                                catch (StringConverterException ex)
                                {
                                    error = $"MiddlewareInvokeService.HandleInvoke.ObjectConvert. Exception в конверторе:  {ex}";
                                    errorHandlerWrapper.AddError(error);
                                }
                                catch (Exception e)
                                {
                                    error = $"MiddlewareInvokeService.HandleInvoke.ObjectConvert. НЕИЗВЕСТНОЕ ИСКЛЮЧЕНИЕ:  {e}";
                                    errorHandlerWrapper.AddError(error);
                                }
                            }
                            else
                            {
                                error = $"MiddlewareInvokeService.HandleInvoke.ObjectConvert.  Ошибка получения стркового свойства:  {resultGet.Error}";
                                errorHandlerWrapper.AddError(error);
                            }
                        });
                    }



                    //DEBUG Nullable dateTime-------------
                    //var firstHandler = _dateTimeHandlers.FirstOrDefault();
                    //var propName1 = "DepartureTime";
                    //var resultGet1 = _mutationsServiseDtN.GetPropValue(data, propName1);
                    //if (resultGet1.IsSuccess)
                    //{
                    //    var tuple = resultGet1.Value;
                    //    try
                    //    {

                    //        //var newValue = DateTime.Now;//firstHandler.Convert((DateTime)tuple.val, data.Id);
                    //        tuple.val = null;// newValue;
                    //        var resultSet = _mutationsServiseDtN.SetPropValue(tuple);
                    //        if (resultSet.IsFailure)
                    //        {
                    //            error = $"MiddlewareInvokeService.HandleInvoke.DateTimeConverter. Ошибка установки свойства:  {resultSet.Error}";
                    //            errorHandlerWrapper.AddError(error);
                    //        }
                    //    }
                    //    catch (StringConverterException ex)
                    //    {
                    //        error = $"MiddlewareInvokeService.HandleInvoke.DateTimeConverter. Exception в конверторе:  {ex}";
                    //        errorHandlerWrapper.AddError(error);
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        error = $"MiddlewareInvokeService.HandleInvoke.DateTimeConverter. НЕИЗВЕСТНОЕ ИСКЛЮЧЕНИЕ:  {e}";
                    //        errorHandlerWrapper.AddError(error);
                    //    }
                    //}
                    //else
                    //{
                    //    error = $"MiddlewareInvokeService.HandleInvoke.DateTimeConverter.  Ошибка получения строкового свойства:  {resultGet1.Error}";
                    //    errorHandlerWrapper.AddError(error);
                    //}
                }
            );

            var res = errorHandlerWrapper.IsEmpty ?
                Result.Ok<InputData<TIn>, ErrorResultMiddleWareInData>(inDataClone) :
                Result.Failure<InputData<TIn>, ErrorResultMiddleWareInData>(errorHandlerWrapper);

            return res;
        }


        public void SendCommand4MemConverters(MemConverterCommand command)
        {
            if (_stringHandlers != null)
            {
                Parallel.ForEach(_stringHandlers, (h) => { h.SendCommand4MemConverters(command); });
            }
            if (_dateTimeHandlers != null)
            {
                Parallel.ForEach(_dateTimeHandlers, (h) => { h.SendCommand4MemConverters(command); });
            }
            if (_enumHandlers != null)
            {
                Parallel.ForEach(_enumHandlers, (h) => { h.SendCommand4MemConverters(command); });
            }
        }
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