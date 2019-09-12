﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using DAL.Abstract.Entities.Options.Exchange;
using DAL.Abstract.Entities.Options.Exchange.ProvidersOption;
using Domain.Exchange.DataProviderAbstract;
using Domain.Exchange.RxModel;
using Domain.Exchange.Services;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.Response;
using Infrastructure.Background.Abstarct;
using Infrastructure.Transport.Base.Abstract;
using Infrastructure.Transport.Base.RxModel;
using Newtonsoft.Json;
using Serilog;
using Shared.Collections;
using Shared.Enums;
using Shared.Types;

namespace Domain.Exchange
{
    public class Exchange<TIn> : IExchange<TIn>
    {
        #region field
        private const int MaxDataInQueue = 5;
        protected readonly ExchangeOption ExchangeOption;
        private readonly ITransport _transport;
        private readonly ITransportBackground _transportBackground;
        private readonly IExchangeDataProvider<TIn, ResponseInfo> _dataProvider;         //провайдер данных является StateFull, т.е. хранит свое последнее состояние между отправкой данных
        private readonly ILogger _logger;
        private readonly LimitConcurrentQueueWithoutDuplicate<InDataWrapper<TIn>> _oneTimeDataQueue = new LimitConcurrentQueueWithoutDuplicate<InDataWrapper<TIn>>(QueueMode.QueueExtractLastItem, MaxDataInQueue);   //Очередь данных для SendOneTimeData().
        private readonly LimitConcurrentQueueWithoutDuplicate<InDataWrapper<TIn>> _cycleTimeDataQueue; //Очередь данных для SendCycleTimeData().
        private readonly InputCycleDataEntryCheker _inputCycleDataEntryCheker;      //таймер отсчитывает период от получения входных данных для цикл. обмена.
        private readonly SkippingPeriodChecker _skippingPeriodChecker;              //таймер отсчитывает время пропуска периода опроса.

        private readonly Stopwatch _sw = Stopwatch.StartNew();
        #endregion



        #region prop
        public string KeyExchange => ExchangeOption.Key;
        public bool AutoStartCycleFunc => ExchangeOption.CycleFuncOption.AutoStartCycleFunc;
        public string ProviderName => ExchangeOption.Provider.Name;
        public int NumberErrorTrying => ExchangeOption.NumberErrorTrying;
        public int NumberTimeoutTrying => ExchangeOption.NumberTimeoutTrying;
        public KeyTransport KeyTransport => ExchangeOption.KeyTransport;
        public bool IsOpen => _transport.IsOpen;
        public bool IsCycleReopened => _transport.IsCycleReopened;
        public bool IsStartedTransportBg => _transportBackground.IsStarted;
        public CycleExchnageStatus CycleExchnageStatus { get; private set; }

        private bool _isConnect;
        public bool IsConnect
        {
            get => _isConnect;
            private set
            {
                if (value == _isConnect) return;
                _isConnect = value;
                IsConnectChangeRx.OnNext(new ConnectChangeRxModel { IsConnect = _isConnect, KeyExchange = KeyExchange });
            }
        }

        private InDataWrapper<TIn> _lastSendData;
        public InDataWrapper<TIn> LastSendData
        {
            get => _lastSendData;
            private set
            {
                if (value == _lastSendData) return;
                _lastSendData = value;
                LastSendDataChangeRx.OnNext(new LastSendDataChangeRxModel<TIn> { KeyExchange = KeyExchange, LastSendData = LastSendData });
            }
        }

        public bool IsFullOneTimeDataQueue => _oneTimeDataQueue.IsFullLimit;
        public bool IsFullCycleTimeDataQueue => _cycleTimeDataQueue.IsFullLimit;
        public bool IsNormalFrequencyCycleDataEntry { get; private set; } = true;

        public ProviderOption ProviderOptionRt
        {
            get => _dataProvider.GetCurrentOptionRt();
            set => _dataProvider.SetCurrentOptionRt(value);
        }
        #endregion



        #region ctor

