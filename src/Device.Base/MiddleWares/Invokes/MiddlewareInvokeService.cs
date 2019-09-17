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

        #endregion



        #region prop

        /// <summary>
        /// Заполнить ыходной буффер.
        /// Если данные новые, то перезапускается таймер, перезаписывается буффер и сразу вызывается HandleInvoke
        /// </summary>
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

        #endregion



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
            //TODO: Можно кешировать результат res. По внешнему флагу IsСache выстваляемому в Device.
            //IsСache == true => HandleInvoke не вызываем передаем предыдушший результат.
            //IsСache == false => HandleInvoke вызываем переписываем результат.
            //Device управляет IsСache след. образом: Если режим ByTimer, то пока все обмены не отправили предыдущую порцию данных IsСache = true (берем из кеша).
            //Когда все обмены отправили данные IsСache = false (вычисляем новые данные).
            //При обновлении данных на входе IsСache = false.
            //Это зашита от потери данных при использовании Mem конверторов, которые при каждом вызове выдают новые данные и если система отправки (очередь обменов) медленнее чем время предобработки, то данные потеряются.
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