using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using DAL.Abstract.Entities.Options.MiddleWare;
using InputDataModel.Base;
using Serilog;

namespace DeviceForExchange.MiddleWares
{
    /// <summary>
    /// Промежуточный обработчик входных данных.
    /// </summary>
    /// <typeparam name="TIn">Входные данные для обработки</typeparam>
    public class MiddleWareInData<TIn>
    {
        private readonly MiddleWareInDataOption _option;
        private readonly ILogger _logger;
        private InputData<TIn> _buferInData;



        #region ctor

        public MiddleWareInData(MiddleWareInDataOption option, ILogger logger)
        {
            _option = option;
            _logger = logger;
        }

        #endregion



        #region RxEvent

        public ISubject<InputData<TIn>> OutputReadyRx { get; } = new Subject<InputData<TIn>>();    //СОБЫТИЕ Готовности выходных данных после обработки

        #endregion



        #region Methods

        public async Task InputSet(InputData<TIn> inData)
        {
            _buferInData = inData;
            switch (_option.InvokerOutput.Mode)
            {
                case InvokerOutputMode.Instantly:
                    //Обработка в конвеере данных.
                    OutputReadyRx.OnNext(_buferInData);
                    break;
            }
            
            await Task.CompletedTask;
        }

        #endregion




        #region EventHandler

        private void TimerRaise()
        {
            //Обработка в конвеере данных.
            OutputReadyRx.OnNext(_buferInData);
        }

        #endregion
    }
}