        public Exchange(ExchangeOption exchangeOption,
                                 ITransport transport,
                                 ITransportBackground transportBackground,
                                 IExchangeDataProvider<TIn, ResponseInfo> dataProvider,
                                 ILogger logger)
        {
            ExchangeOption = exchangeOption;
            _transport = transport;
            _transportBackground = transportBackground;
            _dataProvider = dataProvider;
            _logger = logger;
            _cycleTimeDataQueue = new LimitConcurrentQueueWithoutDuplicate<InDataWrapper<TIn>>(ExchangeOption.CycleFuncOption.CycleQueueMode, MaxDataInQueue);
            _inputCycleDataEntryCheker= new InputCycleDataEntryCheker(KeyExchange, ExchangeOption.CycleFuncOption.NormalIntervalCycleDataEntry);
            _skippingPeriodChecker= new SkippingPeriodChecker(ExchangeOption.CycleFuncOption.SkipInterval);
        }

        #endregion



        #region EventHandler
        #endregion



        #region InputDataChangeRx
        public ISubject<InputDataStateRxModel> CycleDataEntryStateChangeRx =>_inputCycleDataEntryCheker.CycleDataEntryStateChangeRx;
        #endregion


        #region ExchangeRx
        public ISubject<ConnectChangeRxModel> IsConnectChangeRx { get; } = new Subject<ConnectChangeRxModel>();
        public ISubject<LastSendDataChangeRxModel<TIn>> LastSendDataChangeRx { get; } = new Subject<LastSendDataChangeRxModel<TIn>>();
        public ISubject<ResponsePieceOfDataWrapper<TIn>> ResponseChangeRx { get; } = new Subject<ResponsePieceOfDataWrapper<TIn>>();
        #endregion


        #region TransportRx
        public ISubject<IsOpenChangeRxModel> IsOpenChangeTransportRx => _transport.IsOpenChangeRx;
        public ISubject<StatusDataExchangeChangeRxModel> StatusDataExchangeChangeTransportRx => _transport.StatusDataExchangeChangeRx;
        public ISubject<StatusStringChangeRxModel> StatusStringChangeTransportRx => _transport.StatusStringChangeRx;
        #endregion



        #region Methode

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
            _transportBackground.RemoveCycleFunc(CycleTimeActionAsync);
            _transportBackground.RemoveCycleFunc(CycleCommandEmergencyActionAsync);
            CycleExchnageStatus = CycleExchnageStatus.Off;
            _inputCycleDataEntryCheker.StopChecking();
        }

        /// <summary>
        /// перевести в режим НОРМАЛЬНЫЙ цикл. обмен на БГ
        /// </summary>
        public void Switch2NormalCycleExchange()
        {
            _transportBackground.RemoveCycleFunc(CycleCommandEmergencyActionAsync);
            _transportBackground.AddCycleAction(CycleTimeActionAsync);
            CycleExchnageStatus = CycleExchnageStatus.Normal;
        }

        /// <summary>
        /// перевести в режим АВАРИЙНЫЙ цикл. обмен на БГ
        /// </summary>
        public void Switch2CycleCommandEmergency()
        {
            _transportBackground.RemoveCycleFunc(CycleTimeActionAsync);
            _transportBackground.AddCycleAction(CycleCommandEmergencyActionAsync);
            CycleExchnageStatus = CycleExchnageStatus.Emergency;
        }

        #endregion



        #region SendData

        /// <summary>
        /// Отправить команду. аналог однократно выставляемой функции.
        /// </summary>
        /// <param name="command"></param>
        public void SendCommand(Command4Device command)
        {
            if (command == Command4Device.None)
                return;

            if (!_transport.IsOpen)
                return;

            var dataWrapper = new InDataWrapper<TIn> { Command = command };
            _oneTimeDataQueue.Enqueue(dataWrapper);
            _transportBackground.AddOneTimeAction(OneTimeActionAsync);
        }


        /// <summary>
        /// Отправить данные для однократно выставляемой функции.
        /// Функция выставляется на БГ.
        /// </summary>
        public void SendOneTimeData(IEnumerable<TIn> inData, string directHandlerName)
        {
            if (inData == null)
                return;

            if (!_transport.IsOpen)
                return;

            var dataWrapper = new InDataWrapper<TIn> { Datas = inData.ToList(), DirectHandlerName = directHandlerName };
            var result = _oneTimeDataQueue.Enqueue(dataWrapper);
            if (result.IsSuccess)
            {
                _transportBackground.AddOneTimeAction(OneTimeActionAsync);
            }
            else
            {
                //_logger.Debug($"SendOneTimeData in Queue Error: {result.Error}");
            }
        }


