﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Domain.Device.MiddleWares;
using Domain.Device.MiddleWares.Invokes;
using Domain.Device.Produser;
using Domain.Device.Repository.Entities.MiddleWareOption;
using Domain.Exchange;
using Domain.Exchange.Enums;
using Domain.Exchange.RxModel;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.Response;
using Infrastructure.EventBus.Abstract;
using Infrastructure.Transport.Base.RxModel;
using Newtonsoft.Json;
using Serilog;
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
        private readonly IEventBus _eventBus; //TODO: Not Use
        private readonly ProduserUnionStorage<TIn> _produserUnionStorage;
        private readonly ILogger _logger;
        private readonly List<IDisposable> _disposeExchangesEventHandlers = new List<IDisposable>();
        private readonly List<IDisposable> _disposeExchangesCycleDataEntryStateEventHandlers = new List<IDisposable>();
        private IDisposable _disposeMiddlewareInvokeServiceInvokeIsCompleteEventHandler;
        private readonly AllExchangesResponseAnalitic _allExchangesResponseAnalitic;
        #endregion



        #region prop
        public DeviceOption Option { get;  }
        public List<IExchange<TIn>> Exchanges { get; }
        public MiddlewareInvokeService<TIn> MiddlewareInvokeService { get; private set; }
        public string ProduserUnionKey => Option.ProduserUnionKey;
        /// <summary>
        /// Настройки MiddleWareInData сервиса.
        /// </summary>
        public MiddleWareInDataOption MiddleWareInDataOption
        {
            get=> Option.MiddleWareInData;
            set
            {
                Option.MiddleWareInData = value;
                CreateMiddleWareInDataByOption(Option.MiddleWareInData);
            }
        }
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

            CreateMiddleWareInDataByOption(Option.MiddleWareInData);
            _allExchangesResponseAnalitic= new AllExchangesResponseAnalitic(Exchanges.Select(exch=> exch.KeyExchange));
            _allExchangesResponseAnalitic.AllExchangeAnaliticDoneRx.Subscribe(AllExhangeAnaliticDoneEventHandler);
        }
        #endregion



        #region Methode
        /// <summary>
        /// Создать MiddlewareInvokeService из опций.
        /// </summary>
        /// <param name="option">Опции. Если option == null, то ранее созданный MiddlewareInvokeService уничтожается</param>
        private void CreateMiddleWareInDataByOption(MiddleWareInDataOption option)
        {
            _disposeMiddlewareInvokeServiceInvokeIsCompleteEventHandler?.Dispose();
            MiddlewareInvokeService?.Dispose();
            MiddlewareInvokeService = null;
            if (option != null)
            {
                var middleWareInData = new MiddleWareInData<TIn>(option, _logger);
                MiddlewareInvokeService = new MiddlewareInvokeService<TIn>(option.InvokerOutput, middleWareInData, _logger);
                _disposeMiddlewareInvokeServiceInvokeIsCompleteEventHandler= MiddlewareInvokeService?.InvokeIsCompleteRx.Subscribe(MiddlewareInvokeIsCompleteRxEventHandler);
            }
        }


        /// <summary>
        /// Подписка на события от обменов.
        /// </summary>
        /// <param name="produserUnionKey">Имя топика, если == null, то берется из настроек</param>
        public bool SubscrubeOnExchangesEvents(string produserUnionKey = null)
        {
            Exchanges.ForEach(exch =>
            {
                _disposeExchangesEventHandlers.Add(exch.IsConnectChangeRx.Subscribe(ConnectChangeRxEventHandler));
                //_disposeExchangesEventHandlers.Add(exch.LastSendDataChangeRx.Subscribe(LastSendDataChangeRxEventHandler));
                _disposeExchangesEventHandlers.Add(exch.IsOpenChangeTransportRx.Subscribe(OpenChangeTransportRxEventHandler));
                _disposeExchangesEventHandlers.Add(exch.ResponseChangeRx.Subscribe(ResponseChangeRxEventHandler));
            });
            return true;
        }


        /// <summary>
        /// Отписка на событий обменов.
        /// </summary>
        public void UnsubscrubeOnExchangesEvents()
        {
            _disposeExchangesEventHandlers.ForEach(d=>d.Dispose());
        }


        /// <summary>
        /// Подписка на события смены состояния обменов.
        /// </summary>
        /// <returns></returns>
        public bool SubscrubeOnExchangesCycleDataEntryStateEvents()
        {
            Exchanges.ForEach(exch=>
            {
                _disposeExchangesCycleDataEntryStateEventHandlers.Add(exch.CycleDataEntryStateChangeRx.Subscribe(CycleDataEntryStateChangeRxEventHandler));
            });
            return true;
        }


        //TODO: 
        public void UnsubscrubeOnExchangesCycleDataEntryStateEvents()
        {
            _disposeExchangesCycleDataEntryStateEventHandlers.ForEach(d => d.Dispose());
        }


        /// <summary>
        /// Принять данные для УСТРОЙСТВА.
        /// Данные будут переданны напрямую на обмены или в конвеер MiddleWare (если обработчик MiddleWare создан)
        /// Команды передаются напрямую.
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
            var tasks= new List<Task>();
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
            string warningStr;
            if (!exchange.IsStartedTransportBg)
            {
                warningStr = $"Отправка данных НЕ удачна, Бекграунд обмена {exchange.KeyExchange} НЕ ЗАПУЩЕН";
                await SendWarningResult(warningStr);
                return;
            }
            if (!exchange.IsOpen)
            {
                warningStr = $"Отправка данных НЕ удачна, соединение транспорта для обмена {exchange.KeyExchange} НЕ ОТКРЫТО";
                await SendWarningResult(warningStr);
                return;
            }
            switch (dataAction)
            {
                case DataAction.OneTimeAction:
                    if (exchange.IsFullOneTimeDataQueue)
                    {
                        warningStr = $"Отправка данных НЕ удачна, очередь однократных данных ПЕРЕПОЛНЕННА для обмена: {exchange.KeyExchange}";
                        await SendWarningResult(warningStr);
                        return;
                    }
                    exchange.SendOneTimeData(inData, directHandlerName);
                    break;

                case DataAction.CycleAction:
                    if (exchange.CycleExchnageStatus == CycleExchnageStatus.Off)
                    {
                        warningStr = $"Отправка данных НЕ удачна, Цикл. обмен для обмена {exchange.KeyExchange} НЕ ЗАПУЩЕН";
                        await SendWarningResult(warningStr);
                        return;
                    }
                    if (exchange.IsFullCycleTimeDataQueue)
                    {
                        warningStr = $"Отправка данных НЕ удачна, очередь цикличеких данных ПЕРЕПОЛНЕННА для обмена: {exchange.KeyExchange}";
                        await SendWarningResult(warningStr);
                        return;
                    }
                    exchange.SendCycleTimeData(inData, directHandlerName);
                    break;

                case DataAction.CommandAction:
                    if (exchange.IsFullOneTimeDataQueue)
                    {
                        warningStr = $"Отправка команды НЕ удачна, очередь однократных данных ПЕРЕПОЛНЕННА для обмена: {exchange.KeyExchange}";
                        await SendWarningResult(warningStr);
                        return;
                    }
                    exchange.SendCommand(command4Device);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(dataAction), dataAction, null);
            }
        }


        /// <summary>
        /// Отправить предупреждение о неверной работе устройства.
        /// </summary>
        private async Task SendWarningResult(string warningStr)
        {
            _logger.Warning(warningStr);
            await Send2ProduderUnion(Option.Name, warningStr);
        }


        /// <summary>
        /// Отправить ответ от обмена на ProduserUnion.
        /// </summary>
        private async Task Send2ProduderUnion(ResponsePieceOfDataWrapper<TIn> response)
        {
            var produser = _produserUnionStorage.Get(ProduserUnionKey);
            if (produser == null)
            {
                _logger.Error($"Продюссер по ключу {ProduserUnionKey} НЕ НАЙДЕНН для Устройства= {Option.Name}");
                return;
            }

            var results= await produser.SendResponseAll(response);
            foreach (var (isSuccess, isFailure, _, error) in results)
            {
                if (isFailure)
                    _logger.Error($"Ошибки отправки ответов для Устройства= {Option.Name} через ProduderUnion = {ProduserUnionKey}  {error}");

                if(isSuccess)
                    _logger.Information($"ОТПРАВКА ОТВЕТОВ УСПЕШНА для устройства {Option.Name} через ProduderUnion = {ProduserUnionKey}");
            }
        }


        /// <summary>
        /// Отправить сообщение от устройства на ProduserUnion.
        /// </summary>
        private async Task Send2ProduderUnion(string objectName, string message)
        {
            var produser = _produserUnionStorage.Get(ProduserUnionKey);
            if (produser == null)
            {
                _logger.Error($"Продюссер по ключу {ProduserUnionKey} НЕ НАЙДЕНН для Устройства= {Option.Name}");
                return;
            }

            var results = await produser.SendMessageAll(objectName, message);
            foreach (var (isSuccess, isFailure, _, error) in results)
            {
                if (isFailure)
                    _logger.Error($"Ошибки отправки сообщений для Устройства= {Option.Name} через ProduderUnion = {ProduserUnionKey}  {error}");

                if (isSuccess)
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


        /// <summary>
        /// Обработчик события получения Результата обмена.
        /// </summary>
        /// <param name="responsePieceOfDataWrapper"></param>
        private async void ResponseChangeRxEventHandler(ResponsePieceOfDataWrapper<TIn> responsePieceOfDataWrapper)
        {
            //Топик не указан. Нет отправки ответа через ProduserUnion.
            if (!string.IsNullOrEmpty(ProduserUnionKey))
                await Send2ProduderUnion(responsePieceOfDataWrapper);

            //Анализ ответов от всех обменов.
            _allExchangesResponseAnalitic.SetResponseResult(responsePieceOfDataWrapper.KeyExchange, responsePieceOfDataWrapper.IsValidAll);
          
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
        /// Событие все обмены завершены
        /// </summary>
        /// <param name="allExchResultTuple">true- если все обмены завершены успешно</param>
        private void AllExhangeAnaliticDoneEventHandler((bool, bool) allExchResultTuple)
        {
            var (allSucsess, anySucsess) = allExchResultTuple;
            if (anySucsess)
            {
                //Если хотя бы 1 обмен завершен успешно, выставить флаг обратной связи для MiddlewareInvokeService
                MiddlewareInvokeService?.SetFeedBack();
            }
        }
            

        /// <summary>
        /// Обработчик события смены режима поступления входных данных
        /// </summary>
        private void CycleDataEntryStateChangeRxEventHandler(InputDataStateRxModel dataState)
        {
            //Debug.WriteLine($"{dataState.KeyExchange}   {dataState.InputDataState}");//DEBUG
            _logger.Information($"NormalFrequencyCycleDataEntryChangeRxEventHandler.  {dataState.KeyExchange}  InputDataState= {dataState.InputDataStatus}");
            var exch= Exchanges.FirstOrDefault(e => e.KeyExchange.Equals(dataState.KeyExchange));
            switch (dataState.InputDataStatus)
            {
                case InputDataStatus.NormalEntry:
                    exch.Switch2NormalCycleExchange();
                    break;
                case InputDataStatus.ToLongNoEntry:
                    exch.Switch2CycleCommandEmergency();
                    break;
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
                var inData = result.Value;
                await ResiveInExchange(inData);
                _logger.Information($"Данные УСПЕШНО подготовленны MiddlewareInData для устройства: {Option.Name}");
            }
            else
            {
                _logger.Error($"ОШИБКИ ПРЕОБРАЗОВАНИЯ ВХОДНЫХ ДАННЫХ MiddlewareInData ДЛЯ: {Option.Name} Errors= {result.Error.GetErrorsArgegator}"); //ВСЕ ОШИБКИ ПРЕОБРАЗОВАНИЯ ВХОДНЫХ ДАННЫХ.
            }
        }
        #endregion



        #region NestedClass
        /// <summary>
        /// Анализ ответов от всех обменов.
        /// </summary>
        private class AllExchangesResponseAnalitic
        {
            #region fields
            private readonly ConcurrentDictionary<string, bool?> _dictionary = new ConcurrentDictionary<string, bool?>();
            #endregion


            #region ExchangeRx
            /// <summary>
            /// Событие. Все Обмены завершены.
            /// </summary>
            public ISubject<(bool, bool)> AllExchangeAnaliticDoneRx{ get; } = new Subject<(bool, bool)>();  //(allResultSucsses, anyResultSucsses)
            #endregion


            #region ctor
            public AllExchangesResponseAnalitic(IEnumerable<string> keys)
            {
                foreach (var key in keys)
                {
                    _dictionary[key] = null;
                }
            }
            #endregion


            #region Methode
            /// <summary>
            /// Записать результат обмена (ответ).
            /// И выполнить аналитику всех ответов.
            /// </summary>
            /// <param name="key">ключ</param>
            /// <param name="respResult">ответ</param>
            public void SetResponseResult(string key, bool respResult)
            {
                if (_dictionary.ContainsKey(key))
                {
                    _dictionary[key] = respResult;
                    DoAnalitic();
                }
            }

            /// <summary>
            /// Аналитика результатов обмена.
            /// Васе обмены должны закончится (получить результат)
            /// И хотя бы 1 обмен должен завершится успешно.
            /// Тогда срабатывает событие AnalyticsDoneRx.
            /// </summary>
            private void DoAnalitic()
            {
               var allResult= _dictionary.Select(pair => pair.Value).ToList();
               var allResultSet = allResult.All(flag => flag.HasValue);            //Все обмены получили результат.
               if (allResultSet)
               {
                   var anyResultSucsses = allResult.Any(flag => flag ?? false);    //Хотя бы один обмен завершился успешно.
                   var allResultSucsses = allResult.All(flag => flag ?? false);    //Все обмены завершились успешно.
                   var tuple = (allResultSucsses, anyResultSucsses);
                   ResetAllResult();
                   AllExchangeAnaliticDoneRx.OnNext(tuple);
               }
            }

            /// <summary>
            /// Сбросить все значения в null.
            /// </summary>
            private void ResetAllResult()
            {
                foreach (var key in _dictionary.Keys.ToArray())
                {
                    _dictionary[key] = null;
                }
            }

            #endregion
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