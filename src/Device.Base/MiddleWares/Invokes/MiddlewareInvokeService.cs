using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Timers;
using CSharpFunctionalExtensions;
using Domain.Device.Enums;
using Domain.InputDataModel.Base.InData;
using KellermanSoftware.CompareNetObjects;
using Serilog;
using InvokerOutput = Domain.Device.Repository.Entities.MiddleWareOption.InvokerOutput;


namespace Domain.Device.MiddleWares.Invokes
{
    public class MiddlewareInvokeService<TIn> : IDisposable
    {
        #region fields

        private readonly InvokerOutput _option;
        private readonly ISupportMiddlewareInvoke<TIn> _invoker;
        private readonly ILogger _logger;
        private readonly Timer _timerInvoke;
        private InputData<TIn> _buferInData;
        /// <summary>
        /// Обратная связь
        /// </summary>
        private bool _feedBackEnable;

        #endregion



        #region prop
        /// <summary>
        /// Заполнить входной буфер.
        /// Если данные новые, то перезапускается таймер, перезаписывается буфер и сразу вызывается HandleInvoke
        /// </summary>
        private InputData<TIn> BuferInData
        {
            get => _buferInData;
            set
            {
                var compareRes = Comparer(_buferInData, value);
                if (compareRes.IsFailure)
                {
                    RefreshData(value);
                }
            }
        }

        /// <summary>
        /// Режим работы Инвокера.
        /// </summary>
        public InvokerOutputMode InvokerOutputMode { get; private set; }
        #endregion



        #region ctor
        public MiddlewareInvokeService(InvokerOutput option, ISupportMiddlewareInvoke<TIn> invoker, ILogger logger)
        {
            _option = option;
            _invoker = invoker;
            _logger = logger;
            InvokerOutputMode = _option.Mode;

            _timerInvoke = new Timer();
            _timerInvoke.Elapsed += TimerElapsed;
            if (_option.Mode == InvokerOutputMode.ByTimer)
            {
                RestartTimer();
            }
        }
        #endregion



        #region RxEvent
        /// <summary>
        /// СОБЫТИЕ Завершения вобработки данных
        /// </summary>
        public ISubject<Result<InputData<TIn>, ErrorResultMiddleWareInData>> InvokeIsCompleteRx { get; } = new Subject<Result<InputData<TIn>, ErrorResultMiddleWareInData>>();
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
                    await InputSetInstantly(inData);
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


        /// <summary>
        /// Установить обратную связь.
        /// Это разрешает вызов обработки.
        /// </summary>
        public void SetFeedBack()
        {
            _feedBackEnable = true;
            if (InvokerOutputMode == InvokerOutputMode.FeedBackWaiting)
            {
                RestartTimer();
                HandleInvokeByFeedBack(_buferInData);
                InvokerOutputMode = _option.Mode;
            }
        }


        /// <summary>
        /// Обработчик с ожиданием обратной связи.
        /// </summary>
        private void HandleInvokeByFeedBack(InputData<TIn> inData)
        {
            if (_feedBackEnable)
            {
                HandleInvoke(inData);
                _feedBackEnable = false;
            }
            else
            {
                InvokerOutputMode = InvokerOutputMode.FeedBackWaiting;
                _logger.Warning("Обратная связь НЕ выставленна при вызове обработчика. Переход в режим FeedBackWaiting. Подготовка данных ByTimer работатет быстрее чем поступает обратная связь об отправки подготовленных дангных транспортом. ");
            }
        }


        /// <summary>
        /// Обработчик
        /// </summary>
        private void HandleInvoke(InputData<TIn> inData)
        {
            var res = _invoker.HandleInvoke(inData);
            InvokeIsCompleteRx.OnNext(res);
        }


        /// <summary>
        /// Обновление данных. Сброс таймера и мгновенный вызов обработчика
        /// </summary>
        /// <param name="newData">Новые входны данные</param>
        private void RefreshData(InputData<TIn> newData)
        {
            InvokerOutputMode = _option.Mode;
            _buferInData = newData;
            RestartTimer();
            HandleInvoke(_buferInData);
        }


        /// <summary>
        /// Перезапуск таймера.
        /// </summary>
        private void RestartTimer()
        {
            _timerInvoke.Stop();
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

            HandleInvokeByFeedBack(BuferInData);
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