using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Services.Exceptions;
using App.Services.Mediators;
using Autofac.Features.Indexed;
using CSharpFunctionalExtensions;
using Domain.Exchange;
using Domain.Exchange.Enums;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.ProvidersOption;
using Domain.InputDataModel.Base.Response;
using Shared.Types;

namespace App.Services.Actions
{
    /// <summary>
    /// Сервис для работы с текущим набором устройств
    /// * Запуск/Останов цикл обмена. 
    /// * Запуск/Останов бекгроунда транспорта. 
    /// * Послать вручную данные на обмен ус-ва.
    /// 
    /// </summary>
    public class DeviceActionService<TIn> where TIn : InputTypeBase
    {
        #region fields

        private readonly MediatorForStorages<TIn> _mediatorForStorages;

        #endregion




        #region ctor

        public DeviceActionService(MediatorForStorages<TIn> mediatorForStorages)
        {
            _mediatorForStorages = mediatorForStorages;
        }

        #endregion




        #region prop

        public IIndex<string, Func<ProviderOption, IDataProvider<TIn, ResponseInfo>>> DataProviderFactory { get; set; }  //внедряется через DI

        #endregion



        #region Methode

        /// <summary>
        /// Установить функции циклического обмена на бекгроунд
        /// </summary>
        /// <param name="exchnageKey"></param>
        public void StartCycleExchange(string exchnageKey)
        {
            var exchange = _mediatorForStorages.GetExchange(exchnageKey);
            if (exchange == null)
                throw new ActionHandlerException($"Обмен с таким ключем Не найден: {exchnageKey}");

            if (exchange.CycleBehavior.CycleBehaviorState != CycleBehaviorState.Off) 
                throw new ActionHandlerException($"Цикл. обмен уже запущен: {exchnageKey}");

            exchange.CycleBehavior.StartCycleExchange();
        }


        /// <summary>
        /// Снять функции циклического обмена с бекгроунда
        /// </summary>
        /// <param name="exchnageKey"></param>
        public void StopCycleExchange(string exchnageKey)
        {
            var exchange = _mediatorForStorages.GetExchange(exchnageKey);
            if (exchange == null)
                throw new ActionHandlerException($"Обмен с таким ключем Не найден: {exchnageKey}");

            if (exchange.CycleBehavior.CycleBehaviorState == CycleBehaviorState.Off)
                throw new ActionHandlerException($"Цикл. обмен уже остановлен: {exchnageKey}");

            exchange.CycleBehavior.StopCycleExchange();
        }


        /// <summary>
        /// Установить функции циклических обменов на бекгроунды
        /// </summary>
        /// <param name="exchnageKeys"></param>
        public void StartCycleExchanges(IReadOnlyList<string> exchnageKeys)
        {
            foreach (var exchnageKey in exchnageKeys)
            {
                StartCycleExchange(exchnageKey);
            }
        }


        /// <summary>
        /// Снять функции циклических обмена с бекгроундов
        /// </summary>
        /// <param name="exchnageKeys"></param>
        public void StopCycleExchanges(IReadOnlyList<string> exchnageKeys)
        {
            foreach (var exchnageKey in exchnageKeys)
            {
                StopCycleExchange(exchnageKey);
            }
        }



        //БЕКГРОУНД ЗАПУСКАТЬ/ОСТАНАВЛИВАТЬ ИЗ ОБМЕНА НЕЛЬЗЯ, Т.К.ОДИН БГ МОЖЕТ ВХОДИТЬ ВО МНОГО ОБМЕНОВ
        /// <summary>
        /// Запустить Бекграунд транспорта. 
        /// </summary>
        /// <param name="keyTransport"></param>
        public async Task StartBackground(KeyTransport keyTransport)
        {
            var bg = _mediatorForStorages.GetBackground(keyTransport);
            if (bg.IsStarted)
                throw new ActionHandlerException($"Бекграунд уже запущен: {bg.KeyTransport}");

            await bg.StartAsync(CancellationToken.None);
        }


        /// <summary>
        /// Остановить Бекграунд транспорта 
        /// </summary>
        /// <param name="keyTransport"></param>
        public async Task StopBackground(KeyTransport keyTransport)
        {
            var bg = _mediatorForStorages.GetBackground(keyTransport);
            if (!bg.IsStarted)
                throw new ActionHandlerException($"Бекграунд и так остановлен: {bg.KeyTransport}");

            await bg.StopAsync(CancellationToken.None);
        }


        /// <summary>
        /// Запустить коллекцию бекграундов
        /// </summary>
        /// <param name="keysTransport"></param>
        /// <returns></returns>
        public async Task StartBackgrounds(IReadOnlyList<KeyTransport> keysTransport)
        {
            var tasks = keysTransport.Select(StartBackground).ToList();
            await Task.WhenAll(tasks);
        }


        /// <summary>
        /// Остановить коллекцию бекграундов
        /// </summary>
        /// <param name="keysTransport"></param>
        /// <returns></returns>
        public async Task StopBackgrounds(IReadOnlyList<KeyTransport> keysTransport)
        {
            var tasks = keysTransport.Select(StopBackground).ToList();
            await Task.WhenAll(tasks);
        }


