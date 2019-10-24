using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Autofac.Features.OwnedInstances;
using Domain.Exchange.Behaviors;
using Domain.Exchange.Enums;
using Domain.Exchange.Repository.Entities;
using Domain.Exchange.RxModel;
using Domain.Exchange.Services;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.ProvidersOption;
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
    public class Exchange<TIn> : IExchange<TIn> where TIn : InputTypeBase
    {
        #region field
        private readonly ExchangeOption _option;
        private readonly ITransport _transport;
        private readonly ITransportBackground _transportBackground;
        private IDataProvider<TIn, ResponseInfo> _dataProvider;                       //провайдер данных является StateFull, т.е. хранит свое последнее состояние между отправкой данных
        private readonly IDisposable _dataProviderOwner;                              //управляет временем жизни _dataProvider
        private readonly ILogger _logger;
        private readonly Stopwatch _sw = Stopwatch.StartNew();
        private readonly List<IDisposable> _behaviorOwners;
        #endregion



        #region prop
        public string KeyExchange => _option.Key;
        public string ProviderName => _option.Provider.Name;
        public int NumberErrorTrying => _option.NumberErrorTrying;
        public int NumberTimeoutTrying => _option.NumberTimeoutTrying;
        public KeyTransport KeyTransport => _option.KeyTransport;

        public bool IsOpen => _transport.IsOpen;
        public bool IsCycleReopened => _transport.IsCycleReopened;
        public bool IsStartedTransportBg => _transportBackground.IsStarted;

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

        public CycleBehavior<TIn> CycleBehavior { get; }
        public OnceBehavior<TIn> OnceBehavior { get; }
        public CommandBehavior<TIn> CommandBehavior { get; }

        public ProviderOption GetProviderOption => _dataProvider.GetCurrentOption();
        #endregion



        #region ctor
        public Exchange(ExchangeOption option,
                                 ITransport transport,
                                 ITransportBackground transportBackground,
                                 IIndex<string, Func<ProviderOption, Owned<IDataProvider<TIn, ResponseInfo>>>> dataProviderFactory,
                                 ILogger logger,
                                 Func<string, ITransportBackground, CycleFuncOption, Func<InDataWrapper<TIn>, CancellationToken, Task<ResponsePieceOfDataWrapper<TIn>>>, Owned<CycleBehavior<TIn>>> cycleBehaviorFactory,
                                 Func<string, ITransportBackground, Func<InDataWrapper<TIn>, CancellationToken, Task<ResponsePieceOfDataWrapper<TIn>>>, Owned<OnceBehavior<TIn>>> onceBehaviorFactory,
                                 Func<string, ITransportBackground, Func<InDataWrapper<TIn>, CancellationToken, Task<ResponsePieceOfDataWrapper<TIn>>>, Owned<CommandBehavior<TIn>>> commandBehaviorFactory)
        {
            _option = option;
            _transport = transport;
            _transportBackground = transportBackground;
            var owner= dataProviderFactory[_option.Provider.Name](_option.Provider);
            _dataProviderOwner = owner;
            _dataProvider = owner.Value;
            _logger = logger;
            var cycleBehaviorOwner = cycleBehaviorFactory(KeyExchange, transportBackground, _option.CycleFuncOption, SendingPieceOfData);
            var onceBehaviorOwner = onceBehaviorFactory(KeyExchange, transportBackground, SendingPieceOfData);
            var commandBehaviorOwner = commandBehaviorFactory(KeyExchange, transportBackground, SendingPieceOfData);
            _behaviorOwners = new List<IDisposable> {cycleBehaviorOwner, onceBehaviorOwner, commandBehaviorOwner};
            CycleBehavior = cycleBehaviorOwner.Value;
            OnceBehavior = onceBehaviorOwner.Value;
            CommandBehavior = commandBehaviorOwner.Value;
        }
        #endregion



        #region ExchangeRx
        public ISubject<ConnectChangeRxModel> IsConnectChangeRx { get; } = new Subject<ConnectChangeRxModel>();
        public ISubject<LastSendDataChangeRxModel<TIn>> LastSendDataChangeRx { get; } = new Subject<LastSendDataChangeRxModel<TIn>>();
        #endregion



        #region TransportRx
        public ISubject<IsOpenChangeRxModel> IsOpenChangeTransportRx => _transport.IsOpenChangeRx;
        public ISubject<StatusDataExchangeChangeRxModel> StatusDataExchangeChangeTransportRx => _transport.StatusDataExchangeChangeRx;
        public ISubject<StatusStringChangeRxModel> StatusStringChangeTransportRx => _transport.StatusStringChangeRx;
        #endregion



        #region SendData
        /// <summary>
        /// Отправить команду. аналог однократно выставляемой функции.
        /// </summary>
        /// <param name="command"></param>
        public void SendCommand(Command4Device command)
        {
            if (!_transport.IsOpen)
                return;

            CommandBehavior.SendCommand(command);
        }


        /// <summary>
        /// Отправить данные для однократно выставляемой функции.
        /// Функция выставляется на БГ.
        /// </summary>
        public void SendOneTimeData(IEnumerable<TIn> inData, string directHandlerName)
        {
            if (!_transport.IsOpen)
                return;

            OnceBehavior.SendData(inData, directHandlerName);
        }


        /// <summary>
        /// Выставить данные для цикл. функции.
        /// </summary>
        public void SendCycleTimeData(IEnumerable<TIn> inData, string directHandlerName)
        {
            if (!_transport.IsOpen)
                return;

            CycleBehavior.SendData(inData, directHandlerName);
        }
        #endregion



        #region Actions
        /// <summary>
        /// Отправка порции данных.
        /// Провайдер подготавливает данные, транспорт осушетвляет обмен данными. 
        /// </summary>
        /// <returns>Ответ на отправку порции данных</returns>
        private int _countErrorTrying = 0;
        private int _countTimeoutTrying = 0;
        private async Task<ResponsePieceOfDataWrapper<TIn>> SendingPieceOfData(InDataWrapper<TIn> inData, CancellationToken ct)
        {
            var transportResponseWrapper = new ResponsePieceOfDataWrapper<TIn>();
            //ПОДПИСКА НА СОБЫТИЕ ОТПРАВКИ ПОРЦИИ ДАННЫХ
            var subscription = _dataProvider.RaiseSendDataRx.Subscribe(providerResult =>
            {
                var transportResp = new ResponseDataItem<TIn>();
                var status = StatusDataExchange.None;
                try
                {
                    status = _transport.DataExchangeAsync(providerResult, ct).GetAwaiter().GetResult();
                    switch (status)
                    {
                        //ОБМЕН ЗАВЕРШЕН ПРАВИЛЬНО.
                        case StatusDataExchange.End:
                            IsConnect = true;
                            _countErrorTrying = 0;
                            _countTimeoutTrying = 0;
                            LastSendData = providerResult.InputData;
                            transportResp.ResponseInfo = providerResult.OutputData;
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
                    transportResp.RequestData = providerResult.InputData;
                    transportResp.ResponseInfo = providerResult.OutputData;
                    transportResp.Status = status;
                    transportResp.MessageDict = new Dictionary<string, string>(providerResult.StatusDict);
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
        /// Логирование информации.
        /// </summary>
        private void LogedResponseInformation(ResponsePieceOfDataWrapper<TIn> response)
        {
            try
            {
                var numberPreparedPackages = response.ResponsesItems.Count;                                                                           //кол-во подготовленных к отправке пакетов        
                var countAll = response.ResponsesItems.Count(resp => resp.Status != StatusDataExchange.EndWithTimeout);                               //кол-во ВСЕХ полученных ответов
                var countIsValid = response.ResponsesItems.Count(resp => resp.ResponseInfo != null && resp.ResponseInfo.IsOutDataValid);              //кол-во ВАЛИДНЫХ ответов
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
            catch (Exception ex)
            {
                _logger.Error("{Type} {KeyExchange}  {Exception}", "ОШИБКА LogedResponseInformation", KeyExchange, ex);
            }
        }
        #endregion



        #region dataProvider
        public void SetNewProvider(IDataProvider<TIn, ResponseInfo> provider)
        {
            _dataProvider = provider;
        }
        #endregion

        

        #region Disposable
        public void Dispose()
        {
            _behaviorOwners.ForEach(beh=> beh.Dispose());
            _dataProviderOwner.Dispose();
        }
        #endregion
    }
}