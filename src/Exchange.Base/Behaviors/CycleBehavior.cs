using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Domain.Exchange.Enums;
using Domain.Exchange.Repository.Entities;
using Domain.Exchange.RxModel;
using Domain.Exchange.Services;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.Response;
using Infrastructure.Background.Abstarct;
using Serilog;
using Shared.Collections;

namespace Domain.Exchange.Behaviors
{
    public class CycleBehavior<TIn> : BaseBehavior<TIn>, IDisposable where TIn : InputTypeBase
    {
        #region field
        private readonly InputCycleDataEntryCheker _inputCycleDataEntryCheker;      //таймер отсчитывает период от получения входных данных для цикл. обмена.
        private readonly SkippingPeriodChecker _skippingPeriodChecker;              //таймер отсчитывает время пропуска периода опроса.
        private readonly string _keyExchange;
        public readonly CycleFuncOption CycleFuncOption;
        #endregion



        #region prop
        public CycleBehaviorState CycleBehaviorState { get; private set; } //TODO:???
        #endregion



        #region ctor
        public CycleBehavior(string keyExchange,
            ITransportBackground transportBackground,
            CycleFuncOption cycleFuncOption,
            Func<InDataWrapper<TIn>, CancellationToken, Task<ResponsePieceOfDataWrapper<TIn>>> pieceOfDataSender,
            ILogger logger,
            Func<int, InputCycleDataEntryCheker> inputCycleDataEntryChekerFactory,
            Func<int, SkippingPeriodChecker> skippingPeriodCheckerFactory) : base(keyExchange, transportBackground, cycleFuncOption.CycleQueueMode, pieceOfDataSender, logger)
        {
            _inputCycleDataEntryCheker = inputCycleDataEntryChekerFactory(cycleFuncOption.NormalIntervalCycleDataEntry);
            _inputCycleDataEntryCheker.CycleDataEntryStateChangeRx.Subscribe(CycleDataEntryStateChangeRxEventHandler);
            _skippingPeriodChecker = skippingPeriodCheckerFactory(cycleFuncOption.SkipInterval);
            _keyExchange = keyExchange;
            CycleFuncOption = cycleFuncOption;
        }
        #endregion



        #region InputDataChangeRx
        public ISubject<CycleBehaviorStateRxModel> CycleBehaviorStateChangeRx { get; } = new Subject<CycleBehaviorStateRxModel>();
        #endregion



        #region RxEventHandlers
        private void CycleDataEntryStateChangeRxEventHandler(InputDataStatus inputDataStatus)
        {
            switch (inputDataStatus)
            {
                case InputDataStatus.NormalEntry:
                    Switch2NormalCycleExchange();
                    break;
                case InputDataStatus.ToLongNoEntry:
                    Switch2CycleCommandEmergency();
                    break;
            }
        }
        #endregion



        #region CycleExchange
        /// <summary>
        /// Добавление ЦИКЛ. функций на БГ
        /// </summary>
        public void StartCycleExchange()
        {
            Switch2NormalCycleExchange();
            _inputCycleDataEntryCheker.StartChecking();
        }

        /// <summary>
        /// Удаление ЦИКЛ. функций из БГ
        /// </summary>
        public void StopCycleExchange()
        {
            TransportBackground.RemoveCycleFunc(CycleTimeActionAsync);
            TransportBackground.RemoveCycleFunc(CycleCommandEmergencyActionAsync);
            _inputCycleDataEntryCheker.StopChecking();
            CycleBehaviorState = CycleBehaviorState.Off;
            CycleBehaviorStateChangeRx.OnNext(new CycleBehaviorStateRxModel(_keyExchange, CycleBehaviorState));
        }

