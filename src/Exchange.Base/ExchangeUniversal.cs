using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using DAL.Abstract.Entities.Options.Exchange;
using DAL.Abstract.Entities.Options.Exchange.ProvidersOption;
using Exchange.Base.DataProviderAbstract;
using Exchange.Base.Model;
using Exchange.Base.RxModel;
using InputDataModel.Base;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using SerilogTimings.Extensions;
using Shared.Collections;
using Shared.Enums;
using Shared.Types;
using Transport.Base.Abstract;
using Transport.Base.RxModel;
using Worker.Background.Abstarct;


namespace Exchange.Base
{
    public class ExchangeUniversal<TIn> : IExchange<TIn>
    {
        #region field
        private const int MaxDataInQueue = 5;
        protected readonly ExchangeOption ExchangeOption;
        private readonly ITransport _transport;
        private readonly ITransportBackground _transportBackground;
        private readonly IExchangeDataProvider<TIn, ResponseDataItem<TIn>> _dataProvider;                               //проавйдер данных является StateFull, т.е. хранит свое последнее состояние между отправкой данных
        private readonly ILogger _logger;
        private readonly LimitConcurrentQueueWithoutDuplicate<InDataWrapper<TIn>> _oneTimeDataQueue = new LimitConcurrentQueueWithoutDuplicate<InDataWrapper<TIn>>(QueueOption.QueueNotExtractLastItem, MaxDataInQueue);   //Очередь данных для SendOneTimeData().
        private readonly LimitConcurrentQueueWithoutDuplicate<InDataWrapper<TIn>> _cycleTimeDataQueue; //Очередь данных для SendCycleTimeData().
        #endregion



        #region prop
        public string KeyExchange => ExchangeOption.Key;
        public bool AutoStartCycleFunc => ExchangeOption.AutoStartCycleFunc;
        public string ProviderName => ExchangeOption.Provider.Name;
        public int NumberErrorTrying => ExchangeOption.NumberErrorTrying;
        public int NumberTimeoutTrying => ExchangeOption.NumberTimeoutTrying;
        public KeyTransport KeyTransport => ExchangeOption.KeyTransport;
        public bool IsOpen => _transport.IsOpen;
        public bool IsCycleReopened => _transport.IsCycleReopened;
        public bool IsStartedTransportBg => _transportBackground.IsStarted;
        public bool IsStartedCycleFunc { get; private set; }

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

        public ProviderOption ProviderOptionRt
        {
            get => _dataProvider.GetCurrentOptionRt();
            set => _dataProvider.SetCurrentOptionRt(value);
        }
        #endregion



        #region ctor

        public ExchangeUniversal(ExchangeOption exchangeOption,
                                 ITransport transport,
                                 ITransportBackground transportBackground,
                                 IExchangeDataProvider<TIn,
                                 ResponseDataItem<TIn>> dataProvider,
                                 ILogger logger)
        {
            ExchangeOption = exchangeOption;
            _transport = transport;
            _transportBackground = transportBackground;
            _dataProvider = dataProvider;
            _logger = logger;

            _cycleTimeDataQueue = new LimitConcurrentQueueWithoutDuplicate<InDataWrapper<TIn>>(exchangeOption.CycleQueueOption, MaxDataInQueue);
        }

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

        #region ReOpen

        /// <summary>
        /// Циклическое открытие подключения
        /// </summary>
        private CancellationTokenSource _cycleReOpenedCts;
        public async Task CycleReOpened()
        {
            if (_transport != null)
            {
                if (_transport.IsCycleReopened)
                {
                    _logger.Error($"ТРАНСПОРТ УЖЕ НАХОДИТСЯ В ЦИКЛЕ ПЕРЕОТКРЫТИЯ \"{_transport.KeyTransport}\"  KeyExchange \"{KeyExchange}\"");
                    return;
                }
                _cycleReOpenedCts?.Cancel();
                _cycleReOpenedCts?.Dispose();
                _cycleReOpenedCts = new CancellationTokenSource();
                await Task.Factory.StartNew(async () =>
                {
                    await _transport.CycleReOpened();
                }, _cycleReOpenedCts.Token);
            }
        }

