using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Timers;
using Autofac.Features.OwnedInstances;
using CSharpFunctionalExtensions;
using Domain.Device.Enums;
using Domain.Device.Repository.Entities.MiddleWareOption;
using Domain.InputDataModel.Base.InData;
using KellermanSoftware.CompareNetObjects;
using Serilog;
using Shared.MiddleWares.Converters;
using InvokerOutput = Domain.Device.Repository.Entities.MiddleWareOption.InvokerOutput;

namespace Domain.Device.MiddleWares.Invokes
{
    /// <summary>
    /// Определяет способ запуска обработчика IMiddlewareInvoke.
    /// "Instantly" - с приходом данных сразу запускается обработка.
    /// "ByTimer" - Обработчик запускается по внутреннему таймеру.
    /// Если приходят новые данные, то таймер перезапускается и обработчик вызывается сразу.
    /// "FeedBackWaiting" - В этот режим система переходит САМА, если метод SetFeedBack (выставляет флаг обратной связи)
    /// был вызван ПОЗЖЕ, чем прошла обработка вызванная ByTimer (в режиме Instantly обратная связь не работает).
    /// </summary>
    public class MiddlewareInvokeService<TIn> : IDisposable
    {
        #region fields
        private readonly InvokerOutput _option;
        private readonly string _description;
        private readonly ISupportMiddlewareInvoke<TIn> _supportMiddleware;
        private readonly IDisposable _middlewareOwner;
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
        public MiddlewareInvokeService(MiddleWareInDataOption option, Func<MiddleWareInDataOption, Owned<ISupportMiddlewareInvoke<TIn>>> middlewareFactory, ILogger logger)
        {
            _option = option.InvokerOutput;
            _description = option.Description;
            var middlewareOwner = middlewareFactory(option);
            _middlewareOwner = middlewareOwner;
            _supportMiddleware = middlewareOwner.Value;
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
        /// Установить флаг обратной связи.
        /// Если система находится в состоянии FeedBackWaiting,
        /// то сбрасывается таймер и HandleInvoke вызывается сразу.
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
        /// Отправить команду всем Mem конверторам
        /// </summary>
        /// <param name="command"></param>
        public void SendCommand4MemConverters(MemConverterCommand command)
        {
            _supportMiddleware.SendCommand4MemConverters(command);
        }


        /// <summary>
        /// Обработчик с ожиданием обратной связи.
        /// Если флаг обратной связи установлен, то вызывается обработчик HandleInvoke.
        /// Если флаг обратной связи НЕ установлен, то перводится система в состоянии FeedBackWaiting. 
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
                _logger.Warning($"Обратная связь НЕ выставленна при вызове обработчика. Переход в режим FeedBackWaiting. Подготовка данных ByTimer работатет быстрее чем поступает обратная связь об отправки подготовленных данных транспортом. \"{_description}\"");
            }
        }


        /// <summary>
        /// Обработчик
        /// </summary>
        private void HandleInvoke(InputData<TIn> inData)
        {
            var res = _supportMiddleware.HandleInvoke(inData);
            InvokeIsCompleteRx.OnNext(res);
        }


        /// <summary>
        /// Обновление данных.
        /// Команда сброса на все MEM конверторы, Сброс таймера и мгновенный вызов обработчика.
        /// </summary>
        /// <param name="newData">Новые входны данные</param>
        private void RefreshData(InputData<TIn> newData)
        {
            InvokerOutputMode = _option.Mode;
            _buferInData = newData;
            SendCommand4MemConverters(MemConverterCommand.Reset);
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
            return result.AreEqual ? Result.Ok(true) : Result.Failure<bool>(result.DifferencesString);
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
            _middlewareOwner.Dispose();
        }

        #endregion
    }
}