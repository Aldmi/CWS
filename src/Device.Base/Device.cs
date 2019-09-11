using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DAL.Abstract.Entities.Options.Device;
using DAL.Abstract.Entities.Options.MiddleWare;
using Domain.Device.MiddleWares;
using Domain.Device.MiddleWares.Invokes;
using Domain.Device.Produser;
using Domain.Exchange;
using Domain.Exchange.RxModel;
using Domain.Exchange.Services;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.Response;
using Infrastructure.EventBus.Abstract;
using Infrastructure.Transport.Base.RxModel;
using Newtonsoft.Json;
using Serilog;

namespace Domain.Device
{
    /// <summary>
    /// Устройство.
    /// Содержит список обменов.
    /// Ответы о своей работе каждое ус-во выставляет самостоятельно на шину данных
    /// </summary>
    public class Device<TIn> : IDisposable where TIn : InputTypeBase
    {
        #region field

        private readonly IEventBus _eventBus; //TODO: Not Use
        private readonly ProduserUnionStorage<TIn> _produserUnionStorage;
        private readonly ILogger _logger;
        private readonly List<IDisposable> _disposeExchangesEventHandlers = new List<IDisposable>();
        private readonly List<IDisposable> _disposeExchangesCycleDataEntryStateEventHandlers = new List<IDisposable>();
        private IDisposable _disposeMiddlewareInvokeServiceInvokeIsCompleteEventHandler;

        #endregion



        #region prop

        public DeviceOption Option { get;  }
        public List<IExchange<TIn>> Exchanges { get; }
        public MiddlewareInvokeService<TIn> MiddlewareInvokeService { get; private set; }
        public string ProduserUnionKey => Option.ProduserUnionKey;

        #endregion




        #region ctor

        public Device(DeviceOption option,
                      IEnumerable<IExchange<TIn>> exchanges,
                      IEventBus eventBus,
                      ProduserUnionStorage<TIn> produserUnionStorage,
                      ILogger logger)
        {
            Option = option;
            Exchanges = exchanges.ToList();
            _eventBus = eventBus;
            _produserUnionStorage = produserUnionStorage;
            _logger = logger;

            CreateMiddleWareInDataByOption();
        }

        #endregion




        #region Methode

        public MiddleWareInDataOption GetMiddleWareInDataOption()
        {
            return Option.MiddleWareInData;
        }


        public void SetMiddleWareInDataOptionAndCreateNewMiddleWareInData(MiddleWareInDataOption option)
        {
            Option.MiddleWareInData = option;
            CreateMiddleWareInDataByOption();
        }


        private void CreateMiddleWareInDataByOption()
        {
            _disposeMiddlewareInvokeServiceInvokeIsCompleteEventHandler?.Dispose();
            MiddlewareInvokeService?.Dispose();
            MiddlewareInvokeService = null;
            if (Option.MiddleWareInData != null)
            {
                var middleWareInData = new MiddleWareInData<TIn>(Option.MiddleWareInData, _logger);
                MiddlewareInvokeService = new MiddlewareInvokeService<TIn>(Option.MiddleWareInData.InvokerOutput, middleWareInData, _logger);
                _disposeMiddlewareInvokeServiceInvokeIsCompleteEventHandler= MiddlewareInvokeService?.InvokeIsCompleteRx.Subscribe(MiddlewareInvokeIsCompleteRxEventHandler);
            }
        }