        /// <summary>
        /// Параллельный запуск открытия подключений на нескольких обменах
        /// </summary>
        public async Task StartCycleReOpenedConnections(IReadOnlyList<KeyTransport> transportKeys)
        {
            await Task.WhenAll(transportKeys.Select(StartCycleReOpenedConnection).ToArray());
        }


        /// <summary>
        /// Запуск открытия подключения для обмена
        /// </summary>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Обмен не найденн по ключу</exception>
        /// <exception cref="ActionHandlerException">Соединение уже открыто</exception>
        public async Task StartCycleReOpenedConnection(KeyTransport keyTransport)
        {
            var transport = _mediatorForStorages.GetTransport(keyTransport);
            if (transport == null)
                throw new KeyNotFoundException();

            if (transport.IsCycleReopened)
                throw new ActionHandlerException($"Транспорт уже находится в режиме циклического переоткрытия соединения: {keyTransport}");

            await transport.CycleReOpenedExec();
        }


        /// <summary>
        /// Останов задач циклического открытия подключения для нескольких обменов
        /// </summary>
        public void StopCycleReOpenedConnections(IReadOnlyList<KeyTransport> transportKeys)
        {
            foreach (var transportKey in transportKeys)
            {
                StopCycleReOpenedConnection(transportKey);
            }
        }


        /// <summary>
        /// Останов задачи циклического открытия подключения для транспорта
        /// </summary>
        public void StopCycleReOpenedConnection(KeyTransport keyTransport)
        {
            var transport = _mediatorForStorages.GetTransport(keyTransport);
            if (transport == null)
                throw new KeyNotFoundException();

            if (!transport.IsCycleReopened)
                throw new ActionHandlerException($"Транспорт вышел из режима циклического переоткрытия: {keyTransport}");

            transport.CycleReOpenedExecCancelation();
        }



        //TODO: Добавить в контроллер DevicesController
        /// <summary>
        /// Подписка ус-ва на события публикуемуе на шину данных.
        /// </summary>
        /// <param name="deviceName">Имя ус-ва</param>
        /// <param name="topicName4MessageBroker">название топика на шине данных</param>
        public void SubscrubeDeviceOnExchangesEvents(string deviceName, string topicName4MessageBroker = null)
        {
            var device = _mediatorForStorages.GetDevice(deviceName);
            if (device == null)
                throw new ArgumentException();

            //Подписка уже есть. подписываться 2-ой раз нельзя
            if (!string.IsNullOrEmpty(device.ProduserUnionKey))
                throw new ActionHandlerException($"Ошибка подписи устройства на передачу данных по шине. Устройство уже подписанно на шину {device.ProduserUnionKey}. Необходимо сначало отписаться");

            device.SubscrubeOnExchangesEvents(topicName4MessageBroker);
        }


        //TODO: Добавить в контроллер DevicesController
        /// <summary>
        /// Отписка ус-ва от событий публикуемых на шину данных.
        /// </summary>
        /// <param name="deviceName">Имя ус-ва</param>
        public void UnsubscrubeDeviceOnExchangesEvents(string deviceName)
        {
            var device = _mediatorForStorages.GetDevice(deviceName);
            if (device == null)
                throw new ArgumentException();

            device.UnsubscrubeOnExchangesEvents();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceName">Имя устройства</param>
        /// <param name="exchName">Название обмена</param>
        /// <returns>опции по которым был созданн провайдер данных в обмене.</returns>
        public Result<ProviderOption> GetProviderOption(string deviceName, string exchName)
        {
            var device = _mediatorForStorages.GetDevice(deviceName);
            if (device == null)
            {
                return Result.Fail<ProviderOption>($"устройство не найденно {deviceName}");
            }
            var exchange = device.Exchanges.FirstOrDefault(e => e.KeyExchange == exchName);
            if (exchange == null)
            {
                return Result.Fail<ProviderOption>($"Обмен не найденн {exchName}");
            }

            var providerOption = exchange.GetProviderOption;
            return Result.Ok(providerOption);
        }


        /// <summary>
        /// Установить новый провайдер для обмена в устройстве.
        /// </summary>
        /// <param name="deviceName">Имя устройства</param>
        /// <param name="exchName">Название обмена</param>
        /// <param name="providerOption">Обции для создания провайдера</param>
        /// <returns></returns>
        public Result SetProvider(string deviceName, string exchName, ProviderOption providerOption)
        {
            var device = _mediatorForStorages.GetDevice(deviceName);
            if (device == null)
            {
                return Result.Fail($"устройство не найденно {deviceName}");
            }
            var exchange = device.Exchanges.FirstOrDefault(e => e.KeyExchange == exchName);
            if (exchange == null)
            {
                return Result.Fail($"Обмен не найденн {exchName}");
            }
            try
            {
                var dataProvider = DataProviderFactory[providerOption.Name](providerOption);
                exchange.SetNewProvider(dataProvider);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail($"Исключение при установке ProviderOptionRt {ex}");
            }
        }

        #endregion
    }
}