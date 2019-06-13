using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Timers;
using CSharpFunctionalExtensions;
using DAL.Abstract.Entities.Options.MiddleWare;
using DeviceForExchange.MiddleWares.Handlers;
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
    public class MiddleWareInData<TIn> : IDisposable
    {
        #region fields

        private readonly MiddleWareInDataOption _option;
        private readonly ILogger _logger;
        private InputData<TIn> _buferInData;
        private readonly Timer _timerHandleInvoke;

        private readonly PropertyMutationsServise<string> _mutationsServiseStr = new PropertyMutationsServise<string>();    //Сервис изменения совойства типа string, по имени, через рефлексию.
        private readonly PropertyMutationsServise<DateTime> _mutationsServiseDt = new PropertyMutationsServise<DateTime>(); //Сервис изменения совойства типа DateTime, по имени, через рефлексию.

        #endregion



        #region prop

        public List<StringHandlerMiddleWare> StringHandlers { get; }
        public List<DateTimeHandlerMiddleWare> DateTimeHandlers { get; }

        #endregion



        #region ctor

        public MiddleWareInData(MiddleWareInDataOption option, ILogger logger)
        {
            _option = option;
            _logger = logger;

            StringHandlers= option.StringHandlers?.Select(handler => new StringHandlerMiddleWare(handler)).ToList();
            DateTimeHandlers = option.DateTimeHandlers?.Select(handler => new DateTimeHandlerMiddleWare(handler)).ToList();

            _timerHandleInvoke = new Timer();
            _timerHandleInvoke.Elapsed += TimerElapsed;
            CheckStartTimer();
        }

        #endregion



        #region Methods


        /// <summary>
        /// Прием входных данных.
        /// </summary>
        public async Task InputSet(InputData<TIn> inData)
        {
            _buferInData = inData;
            switch (_option.InvokerOutput.Mode)
            {
                case InvokerOutputMode.Instantly:
                    //Обработка в конвеере данных.
                    var res=  HandleInvoke(_buferInData);
                    InvokeReadyRx.OnNext(res);
                    break;
            }
            
            await Task.CompletedTask;
        }


        /// <summary>
        /// Вызов обработчиков для преобразования данных
        /// </summary>
        private Result<InputData<TIn>> HandleInvoke(InputData<TIn> inData)
        {
            string error;
            var errors = new List<string>();
            Parallel.ForEach(inData.Data, (data) =>
            {
                Parallel.ForEach(StringHandlers, (stringHandler) =>
                {
                    var propName = stringHandler.PropName;
                    var resultGet = _mutationsServiseStr.GetPropValue(data, propName);
                    if (resultGet.IsSuccess)
                    {
                        var tuple = resultGet.Value;
                        try
                        {
                            var newValue = stringHandler.Convert(tuple.val); //TODO: Создать новый тип искобчений для конверторов.
                            tuple.val = newValue;
                            var resultSet = _mutationsServiseStr.SetPropValue(tuple);
                            if (resultSet.IsFailure)
                            {
                                error =$"MiddleWareInData.HandleInvoke Ошибка установки стркового свойства.  {resultSet.Error}";
                                errors.Add(error);
                                //_logger.Error(error);
                            }
                        }
                        catch (Exception e)
                        {
                            error = $"MiddleWareInData.HandleInvoke Ошибка в String конверторе. {e}";
                            errors.Add(error);
                            //_logger.Error(error);
                        }
                    }
                    else
                    {
                        error = $"MiddleWareInData.HandleInvoke Ошибка получения стркового свойства.  {resultGet.Error}";
                        errors.Add(error);
                        //_logger.Error(error);
                    }
                });


                //ОБРАБОТЧИКИ DateTime
                //foreach (var dateTimeHandler in DateTimeHandlers)
                //{

                //}
            });

            return errors.Count == 0 ? Result.Ok(_buferInData) : Result.Fail<InputData<TIn>, List<string>>(errors);
        }


        private void CheckStartTimer()
        {
            if (_option.InvokerOutput.Mode == InvokerOutputMode.ByTimer)
            {
                StartTimer();
            }
        }


        private void StartTimer()
        {
            _timerHandleInvoke.Interval = _option.InvokerOutput.Time;
            _timerHandleInvoke.Start();
        }


        #endregion



        #region RxEvent

        /// <summary>
        /// Выходные данные функции
        /// </summary>
        public ISubject<Result<InputData<TIn>>> InvokeReadyRx { get; } = new Subject<Result<InputData<TIn>>>();    //СОБЫТИЕ Готовности выходных данных после обработки

        #endregion



        #region EventHandler

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            //Обработка в конвеере данных.
            var res = HandleInvoke(_buferInData);
            InvokeReadyRx.OnNext(res);
        }

        #endregion




        #region Disposable

        public void Dispose()
        {
            _timerHandleInvoke?.Dispose();
        }

        #endregion
    }
}