        /// <summary>
        /// Подписка на публикацию событий устройства на ВНЕШНЮЮ ШИНУ ДАННЫХ
        /// </summary>
        /// <param name="produserUnionKey">Имя топика, если == null, то берется из настроек</param>
        public bool SubscrubeOnExchangesEvents(string produserUnionKey = null)
        {
            //Топик не указан
            if (string.IsNullOrEmpty(ProduserUnionKey))
                return false;

            Exchanges.ForEach(exch =>
            {
                _disposeExchangesEventHandlers.Add(exch.IsConnectChangeRx.Subscribe(ConnectChangeRxEventHandler));
                //_disposeExchangesEventHandlers.Add(exch.LastSendDataChangeRx.Subscribe(LastSendDataChangeRxEventHandler));
                _disposeExchangesEventHandlers.Add(exch.IsOpenChangeTransportRx.Subscribe(OpenChangeTransportRxEventHandler));
                _disposeExchangesEventHandlers.Add(exch.ResponseChangeRx.Subscribe(ResponseChangeRxEventHandler));
            });
            return true;
        }

 
        public void UnsubscrubeOnExchangesEvents()
        {
            _disposeExchangesEventHandlers.ForEach(d=>d.Dispose());
        }


        public bool SubscrubeOnExchangesCycleDataEntryStateEvents()
        {
            Exchanges.ForEach(exch =>
            {
                _disposeExchangesCycleDataEntryStateEventHandlers.Add(exch.CycleDataEntryStateChangeRx.Subscribe(CycleDataEntryStateChangeRxEventHandler));
            });
            return true;
        }


        public void UnsubscrubeOnExchangesCycleDataEntryStateEvents()
        {
            _disposeExchangesCycleDataEntryStateEventHandlers.ForEach(d => d.Dispose());
        }


        /// <summary>
        /// Принять данные для УСТРОЙСТВА.
        /// Данные будут переданны напрямую на обмены или в конвеер MiddleWare 
        /// </summary>
        /// <param name="inData">входные данные в обертке</param>
        public async Task Resive(InputData<TIn> inData)
        {
            if (MiddlewareInvokeService != null)
            {
                switch (inData.DataAction)
                {
                    case DataAction.OneTimeAction://однократные данные обрабатываем сразу.
                        await MiddlewareInvokeService.InputSetInstantly(inData);
                        break;
                    case DataAction.CycleAction: //однократные данные обрабатываем сразу.
                         await MiddlewareInvokeService.InputSetByOptionMode(inData);
                        break;
                    case DataAction.CommandAction://Команды не проходят обработку через MiddlewareInvokeService
                        await ResiveInExchange(inData);
                        break;
                }
            }
            else
            {
               await ResiveInExchange(inData);
            }
        }


        private async Task ResiveInExchange(InputData<TIn> inData)
        {
            if (string.IsNullOrEmpty(inData.ExchangeName))
            {
                await Send2AllExchanges(inData.DataAction, inData.Data, inData.Command);
            }
            else
            {
                await Send2ConcreteExchanges(inData.ExchangeName, inData.DataAction, inData.Data, inData.Command, inData.DirectHandlerName);
            }
        }


        private async void MiddlewareInvokeIsCompleteRxEventHandler(Result<InputData<TIn>, ErrorResultMiddleWareInData> result)
        {
            if (result.IsSuccess)
            {
                var inData = result.Value; 
                await ResiveInExchange(inData);
                _logger.Information($"Данные УСПЕШНО подготовленны MiddlewareInData для устройства: {Option.Name}");
            }
            else
            {
                _logger.Error($"ОШИБКИ ПРЕОБРАЗОВАНИЯ ВХОДНЫХ ДАННЫХ MiddlewareInData ДЛЯ: {Option.Name} Errors= {result.Error.GetErrorsArgegator}"); //ВСЕ ОШИБКИ ПРЕОБРАЗОВАНИЯ ВХОДНЫХ ДАННЫХ.
            }
        }



        /// <summary>
        /// Отправить данные или команду на все обмены.
        /// </summary>
        /// <param name="dataAction"></param>
        /// <param name="inData"></param>
        /// <param name="command4Device"></param>
        public async Task Send2AllExchanges(DataAction dataAction, List<TIn> inData, Command4Device command4Device = Command4Device.None)
        {
            var tasks= new List<Task>();
            foreach (var exchange in Exchanges)
            {
                tasks.Add(SendDataOrCommand(exchange, dataAction, inData, command4Device));
            }
            await Task.WhenAll(tasks);
        }


