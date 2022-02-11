using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using CSharpFunctionalExtensions;
using Domain.Device.MiddleWares4InData;
using Domain.Device.MiddleWares4InData.Invokes;
using Domain.Device.Paged4InData;
using Domain.Device.Services;
using Domain.Exchange;
using Domain.Exchange.Enums;
using Domain.Exchange.Models;
using Domain.Exchange.RxModel;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.Response;
using Infrastructure.Transport.Base.RxModel;
using Newtonsoft.Json;
using Serilog;
using Shared.Extensions;
using Shared.Paged;
using DeviceOption = Domain.Device.Repository.Entities.DeviceOption;


namespace Domain.Device
{
    /// <summary>
    /// Устройство.
    /// Содержит список обменов.
    /// Ответы о своей работе каждое ус-во выставляет самостоятельно на шину данных.
    /// </summary>
    public class Device<TIn> : IDisposable where TIn : InputTypeBase
    {
        #region field
        private readonly Func<PagedOption, Owned<PagingInvokeService<TIn>>> _pagedInvokeServiceFactory;                        //PagingInvokeService пересоздается динамически, поэтому стару версию нужно уничтожать через Owned
        private readonly Func<MiddleWareMediatorOption, Owned<MiddlewareInvokeService<TIn>>> _middlewareInvokeServiceFactory;  //MiddlewareInvokeService пересоздается динамически, поэтому стару версию нужно уничтожать через Owned
        
        private readonly ProduserAdapter<TIn> _produserAdapter;
        private readonly List<IDisposable> _disposeExchangesEventHandlers = new List<IDisposable>();
        private readonly List<IDisposable> _disposeExchangesCycleBehaviorEventHandlers = new List<IDisposable>();
        private readonly AllExchangesResponseAnaliticService _allCycleBehaviorResponseAnalitic;
        
        private IDisposable? _disposeMiddlewareInvokeServiceInvokeIsCompleteLifeTime;
        private readonly IDisposable? _allCycleBehaviorResponseAnaliticOwner;
        private IDisposable? _middlewareInvokeServiceOwner;
        private IDisposable? _pagedInvokeServiceOwner;
        private IDisposable? _pagedInvokeServiceNextPageRxLifeTime;
        private readonly ILogger _logger;
        #endregion



        #region prop
        public DeviceOption Option { get; }
        public List<IExchange<TIn>> Exchanges { get; }
        public PagingInvokeService<TIn>? PagedInvokeService { get; private set; }
        public MiddlewareInvokeService<TIn>? MiddlewareInvokeService { get; private set; }
        public string ProduserUnionKey => Option.ProduserUnionKey;
        
        /// <summary>
        /// Настройки MiddleWareMediator сервиса.
        /// </summary>
        public MiddleWareMediatorOption MiddleWareMediatorOption
        {
            get => Option.MiddleWareMediator;
            set
            {
                Option.MiddleWareMediator = value;
                CreateMiddleWareInDataByOption(Option.MiddleWareMediator);
            }
        }
        
        /// <summary>
        /// Настройки Paging сервиса.
        /// </summary>
        public PagedOption PagedOption
        {
            get => Option.Paging;
            set
            {
                Option.Paging = value;
                CreatePagedServiceByOption(Option.Paging);
            }
        }
        #endregion



        #region ctor
        public Device(DeviceOption option,
                      IEnumerable<IExchange<TIn>> exchanges,
                      Func<(string, string), Func<List<ExchangeFullState<TIn>>>, ProduserAdapter<TIn>> produserAdapterFactory,
                      Func<PagedOption, Owned<PagingInvokeService<TIn>>> pagedInvokeServiceFactory,
                      Func<MiddleWareMediatorOption, Owned<MiddlewareInvokeService<TIn>>> middlewareInvokeServiceFactory,
                      Func<IEnumerable<string>, Owned<AllExchangesResponseAnaliticService>> allExchangesResponseAnaliticServiceFactory,
                      ILogger logger)
        {
            Option = option;
            Exchanges = exchanges.ToList();
            _pagedInvokeServiceFactory = pagedInvokeServiceFactory;
            _middlewareInvokeServiceFactory = middlewareInvokeServiceFactory;
            
            _produserAdapter= produserAdapterFactory((Option.ProduserUnionKey, Option.Name), () => Exchanges.Select(exch => exch.FullState).ToList());
            
            var owner = allExchangesResponseAnaliticServiceFactory(Exchanges.Select(exch => exch.KeyExchange));
            _allCycleBehaviorResponseAnalitic = owner.Value;
            _allCycleBehaviorResponseAnalitic.AllExchangeAnaliticDoneRx.Subscribe(AllExhangeAnaliticDoneRxEventHandler);
            _allCycleBehaviorResponseAnaliticOwner = owner;

            CreatePagedServiceByOption(Option.Paging);
            CreateMiddleWareInDataByOption(Option.MiddleWareMediator);
            
            _logger = logger;
        }
        #endregion