        /// <summary>
        /// Выставить данные для цикл. функции.
        /// </summary>
        public void SendCycleTimeData(IEnumerable<TIn> inData, string directHandlerName)
        {
            if (inData == null)
                return;

            if (!_transport.IsOpen)
                return;

            _inputCycleDataEntryCheker.InputDataEntry();

            var dataWrapper = new InDataWrapper<TIn> { Datas = inData.ToList(), DirectHandlerName = directHandlerName };
            var result = _cycleTimeDataQueue.Enqueue(dataWrapper);
            if (result.IsSuccess) //Добавленны НОВЫЕ данные в очередь.
            {
                _skippingPeriodChecker.StopSkipping();
            }
        }

        #endregion


        #region Actions

        /// <summary>
        /// Однократно вызываемая функция.
        /// </summary>
        protected async Task OneTimeActionAsync(CancellationToken ct)
        {
            if (!_transport.IsOpen)
                return;

            var result = _oneTimeDataQueue.Dequeue();
            if (result.IsSuccess)
            {
                var inData = result.Value;
                var transportResponseWrapper = await SendingPieceOfData(inData, ct);
                transportResponseWrapper.KeyExchange = KeyExchange;
                transportResponseWrapper.DataAction = (inData.Command == Command4Device.None) ? DataAction.OneTimeAction : DataAction.CommandAction;
                ResponseChangeRx.OnNext(transportResponseWrapper);
            }
        }


        /// <summary>
        /// Обработка отправки цикл. даных.
        /// Если транспорт НЕ открыт, данные не отправляются
        /// Если очередь ПУСТА, отправляется NULL.
        /// Если ошибка извлечения из очереди, данные не отправляются
        /// </summary>
        protected async Task CycleTimeActionAsync(CancellationToken ct)
        {
            //TODO: IsOpen на транспорте заменить на StatusConnect(Open, Reconnect, StopedReconnect)
            //if (!_transport.StatusConnecttus != Open)
            //{
            //    _logger.Warning($"Exchange/CycleTimeActionAsync Попытка отправить данные на не открытый тарнспорт {_transport.Status}");
            //    return;
            //}

            if (_skippingPeriodChecker.IsSkip)
            {
                Debug.WriteLine("CycleTimeActionAsync SKIP ----------------------------------------");
                return;
            }

            if (!_transport.IsOpen) 
                return;

            InDataWrapper<TIn> inData = null;
            var result = _cycleTimeDataQueue.Dequeue();
            if (result.IsSuccess)
            {
                inData = result.Value;
            }
            else
            {
                var errorResult = result.Error.DequeueResultError;
                if (errorResult == DequeueResultError.FailTryDequeue || errorResult == DequeueResultError.FailTryPeek)
                {
                    _logger.Error("{Type} {KeyExchange}  {MessageShort}", "Ошибка извлечения данных из ЦИКЛ. очереди", KeyExchange, errorResult.ToString());
                    return;
                }
            }
            var transportResponseWrapper = await SendingPieceOfData(inData, ct);
            transportResponseWrapper.KeyExchange = KeyExchange;
            transportResponseWrapper.DataAction = DataAction.CycleAction;
            if (transportResponseWrapper.IsValidAll)
            {
                _skippingPeriodChecker.StartSkipping(); //Если все ответы валидны - запустим отсчет пропуска вызовов CycleTimeAction.
            }

            ResponseChangeRx.OnNext(transportResponseWrapper);
            await Task.Delay(1, ct); //TODO: Продумать как задвать скважность между выполнением цикл. функции на обмене.
        }



        /// <summary>
        /// </summary>
        protected async Task CycleCommandEmergencyActionAsync(CancellationToken ct)
        {
            if (!_transport.IsOpen)
                return;

            InDataWrapper<TIn> inData = new InDataWrapper<TIn> {Command = Command4Device.InfoEmergency};        
            var transportResponseWrapper = await SendingPieceOfData(inData, ct);
            transportResponseWrapper.KeyExchange = KeyExchange;
            transportResponseWrapper.DataAction = DataAction.CycleAction;
            ResponseChangeRx.OnNext(transportResponseWrapper);

            await Task.Delay(100, ct); //TODO: Продумать как задвать скважность между выполнением цикл. функции на обмене.
        }