        /// <summary>
        /// Отправить данные или команду на выбранный обмен.
        /// </summary>
        /// <param name="keyExchange"></param>
        /// <param name="dataAction"></param>
        /// <param name="inData"></param>
        /// <param name="command4Device"></param>
        public async Task Send2ConcreteExchanges(string keyExchange, DataAction dataAction, List<TIn> inData, Command4Device command4Device = Command4Device.None, string directHandlerName = null)
        {
            var exchange = Exchanges.FirstOrDefault(exch=> exch.KeyExchange == keyExchange);
            if (exchange == null)
            {
                //await Send2Produder(Option.TopicName4MessageBroker, $"Обмен не найденн для этого ус-ва {keyExchange}");
                return;
            }
            await SendDataOrCommand(exchange, dataAction, inData, command4Device, directHandlerName);  
        }


        /// <summary>
        /// Отправить данные или команду.
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="dataAction"></param>
        /// <param name="inData"></param>
        /// <param name="command4Device"></param>
        private async Task SendDataOrCommand(IExchange<TIn> exchange, DataAction dataAction, List<TIn> inData, Command4Device command4Device = Command4Device.None, string directHandlerName = null)
        {
            if (!exchange.IsStartedTransportBg)
            {
                _logger.Warning($"Отправка данных НЕ удачна, Бекграунд обмена {exchange.KeyExchange} НЕ ЗАПЦУЩЕН");
                //await Send2Produder(Option.TopicName4MessageBroker, $"Отправка данных НЕ удачна, Бекграунд обмена {exchange.KeyExchange} НЕ ЗАПЦУЩЕН");
                return;
            }
            if (!exchange.IsOpen)
            {
                _logger.Warning($"Отправка данных НЕ удачна, соединение транспорта для обмена {exchange.KeyExchange} НЕ ОТКРЫТО");
                //await Send2Produder(Option.TopicName4MessageBroker, $"Отправка данных НЕ удачна, соединение транспорта для обмена {exchange.KeyExchange} НЕ ОТКРЫТО");
                return;
            }
            switch (dataAction)
            {
                case DataAction.OneTimeAction:
                    if (exchange.IsFullOneTimeDataQueue)
                    {
                        _logger.Error($"Отправка данных НЕ удачна, очередь однократных данных ПЕРЕПОЛНЕННА для обмена: {exchange.KeyExchange}");
                        //await Send2Produder(Option.TopicName4MessageBroker, $"Отправка данных НЕ удачна, Цикл. обмен для обмена {exchange.KeyExchange} НЕ ЗАПУЩЕН");
                        return;
                    }
                    exchange.SendOneTimeData(inData, directHandlerName);
                    break;

                case DataAction.CycleAction:
                    if (exchange.CycleExchnageStatus == CycleExchnageStatus.Off)
                    {
                        _logger.Warning($"Отправка данных НЕ удачна, Цикл. обмен для обмена {exchange.KeyExchange} НЕ ЗАПУЩЕН");
                        //await Send2Produder(Option.TopicName4MessageBroker, $"Отправка данных НЕ удачна, Цикл. обмен для обмена {exchange.KeyExchange} НЕ ЗАПУЩЕН");
                        return;
                    }
                    if (exchange.IsFullCycleTimeDataQueue)
                    {
                        _logger.Error($"Отправка данных НЕ удачна, очередь цикличеких данных ПЕРЕПОЛНЕННА для обмена: {exchange.KeyExchange}");
                        //await Send2Produder(Option.TopicName4MessageBroker, $"Отправка данных НЕ удачна, Цикл. обмен для обмена {exchange.KeyExchange} НЕ ЗАПУЩЕН");
                        return;
                    }
                    exchange.SendCycleTimeData(inData, directHandlerName);
                    break;

                case DataAction.CommandAction:
                    if (exchange.IsFullOneTimeDataQueue)
                    {
                        _logger.Error($"Отправка команды НЕ удачна, очередь однократных данных ПЕРЕПОЛНЕННА для обмена: {exchange.KeyExchange}");
                        //await Send2Produder(Option.TopicName4MessageBroker, $"Отправка данных НЕ удачна, Цикл. обмен для обмена {exchange.KeyExchange} НЕ ЗАПУЩЕН");
                        return;
                    }
                    exchange.SendCommand(command4Device);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(dataAction), dataAction, null);
            }
        }