        /// <summary>
        /// перевести в режим НОРМАЛЬНЫЙ цикл. обмен на БГ
        /// </summary>
        public void Switch2NormalCycleExchange()
        {
            TransportBackground.RemoveCycleFunc(CycleCommandEmergencyActionAsync);
            TransportBackground.AddCycleAction(CycleTimeActionAsync);
            CycleBehaviorState = CycleBehaviorState.Normal;
            CycleBehaviorStateChangeRx.OnNext(new CycleBehaviorStateRxModel(_keyExchange, CycleBehaviorState));
        }

        /// <summary>
        /// перевести в режим АВАРИЙНЫЙ цикл. обмен на БГ
        /// </summary>
        public void Switch2CycleCommandEmergency()
        {
            TransportBackground.RemoveCycleFunc(CycleTimeActionAsync);
            TransportBackground.AddCycleAction(CycleCommandEmergencyActionAsync);
            CycleBehaviorState = CycleBehaviorState.Emergency;
            CycleBehaviorStateChangeRx.OnNext(new CycleBehaviorStateRxModel(_keyExchange, CycleBehaviorState));
        }
        #endregion



        #region Methode
        /// <summary>
        /// Выставить данные для цикл. функции.
        /// </summary>
        public void SendData(IEnumerable<TIn> inData, string directHandlerName)
        {
            if (inData == null)
                throw new ArgumentNullException($"{nameof(inData)} НЕ может быть NULL");

            _inputCycleDataEntryCheker.InputDataEntry();
            var dataWrapper = new InDataWrapper<TIn> { Datas = inData.ToList(), DirectHandlerName = directHandlerName };
            var result = DataQueue.Enqueue(dataWrapper);
            if (result.IsSuccess) //Добавленны НОВЫЕ данные в очередь.
            {
                _skippingPeriodChecker.StopSkipping();
            }
        }
        #endregion



        #region CycleActions
        private async Task CycleTimeActionAsync(CancellationToken ct)
        {
            if (_skippingPeriodChecker.IsSkip)
            {
                Debug.WriteLine("CycleTimeActionAsync SKIP ----------------------------------------");
                return;
            }

            InDataWrapper<TIn> inData = null;
            var result = DataQueue.Dequeue();
            if (result.IsSuccess)
            {
                inData = result.Value;
            }
            else
            {
                var errorResult = result.Error.DequeueResultError;
                if (errorResult == DequeueResultError.FailTryDequeue || errorResult == DequeueResultError.FailTryPeek)
                {
                    Logger.Error("{Type} {KeyExchange}  {MessageShort}", "Ошибка извлечения данных из ЦИКЛ. очереди", KeyExchange, errorResult.ToString());
                    return;
                }
            }
            var transportResponseWrapper = await PieceOfDataSender(inData, ct);
            transportResponseWrapper.KeyExchange = KeyExchange;
            transportResponseWrapper.DataAction = DataAction.CycleAction;
            if (transportResponseWrapper.IsValidAll)
            {
                _skippingPeriodChecker.StartSkipping(); //Если все ответы валидны - запустим отсчет пропуска вызовов CycleTimeAction.
            }
            ResponseReadyRx.OnNext(transportResponseWrapper);
            await Task.Delay(1, ct); //TODO: Продумать как задвать скважность между выполнением цикл. функции на обмене.
        }


        /// <summary>
        /// Выставить на циклический обмен команду InfoEmergency.
        /// </summary>
        private async Task CycleCommandEmergencyActionAsync(CancellationToken ct)
        {
            var inData = new InDataWrapper<TIn> { Command = Command4Device.InfoEmergency };
            var transportResponseWrapper = await PieceOfDataSender(inData, ct);
            transportResponseWrapper.KeyExchange = KeyExchange;
            transportResponseWrapper.DataAction = DataAction.CycleAction;
            ResponseReadyRx.OnNext(transportResponseWrapper);
            await Task.Delay(1, ct); //TODO: Продумать как задвать скважность между выполнением цикл. функции на обмене.
        }
        #endregion




        public void Dispose()
        {
            _inputCycleDataEntryCheker?.Dispose();
            _skippingPeriodChecker?.Dispose();
        }
    }
}