        /// <summary>
        /// Отправка порции данных.
        /// </summary>
        /// <returns>Ответ на отправку порции данных</returns>
        private int _countErrorTrying = 0;
        private int _countTimeoutTrying = 0;
        private async Task<ResponsePieceOfDataWrapper<TIn>> SendingPieceOfData(InDataWrapper<TIn> inData, CancellationToken ct)
        {
            var transportResponseWrapper = new ResponsePieceOfDataWrapper<TIn>();
            //ПОДПИСКА НА СОБЫТИЕ ОТПРАВКИ ПОРЦИИ ДАННЫХ
            var subscription = _dataProvider.RaiseSendDataRx.Subscribe(provider =>
            {
                var transportResp = new ResponseDataItem<TIn>();
                var status = StatusDataExchange.None;
                try
                {
                    status = _transport.DataExchangeAsync(provider.TimeRespone, provider, ct).GetAwaiter().GetResult();
                    switch (status)
                    {
                        //ОБМЕН ЗАВЕРШЕН ПРАВИЛЬНО.
                        case StatusDataExchange.End:
                            IsConnect = true;
                            _countErrorTrying = 0;
                            _countTimeoutTrying = 0;
                            LastSendData = provider.InputData;
                            transportResp.ResponseInfo = provider.OutputData;
                            break;

                        //ТРАНСПОРТ НЕ ОТКРЫТ.
                        case StatusDataExchange.NotOpenTransport:
                            IsConnect = false;
                            _logger.Warning("{Type}  KeyExchange:{KeyExchange}   KeyTransport:{KeyTransport} ", "ПОПЫТКА ОТПРАВИТЬ ЗАПРОС НА НЕ ОТКРЫТЫЙ ТРАНСПОРТ. IsConnect = false;", KeyExchange, KeyTransport);
                            break;

                        //ТАЙМАУТ ОТВЕТА.
                        case StatusDataExchange.EndWithTimeout:
                            if (++_countTimeoutTrying > NumberTimeoutTrying)
                            {
                                _countTimeoutTrying = 0;
                                IsConnect = false;
                                _logger.Warning("{Type} {KeyExchange}", "МАКСИМАЛЬНОЕ КОЛ-ВО ТАЙМАУТОВ ОТВЕТА ПРЕВЫШЕННО. IsConnect = false", KeyExchange);
                            }
                            break;

                        //ОБМЕН ЗАВЕРЩЕН КРИТИЧЕСКИ НЕ ПРАВИЛЬНО. ПЕРЕОТКРЫТИЕ СОЕДИНЕНИЯ.
                        case StatusDataExchange.EndWithTimeoutCritical:
                        case StatusDataExchange.EndWithErrorCritical:
                            IsConnect = false;
                            _transport.CycleReOpenedExec(); //TODO: заменить на IncReopenCount
                            _logger.Error("{Type} {KeyExchange} {ErrorStatus}", "КРИТИЧЕСКАЯ ОШИБКА. ПЕРЕОТКРЫТИЕ СОЕДИНЕНИЯ. IsConnect = false;", KeyExchange, status);
                            break;

                        //ОБМЕН ЗАВЕРЩЕН С ОШИБКАМИ.
                        case StatusDataExchange.EndWithErrorCannotSendData:
                        case StatusDataExchange.EndWithErrorWrongAnswers:
                            if (++_countErrorTrying > NumberErrorTrying)
                            {
                                _countErrorTrying = 0;
                                IsConnect = false;
                                _logger.Warning("{Type} {KeyExchange} {ErrorStatus}", "МАКСИМАЛЬНОЕ КОЛ-ВО НАКОПЛЕННЫХ ОШИБОК ПРЕВЫЩЕННО. ПЕРЕОТКРЫТИЕ СОЕДИНЕНИЯ. IsConnect = false;", KeyExchange, status);
                                _transport.CycleReOpenedExec();//TODO: заменить на IncReopenCount
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    //ОШИБКА ТРАНСПОРТА.
                    IsConnect = false;
                    transportResp.TransportException = ex;
                    _logger.Error("{Type} {KeyExchange} KeyTransport:{KeyTransport}", "ОШИБКА ТРАНСПОРТА.", KeyExchange, KeyTransport);
                }
                finally
                {
                    transportResp.RequestData = provider.InputData;
                    transportResp.ResponseInfo = provider.OutputData;
                    transportResp.Status = status;
                    transportResp.MessageDict = new Dictionary<string, string>(provider.StatusDict);
                    transportResponseWrapper.ResponsesItems.Add(transportResp);
                }
            });

            try
            {   //ЗАПУСК КОНВЕЕРА ПОДГОТОВКИ ДАННЫХ К ОБМЕНУ
                _sw.Restart();
                await _dataProvider.StartExchangePipeline(inData);
                _sw.Stop();
                transportResponseWrapper.TimeAction= _sw.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                //ОШИБКА ПОДГОТОВКИ ДАННЫХ К ОБМЕНУ.
                IsConnect = false;
                transportResponseWrapper.ExceptionExchangePipline = ex;
                transportResponseWrapper.MessageDict = new Dictionary<string, string>(_dataProvider.StatusDict);
                _logger.Warning("{Type} {KeyExchange}", "ОШИБКА ПОДГОТОВКИ ДАННЫХ К ОБМЕНУ. (смотерть ExceptionExchangePipline)", KeyExchange);
            }
            finally
            {
                subscription.Dispose();
            }

            //ВЫВОД ОТЧЕТА ОБ ОТПРАВКИ ПОРЦИИ ДАННЫХ.
            LogedResponseInformation(transportResponseWrapper);
            return transportResponseWrapper;
        }


        /// <summary>
        /// 
        /// </summary>
        private void LogedResponseInformation(ResponsePieceOfDataWrapper<TIn> response)
        {
            var numberPreparedPackages = response.ResponsesItems.Count;                                              //кол-во подготовленных к отправке пакетов        
            var countAll = response.ResponsesItems.Count(resp => resp.Status != StatusDataExchange.EndWithTimeout);  //кол-во ВСЕХ полученных ответов
            var countIsValid = response.ResponsesItems.Count(resp => resp.ResponseInfo.IsOutDataValid);              //кол-во ВАЛИДНЫХ ответов
            string errorStat = string.Empty;
            response.IsValidAll = true;
            if (countIsValid < numberPreparedPackages)
            {
                errorStat = response.ResponsesItems.Select(r => r.Status.ToString()).Aggregate((i, j) => i + " | " + j);
                response.IsValidAll = false;
            }

            var responseInfo = response.ResponsesItems.Select(item => new
            {
                Rule = $"RuleName= {item.MessageDict["RuleName"]}  viewRule.Id= {item.MessageDict["viewRule.Id"]}",
                StatusStr = item.StatusStr,
                Request = item.MessageDict["GetDataByte.Request"],
                RequestBase = item.MessageDict["GetDataByte.RequestBase"],
                StringResponseRef = item.MessageDict.ContainsKey("SetDataByte.StringResponse") ? item.MessageDict["SetDataByte.StringResponse"] : null,
                TimeResponse = item.MessageDict["TimeResponse"],
                StronglyTypedResponse = item.MessageDict.ContainsKey("SetDataByte.StronglyTypedResponse") ? item.MessageDict["SetDataByte.StronglyTypedResponse"] : null,
            }).ToList();

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,             //Отступы дочерних элементов 
                NullValueHandling = NullValueHandling.Ignore  //Игнорировать пустые теги
            };
            var jsonRespInfo = JsonConvert.SerializeObject(responseInfo, settings);
            var countStat = $"успех/ответов/запросов= ({countIsValid} / {countAll} / {numberPreparedPackages})";
            var timeAction = response.TimeAction;
            var isValidAll = response.IsValidAll;
            _logger.Information("{Type} ({TimeAction} мс.) {KeyExchange}  РЕЗУЛЬТАТ= {isValid}  {countStat}  [{errorStat}] jsonRespInfo= {jsonRespInfo}",
                                "ОТВЕТ НА ПАКЕТНУЮ ОТПРАВКУ ПОЛУЧЕН.", timeAction, KeyExchange, isValidAll, countStat, errorStat, jsonRespInfo);
        }

        #endregion

        #endregion



        #region Disposable

        public void Dispose()
        {
            _inputCycleDataEntryCheker.Dispose();
            _skippingPeriodChecker.Dispose();
        }

        #endregion
    }

    public enum CycleExchnageStatus{Off, Normal, Emergency}
}