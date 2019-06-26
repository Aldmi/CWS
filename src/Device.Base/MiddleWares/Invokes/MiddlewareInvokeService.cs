using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Timers;
using CSharpFunctionalExtensions;
using DAL.Abstract.Entities.Options.MiddleWare;
using InputDataModel.Base;
using KellermanSoftware.CompareNetObjects;
using Serilog;

namespace DeviceForExchange.MiddleWares.Invokes
{
    public class MiddlewareInvokeService<TIn> : IDisposable
    {
        private readonly InvokerOutput _option;
        private readonly ISupportMiddlewareInvoke<TIn> _invoker;
        private readonly ILogger _logger;
  
        private readonly Timer _timerInvoke;

        private InputData<TIn> _buferInData;
        private InputData<TIn> BuferInData
        {
            get => _buferInData;
            set
            {
                var compareRes = Comparer(_buferInData, value);
                if (compareRes.IsFailure)
                {
                    _timerInvoke.Stop();
                    _buferInData = value;
                    HandleInvoke(_buferInData);
                    _timerInvoke.Interval = _option.Time;
                    _timerInvoke.Start();
                }
            }
        }


        #region ctor

        public MiddlewareInvokeService(InvokerOutput option, ISupportMiddlewareInvoke<TIn> invoker, ILogger logger)
        {
            _option = option;
            _invoker = invoker;
            _logger = logger;

            _timerInvoke = new Timer();
            _timerInvoke.Elapsed += TimerElapsed;
            CheckStartTimer();
        }

        #endregion



        #region RxEvent

        /// <summary>
        /// СОБЫТИЕ Завершения вобработки данных
        /// </summary>
        public ISubject<Result<InputData<TIn>, ErrorMiddleWareInDataWrapper>> InvokeIsCompleteRx { get; } = new Subject<Result<InputData<TIn>, ErrorMiddleWareInDataWrapper>>();

        #endregion




        #region Methods

        /// <summary>
        /// Прием входных данных. InvokerOutputMode определяется опциями.
        /// </summary>
        public async Task InputSetByOptionMode(InputData<TIn> inData)
        {
            switch (_option.Mode)
            {
                case InvokerOutputMode.Instantly:
                    HandleInvoke(inData);
                    break;

                case InvokerOutputMode.ByTimer:
                    BuferInData = inData;
                    break;
            }
            await Task.CompletedTask;
        }


        /// <summary>
        /// Прием входных данных, сразу с вызовом обработчика.
        /// </summary>
        public async Task InputSetInstantly(InputData<TIn> inData)
        {
            HandleInvoke(inData);
            await Task.CompletedTask;
        }



        private void HandleInvoke(InputData<TIn> inData)
        {
            var res = _invoker.HandleInvoke(inData);
            InvokeIsCompleteRx.OnNext(res);
        }
        


        private void CheckStartTimer()
        {
            if (_option.Mode == InvokerOutputMode.ByTimer)
            {
                StartTimer();
            }
        }


        private void StartTimer()
        {
            _timerInvoke.Interval = _option.Time;
            _timerInvoke.Start();
        }

        /// <summary>
        /// Сравнить 2 элемента.
        /// </summary>
        private Result<bool> Comparer(InputData<TIn> obj1, InputData<TIn> obj2)
        {
            var compareLogic = new CompareLogic { Config = { MaxMillisecondsDateDifference = 1000 } };
            ComparisonResult result = compareLogic.Compare(obj1, obj2);
            return result.AreEqual ? Result.Ok(true) : Result.Fail<bool>(result.DifferencesString);
        }


        #endregion




        #region EventHandler

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if(BuferInData?.Data == null)
                return;

            HandleInvoke(BuferInData);
        }

        #endregion




        #region Disposable

        public void Dispose()
        {
            _timerInvoke?.Dispose();
        }

        #endregion
    }
}