        private async Task Send2ProduderUnion(ResponsePieceOfDataWrapper<TIn> response)
        {
            var produser = _produserUnionStorage.Get(ProduserUnionKey);
            if (produser == null)
            {
                _logger.Error($"Продюссер по ключу {ProduserUnionKey} НЕ НАЙДЕНН для Устройства= {Option.Name}");
                return;
            }

            var results= await produser.SendAll(response);
            foreach (var (isSuccess, isFailure, _, error) in results)
            {
                if (isFailure)
                    _logger.Error($"Ошибки отправки ответов для Устройства= {Option.Name} через ProduderUnion = {ProduserUnionKey}  {error}");

                if(isSuccess)
                    _logger.Information($"ОТПРАВКА ОТВЕТОВ УСПЕШНА для устройства {Option.Name} через ProduderUnion = {ProduserUnionKey}");
            }
        }

        #endregion




        #region RxEventHandler 

        private async void ConnectChangeRxEventHandler(ConnectChangeRxModel model)
        {
            //await Send2Produder(Option.TopicName4MessageBroker, $"Connect = {model.IsConnect} для обмена {model.KeyExchange}");
            _logger.Debug($"ConnectChangeRxEventHandler.  Connect = {model.IsConnect} для обмена {model.KeyExchange}");
        }


        private async void OpenChangeTransportRxEventHandler(IsOpenChangeRxModel model)
        {
            //await Send2Produder(Option.TopicName4MessageBroker,$"IsOpen = {model.IsOpen} для ТРАНСПОРТА {model.TransportName}");
            //_eventBus.Publish(isOpenChangeRxModel);  //Публикуем событие на общую шину данных  
            _logger.Debug($"OpenChangeTransportRxEventHandler.  IsOpen = {model.IsOpen} для ТРАНСПОРТА {model.TransportName}");
        }

        private async void ResponseChangeRxEventHandler(ResponsePieceOfDataWrapper<TIn> responsePieceOfDataWrapper)
        {
            await Send2ProduderUnion(responsePieceOfDataWrapper);
            //логирование ответов в полном виде
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,             //Отступы дочерних элементов 
                NullValueHandling = NullValueHandling.Ignore  //Игнорировать пустые теги
            };
            var jsonResp = JsonConvert.SerializeObject(responsePieceOfDataWrapper, settings);
            _logger.Debug($"TransportResponseChangeRxEventHandler.  jsonResp = {jsonResp} ");
        }

        /// <summary>
        /// Обработчик события смены режима поступления входных данных
        /// </summary>
        private void CycleDataEntryStateChangeRxEventHandler(InputDataStateRxModel dataState)
        {
            //Debug.WriteLine($"{dataState.KeyExchange}   {dataState.InputDataState}");//DEBUG
            _logger.Information($"NormalFrequencyCycleDataEntryChangeRxEventHandler.  {dataState.KeyExchange}  InputDataState= {dataState.InputDataState}");
            var exch= Exchanges.FirstOrDefault(e => e.KeyExchange.Equals(dataState.KeyExchange));
            switch (dataState.InputDataState)
            {
                case InputDataState.NormalEntry:
                    exch.Switch2NormalCycleExchange();
                    break;
                case InputDataState.ToLongNoEntry:
                    exch.Switch2CycleCommandEmergency();
                    break;
            }
        }

        #endregion




        #region Disposable

        public void Dispose()
        {
            UnsubscrubeOnExchangesEvents();
            UnsubscrubeOnExchangesCycleDataEntryStateEvents();
            _disposeMiddlewareInvokeServiceInvokeIsCompleteEventHandler.Dispose();
            MiddlewareInvokeService.Dispose();
        }

        #endregion
    }
}