        /// <summary>
        /// Отмена задачи циклического открытия подключения
        /// </summary>
        public void CycleReOpenedCancelation()
        {
            if (!IsOpen)
            {
                _transport.CycleReOpenedCancelation();
                _cycleReOpenedCts?.Cancel();
            }
        }

        #endregion


        #region CycleExchange

        /// <summary>
        /// Добавление ЦИКЛ. функций на БГ
        /// </summary>
        public void StartCycleExchange()
        {
            _transportBackground.AddCycleAction(CycleTimeActionAsync);
            IsStartedCycleFunc = true;
        }

        /// <summary>
        /// Удаление ЦИКЛ. функций из БГ
        /// </summary>
        public void StopCycleExchange()
        {
            _transportBackground.RemoveCycleFunc(CycleTimeActionAsync);
            IsStartedCycleFunc = false;
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

            var dataWrapper = new InDataWrapper<TIn> { Datas = inData.ToList(), DirectHandlerName = directHandlerName };
            var result = _cycleTimeDataQueue.Enqueue(dataWrapper);
        }

        #endregion


        #region Actions

        /// <summary>
        /// Однократно вызываемая функция.
        /// </summary>
        protected async Task OneTimeActionAsync(CancellationToken ct)
        {
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
                    _logger.Error($"Ошибка извлечения данных из ЦИКЛ. очереди {errorResult.ToString()}");
                    return;
                }
            }
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
                    status = _transport.DataExchangeAsync(_dataProvider.TimeRespone, provider, ct).GetAwaiter().GetResult();
                    switch (status)
                    {
                        //ОБМЕН ЗАВЕРШЕН ПРАВИЛЬНО.
                        case StatusDataExchange.End:
                            IsConnect = true;
                            _countErrorTrying = 0;
                            _countTimeoutTrying = 0;
                            LastSendData = provider.InputData;
                            transportResp.ResponseData = provider.OutputData.ResponseData;
                            transportResp.Encoding = provider.OutputData.Encoding;
                            transportResp.IsOutDataValid = provider.OutputData.IsOutDataValid;
                            break;

                        //ТРАНСПОРТ НЕ ОТКРЫТ.
                        case StatusDataExchange.NotOpenTransport:
                            _logger.Error($"ПОПЫТКА ОТПРАВИТЬ ЗАПРОС НА НЕ ОТКРЫТЫЙ ТРАНСПОРТ. KeyExchange: {KeyExchange}  KeyTransport: {KeyTransport}");
                            IsConnect = false;
                            break;

                        //ТАЙМАУТ ОТВЕТА.
                        case StatusDataExchange.EndWithTimeout:
                            if (++_countTimeoutTrying > NumberTimeoutTrying)
                            {
                                _countTimeoutTrying = 0;
                                IsConnect = false;
                                _logger.Warning($"ТАЙМАУТ ОТВЕТА. KeyExchange {KeyExchange}");
                            }
                            break;

                        //ОБМЕН ЗАВЕРЩЕН КРИТИЧЕСКИ НЕ ПРАВИЛЬНО. ПЕРЕОТКРЫТИЕ СОЕДИНЕНИЯ.
                        case StatusDataExchange.EndWithTimeoutCritical:
                        case StatusDataExchange.EndWithErrorCritical:
                            CycleReOpened();
                            _logger.Error($"ОБМЕН ЗАВЕРЩЕН КРИТИЧЕСКИ НЕ ПРАВИЛЬНО. ПЕРЕОТКРЫТИЕ СОЕДИНЕНИЯ. KeyExchange {KeyExchange}");
                            break;

                        //ОБМЕН ЗАВЕРШЕН НЕ ПРАВИЛЬНО. Считаем попытки.
                        default:  //EndWithError
                            if (++_countErrorTrying > NumberErrorTrying)
                            {
                                _countErrorTrying = 0;
                                IsConnect = false;
                                _logger.Warning($"ОБМЕН ЗАВЕРШЕН НЕ ПРАВИЛЬНО. KeyExchange {KeyExchange}");
                                CycleReOpened();
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    //ОШИБКА ТРАНСПОРТА.
                    IsConnect = false;
                    transportResp.TransportException = ex;
                    _logger.Error(ex, $"ОШИБКА ТРАНСПОРТА. KeyExchange {KeyExchange}");
                }
                finally
                {
                    transportResp.RequestData = provider.InputData;
                    transportResp.Status = status;
                    transportResp.MessageDict = new Dictionary<string, string>(provider.StatusDict);
                    transportResponseWrapper.ResponsesItems.Add(transportResp);
                }
            });

            try
            {   //ЗАПУСК КОНВЕЕРА ПОДГОТОВКИ ДАННЫХ К ОБМЕНУ
                using (_logger.OperationAt(LogEventLevel.Information).Time("TimeOpertaion End Exchanage Pipeline"))
                {
                    await _dataProvider.StartExchangePipeline(inData);
                }
            }
            catch (Exception ex)
            {
                //ОШИБКА ПОДГОТОВКИ ДАННЫХ К ОБМЕНУ.
                IsConnect = false;
                transportResponseWrapper.ExceptionExchangePipline = ex;
                transportResponseWrapper.MessageDict = new Dictionary<string, string>(_dataProvider.StatusDict);
                _logger.Error(ex, $"ОШИБКА ПОДГОТОВКИ ДАННЫХ К ОБМЕНУ. KeyExchange {KeyExchange}");
            }
            finally
            {
                subscription.Dispose();
            }

            //ВЫВОД ОТЧЕТА ОБ ОТПРАВКИ ПОРЦИИ ДАННЫХ.
            LogedResponseInformation(transportResponseWrapper);
            return transportResponseWrapper;
        }


        private void LogedResponseInformation(ResponsePieceOfDataWrapper<TIn> response)
        {
            var numberPreparedPackages = response.ResponsesItems.Count;//кол-во подготовленных к отправке пакетов
            var countIsValid = response.ResponsesItems.Count(resp => resp.IsOutDataValid);
            var countAll = response.ResponsesItems.Count(resp => resp.IsOutDataValid);
            string errorStat = String.Empty;
            if (countIsValid < numberPreparedPackages)
            {
                errorStat = response.ResponsesItems.Select(r => r.Status.ToString()).Aggregate((i, j) => i + " | " + j);
            }

            var responseInfo = response.ResponsesItems.Select(item => new
            {
                viewRuleId = $"RuleName= {item.MessageDict["RuleName"]}  viewRule.Id= {item.MessageDict["viewRule.Id"]}",
                StatusStr = item.StatusStr,
                StringRequest = item.MessageDict["GetDataByte.StringRequest"],
                TimeResponse = item.MessageDict["TimeResponse"]
            }).ToList();

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,             //Отступы дочерних элементов 
                NullValueHandling = NullValueHandling.Ignore  //Игнорировать пустые теги
            };
            var jsonRespInfo = JsonConvert.SerializeObject(responseInfo, settings);

            _logger.Information($"ЗАПРОСЫ В ПАКЕТНОЙ ОТПРАВКИ. KeyExchange= {KeyExchange}   jsonRespInfo= {jsonRespInfo}");
            _logger.Information($"ОТВЕТ НА ПАКЕТНУЮ ОТПРАВКУ ПОЛУЧЕН . KeyExchange= {KeyExchange} успех/ответов/запросов=  ({countIsValid} / {countAll} / {numberPreparedPackages})   [{errorStat}]");
        }


        #endregion

        #endregion



        #region Disposable

        public void Dispose()
        {

        }

        #endregion
    }
}