        #region Methode
        /// <summary>
        /// Создать MiddlewareInvokeService из опций.
        /// </summary>
        /// <param name="option">Опции. Если option == null, то ранее созданный MiddlewareInvokeService уничтожается</param>
        private void CreateMiddleWareInDataByOption(MiddleWareMediatorOption? option)
        {
            _disposeMiddlewareInvokeServiceInvokeIsCompleteLifeTime?.Dispose();
            _middlewareInvokeServiceOwner?.Dispose();
            MiddlewareInvokeService = null;
            if (option != null)
            {
               // var middleWareInData = new MiddleWareMediator<TIn>(option, _logger); //TODO: Создавать внутри MiddlewareInvokeService
                var owner = _middlewareInvokeServiceFactory(option);
                MiddlewareInvokeService = owner.Value;
                _middlewareInvokeServiceOwner = owner;
                _disposeMiddlewareInvokeServiceInvokeIsCompleteLifeTime = MiddlewareInvokeService?.InvokeIsCompleteRx.Subscribe(MiddlewareInvokeIsCompleteRxEventHandler);
            }
        }
        
        
        /// <summary>
        /// Создать PagedService из опций.
        /// </summary>
        /// <param name="option">Опции. Если option == null, то ранее созданный PagedService уничтожается</param>
        private void CreatePagedServiceByOption(PagedOption? option)
        {
            _pagedInvokeServiceNextPageRxLifeTime?.Dispose();
            _pagedInvokeServiceOwner?.Dispose();
            PagedInvokeService = null;
            if (option != null)
            {
                var owner = _pagedInvokeServiceFactory(option);
                PagedInvokeService = owner.Value;
                _pagedInvokeServiceOwner = owner;
                _pagedInvokeServiceNextPageRxLifeTime = PagedInvokeService?.NextPageRx.Subscribe(PagedServiceOnNextEventHandler);
            }
        }

        
        /// <summary>
        /// Подписка на события от обменов.
        /// </summary>
        public bool SubscrubeOnExchangesEvents()
        {
            Exchanges.ForEach(exch =>
            {
                _disposeExchangesEventHandlers.Add(exch.IsConnectChangeRx.Subscribe(ConnectChangeRxEventHandler));
                //_disposeExchangesEventHandlers.Add(exch.LastSendDataChangeRx.Subscribe(LastSendDataChangeRxEventHandler));
                _disposeExchangesEventHandlers.Add(exch.IsOpenChangeTransportRx.Subscribe(OpenChangeTransportRxEventHandler));
                _disposeExchangesEventHandlers.Add(exch.CycleBehavior.ResponseReadyRx.SubscribeAsyncConcurrent(CycleBehaviorResponseReadyRxEventHandler));
                _disposeExchangesEventHandlers.Add(exch.OnceBehavior.ResponseReadyRx.SubscribeAsyncConcurrent(OnceAndCommandBehaviorResponseReadyRxEventHandler));
                _disposeExchangesEventHandlers.Add(exch.CommandBehavior.ResponseReadyRx.SubscribeAsyncConcurrent(OnceAndCommandBehaviorResponseReadyRxEventHandler));
            });
            return true;
        }


        /// <summary>
        /// Отписка от событий обменов.
        /// </summary>
        public void UnsubscrubeOnExchangesEvents()
        {
            _disposeExchangesEventHandlers.ForEach(d => d.Dispose());
        }

        /// <summary>
        /// Подписка на событие от продюссеров
        /// </summary>
        public bool SubscrubeOnProdusersEvents()
        {
          return _produserAdapter.SubscrubeOnEvents();
        }


        /// <summary>
        /// Отписка от событий обменов.
        /// </summary>
        public void UnsubscrubeOnProdusersEvents()
        {
            _produserAdapter.UnsubscrubeOnEvents();
        }


