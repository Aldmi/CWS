﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Timers;
using DAL.Abstract.Entities.Options.MiddleWare;
using DeviceForExchange.MiddleWares.Handlers;
using InputDataModel.Base;
using Serilog;

namespace DeviceForExchange.MiddleWares
{
    /// <summary>
    /// Промежуточный обработчик входных данных.
    /// Сервис является иммутабельным. Он создается на базе MiddleWareInDataOption и его State не меняется
    /// </summary>
    /// <typeparam name="TIn">Входные данные для обработки</typeparam>
    public class MiddleWareInData<TIn> : IDisposable
    {
        private readonly MiddleWareInDataOption _option;
        private readonly ILogger _logger;
        private InputData<TIn> _buferInData;
        private readonly Timer _timerHandleInvoke;



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
            StartTimer();
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
                    HandleInvoke(inData.Data);
                    InvokeReadyRx.OnNext(_buferInData);
                    break;
            }
            
            await Task.CompletedTask;
        }

        /// <summary>
        /// Вызов обработчиков для преобразования данных
        /// </summary>
        /// <param name="datas"></param>
        private void HandleInvoke(IEnumerable<TIn> datas)
        {
            //TODO: продумать паралельную обработку
            foreach (var data in datas)
            {
                //ОБРАБОТЧИКИ String

                string note = null;//DEBUG
                Parallel.ForEach(StringHandlers, (stringHandler) =>
                {
                    var propName = stringHandler.PropName;
                    var str = "Начальная строка"; //Найденное свойство
                    var res = stringHandler.Convert(str);
                    str = res; //перезаписали занчение свойства
                    note = str;
                });
                

                //ОБРАБОТЧИКИ DateTime
                //foreach (var dateTimeHandler in DateTimeHandlers)
                //{
                    
                //}
            }
        }


        private void StartTimer()
        {
            if (_option.InvokerOutput.Mode == InvokerOutputMode.ByTimer)
            {
                _timerHandleInvoke.Interval = _option.InvokerOutput.Time;
                _timerHandleInvoke.Start();
            }
        }


        #endregion



        #region RxEvent

        /// <summary>
        /// Выходные данные функции
        /// </summary>
        public ISubject<InputData<TIn>> InvokeReadyRx { get; } = new Subject<InputData<TIn>>();    //СОБЫТИЕ Готовности выходных данных после обработки

        #endregion



        #region EventHandler

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            //Обработка в конвеере данных.
            HandleInvoke(_buferInData.Data);
            InvokeReadyRx.OnNext(_buferInData);
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