        /// <summary>
        /// Подписка на события смены состояния Цикл поведения обменов.
        /// </summary>
        public bool SubscrubeOnExchangesCycleBehaviorEvents()
        {
            Exchanges.ForEach(exch =>
            {
                _disposeExchangesCycleBehaviorEventHandlers.Add(exch.CycleBehavior.CycleBehaviorStateChangeRx.SubscribeAsyncConcurrent(CycleBehaviorStateChangeRxEventHandler));
            });
            return true;
        }


        /// <summary>
        /// Отписка от событий смены состояния Цикл поведения обменов.
        /// </summary>
        public void UnsubscrubeOnExchangesCycleBehaviorEvents()
        {
            _disposeExchangesCycleBehaviorEventHandlers.ForEach(d => d.Dispose());
        }


        /// <summary>
        /// Принять данные для УСТРОЙСТВА.
        /// Данные будут переданны напрямую на обмены или в конвеер MiddleWare (если обработчик MiddleWare создан)
        /// Команды передаются напрямую.
        /// </summary>
        /// <param name="inData">входные данные в обертке</param>
        public async Task Resive(InputData<TIn> inData)
        {
            if (PagedInvokeService != null && inData.DataAction == DataAction.CycleAction)
            {
                PagedInvokeService.SetData(inData);
            }
            else
            if (MiddlewareInvokeService != null)
            {
                switch (inData.DataAction)
                {
                    case DataAction.OneTimeAction://однократные данные обрабатываем сразу.
                        await MiddlewareInvokeService.InputSetInstantly(inData);
                        break;
                    case DataAction.CycleAction: //циклические данные поступают в обработку MiddlewareInvokeService.
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


        /// <summary>
        /// Получить текушее состояние обменов
        /// </summary>
        /// <param name="keyExchange">ключ обмена</param>
        /// <returns></returns>
        public IReadOnlyList<ExchangeInfoModel> GetExchnagesInfo(string keyExchange = null)
        {
            var infoListQuery = Exchanges.Select(exch => new ExchangeInfoModel(exch.KeyExchange, exch.IsConnect, exch.IsOpen));
            if (string.IsNullOrEmpty(keyExchange))
            {
                return infoListQuery.ToList();
            }
            var info = infoListQuery.FirstOrDefault(ex => ex.KeyExchange == keyExchange);
            return new List<ExchangeInfoModel>{ info };
        }


        /// <summary>
        /// Передать данные на все обмены или на конкретный обмен.
        /// </summary>
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


        /// <summary>
        /// Отправить данные или команду на все обмены.
        /// </summary>
        private async Task Send2AllExchanges(DataAction dataAction, List<TIn> inData, Command4Device command4Device = Command4Device.None)
        {
            var tasks = new List<Task>();
            foreach (var exchange in Exchanges)
            {
                tasks.Add(SendDataOrCommand(exchange, dataAction, inData, command4Device));
            }
            await Task.WhenAll(tasks);
        }


        /// <summary>
        /// Отправить данные или команду на конкретный обмен.
        /// </summary>
        private async Task Send2ConcreteExchanges(string keyExchange, DataAction dataAction, List<TIn> inData, Command4Device command4Device = Command4Device.None, string directHandlerName = null)
        {
            var exchange = Exchanges.FirstOrDefault(exch => exch.KeyExchange == keyExchange);
            if (exchange == null)
            {
                //await Send2Produder(Option.TopicName4MessageBroker, $"Обмен не найденн для этого ус-ва {KeyExchange}");
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
            string warningStr;
            if (!exchange.IsStartedTransportBg)
            {
                warningStr = $"Отправка данных НЕ удачна, Бекграунд обмена {exchange.KeyExchange} НЕ ЗАПУЩЕН";
                await SendWarning2Produser(Option.Name, exchange.KeyExchange, warningStr);
                return;
            }
            if (!exchange.IsOpen)
            {
                warningStr = $"Отправка данных НЕ удачна, соединение транспорта для обмена {exchange.KeyExchange} НЕ ОТКРЫТО";
                await SendWarning2Produser(Option.Name, exchange.KeyExchange, warningStr);
                return;
            }
            switch (dataAction)
            {
                case DataAction.OneTimeAction:
                    if (exchange.OnceBehavior.IsFullDataQueue)
                    {
                        warningStr = $"Отправка данных НЕ удачна, очередь однократных данных ПЕРЕПОЛНЕННА для обмена: {exchange.KeyExchange}";
                        await SendWarning2Produser(Option.Name, exchange.KeyExchange, warningStr);
                        return;
                    }
                    exchange.SendOneTimeData(inData, directHandlerName);
                    break;

                case DataAction.CycleAction:
                    if (exchange.CycleBehavior.CycleBehaviorState == CycleBehaviorState.Off)
                    {
                        warningStr = $"Отправка данных НЕ удачна, Цикл. обмен для обмена {exchange.KeyExchange} НЕ ЗАПУЩЕН";
                        await SendWarning2Produser(Option.Name, exchange.KeyExchange, warningStr);
                        return;
                    }
                    if (exchange.CycleBehavior.IsFullDataQueue)
                    {
                        warningStr = $"Отправка данных НЕ удачна, очередь цикличеких данных ПЕРЕПОЛНЕННА для обмена: {exchange.KeyExchange}";
                        await SendWarning2Produser(Option.Name, exchange.KeyExchange, warningStr);
                        return;
                    }
                    exchange.SendCycleTimeData(inData, directHandlerName);
                    break;

                case DataAction.CommandAction:
                    if (exchange.CommandBehavior.IsFullDataQueue)
                    {
                        warningStr = $"Отправка команды НЕ удачна, очередь однократных данных ПЕРЕПОЛНЕННА для обмена: {exchange.KeyExchange}";
                        await SendWarning2Produser(Option.Name, exchange.KeyExchange, warningStr);
                        return;
                    }
                    exchange.SendCommand(command4Device);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(dataAction), dataAction, null);
            }
        }

        private async Task SendWarning2Produser(string deviceName, string keyExchange, string warningStr)
        {
            var warningObj = new {DeviceName= deviceName, KeyExchange = keyExchange, Message = warningStr };
            await _produserAdapter.SendWarningAsync(warningObj);
            _logger.Warning("{Type} {KeyExchange} {WarningStatus}", "Отправка данных НЕ удачна.", keyExchange, warningStr);
        }

        #endregion



        #region RxEventHandler 
        /// <summary>
        /// Обработка события смены флага IsConnect у обмена.
        /// </summary>
        private async void ConnectChangeRxEventHandler(ConnectChangeRxModel model)
        {
            var warningStr = $"Exchange Connect Change. IsConnect = {model.IsConnect} для ОБМЕНА {model.KeyExchange}";
            var warningObj = new { DeviceName=Option.Name,  KeyExchange = model.KeyExchange, IsConnect= model.IsConnect, Message = warningStr };
            await _produserAdapter.SendWarningAsync(warningObj);
            _logger.Warning(warningStr);
        }


        /// <summary>
        /// Обработка события смены флага IsOpen у обмена.
        /// </summary>
        private async void OpenChangeTransportRxEventHandler(IsOpenChangeRxModel model)
        {
            var warningStr =$"Transport Open Change. IsOpen = {model.IsOpen} для ТРАНСПОРТА {model.TransportName}";
            var warningObj = new { DeviceName = Option.Name, KeyExchange = model.KeyExchange, TransportName = model.TransportName, IsOpen = model.IsOpen, Message = warningStr };
            await _produserAdapter.SendWarningAsync(warningObj);
            _logger.Warning(warningStr);
        }


        /// <summary>
        /// Обработчик события получения Результата обмена от циклического поведения.
        /// </summary>
        private async Task CycleBehaviorResponseReadyRxEventHandler(ResponsePieceOfDataWrapper<TIn> responsePieceOfDataWrapper)
        {
            //Анализ ответов от всех обменов.
           var exchangesInfoTuple= Exchanges.Select(exchange=> (key: exchange.KeyExchange, isOpen: exchange.IsOpen)).ToList(); 
            _allCycleBehaviorResponseAnalitic.SetResponseResult(responsePieceOfDataWrapper.KeyExchange, responsePieceOfDataWrapper.Evaluation.IsValidAll, exchangesInfoTuple);
            await OnceAndCommandBehaviorResponseReadyRxEventHandler(responsePieceOfDataWrapper);
        }


        /// <summary>
        /// Обработчик события получения Результата обмена от однократного обмена или команды.
        /// </summary>
        private async Task OnceAndCommandBehaviorResponseReadyRxEventHandler(ResponsePieceOfDataWrapper<TIn> responsePieceOfDataWrapper)
        {
            //Топик не указан. Нет отправки ответа через ProduserUnion.
            if (_produserAdapter.IsExistProduserUnion)
                await _produserAdapter.SendData2ProduderUnionAsync(responsePieceOfDataWrapper);

            //логирование ответов в полном виде
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,             //Отступы дочерних элементов 
                NullValueHandling = NullValueHandling.Ignore  //Игнорировать пустые теги
            };
            
            //TODO: Не вызывать сериализатор, если текущий уровень _logger.Debug
            var jsonResp = JsonConvert.SerializeObject(responsePieceOfDataWrapper, settings);
            _logger.Debug($"TransportResponseChangeRxEventHandler.  jsonResp = {jsonResp} ");
        }


        /// <summary>
        /// Событие все обмены завершены.
        /// АНАЛИЗ ПРОВОДИТСЯ ТОЛЬКО ДЛЯ ЦИКЛ. ПОВЕДЕНИЯ
        /// </summary>
        /// <param name="allExchResultTuple">true- если все обмены завершены успешно</param>
        private void AllExhangeAnaliticDoneRxEventHandler((bool, bool) allExchResultTuple)
        {
            var (_, anySucsess) = allExchResultTuple;
            if (anySucsess)
            {
                //Если хотя бы 1 обмен завершен успешно, выставить флаг обратной связи для MiddlewareInvokeService
                MiddlewareInvokeService?.SetFeedBack();
            }
        }


        /// <summary>
        /// Обработчик события смены режима Циклической обмена
        /// </summary>
        private async Task CycleBehaviorStateChangeRxEventHandler(CycleBehaviorStateRxModel dataState)
        {
            var message = $"Переключился режим Циклического обмена.  {dataState.KeyExchange}  {dataState.CycleBehaviorState}";
            _logger.Information(message);

            var messageObj = new
            {
               DeviceName= Option.Name,
               KeyExchange= dataState.KeyExchange,
               CycleBehaviorState= dataState.CycleBehaviorState,
               Message= message
            };

            if (_produserAdapter.IsExistProduserUnion)
                await _produserAdapter.SendInfoAsync(messageObj);
        }

        
        /// <summary>
        /// Обработчик события получения данных от сервиса Paging.
        /// </summary>
        /// <param name="inData"></param>
        private async void PagedServiceOnNextEventHandler(InputData<TIn> inData)
        {
            _logger.Information($"Данные УСПЕШНО подготовленны Paging для устройства: {Option.Name}  Count= '{inData.Data.Count}'");
            if (MiddlewareInvokeService != null)
            {
                switch (inData.DataAction)
                {
                    case DataAction.OneTimeAction://однократные данные обрабатываем сразу.
                        await MiddlewareInvokeService.InputSetInstantly(inData);
                        break;
                    case DataAction.CycleAction: //циклические данные поступают в обработку MiddlewareInvokeService.
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
        
        
        /// <summary>
        /// Обработчик события получения данных после подготовки в Middleware.
        /// </summary>
        /// <param name="result">результат подготовки данных через конвееры Middleware</param>
        private async void MiddlewareInvokeIsCompleteRxEventHandler(Result<InputData<TIn>, ErrorResultMiddleWareInData> result)
        {
            if (result.IsSuccess)
            {
                _logger.Information($"Данные УСПЕШНО подготовленны MiddlewareInData для устройства: {Option.Name}");
                var inData = result.Value;
                await ResiveInExchange(inData);
            }
            else
            {
                _logger.Error($"ОШИБКИ ПРЕОБРАЗОВАНИЯ ВХОДНЫХ ДАННЫХ MiddlewareInData ДЛЯ: {Option.Name} Errors= {result.Error.GetErrorsArgegator}"); //ВСЕ ОШИБКИ ПРЕОБРАЗОВАНИЯ ВХОДНЫХ ДАННЫХ.
            }
        }
        #endregion



        #region Disposable
        public void Dispose()
        {
            _middlewareInvokeServiceOwner?.Dispose();
            _disposeMiddlewareInvokeServiceInvokeIsCompleteLifeTime?.Dispose();
            _allCycleBehaviorResponseAnaliticOwner?.Dispose();
            _pagedInvokeServiceOwner?.Dispose();
            _pagedInvokeServiceNextPageRxLifeTime?.Dispose();
            
            UnsubscrubeOnExchangesEvents();
            UnsubscrubeOnProdusersEvents();
            UnsubscrubeOnExchangesCycleBehaviorEvents();
        }
        #endregion
    }
}