﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Services.Config;
using App.Services.Exceptions;
using Autofac.Features.Indexed;
using Autofac.Features.OwnedInstances;
using Domain.Device;
using Domain.Device.Produser;
using Domain.Exchange;
using Domain.Exchange.Enums;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.ProvidersOption;
using Domain.InputDataModel.Base.Response;
using Infrastructure.Background;
using Infrastructure.Background.Abstarct;
using Infrastructure.Background.Concrete.HostingBackground;
using Infrastructure.EventBus.Abstract;
using Infrastructure.Transport;
using Infrastructure.Transport.Base.Abstract;
using Infrastructure.Transport.Http;
using Infrastructure.Transport.SerialPort;
using Infrastructure.Transport.TcpIp;
using Serilog;
using Shared.Enums;
using Shared.Types;
using OptionAgregator = App.Services.Agregators.OptionAgregator;

namespace App.Services.Mediators
{
    /// <summary>
    /// Сервис объединяет работу со всеми Storage,
    /// и предоставляет интерфейс для Добавления/Удаления элементов в Storage
    /// </summary>
    public class MediatorForStorages<TIn> where TIn : InputTypeBase
    {
        #region fields

        private readonly DeviceStorage<TIn> _deviceStorage;
        private readonly ExchangeStorage<TIn> _exchangeStorage;
        private readonly BackgroundStorage _backgroundStorage;
        private readonly TransportStorage _transportStorage;
        private readonly ProduserUnionStorage<TIn> _produserUnionStorage;
        private readonly IEventBus _eventBus;
        private readonly IIndex<string, Func<ProviderOption, Owned<IDataProvider<TIn, ResponseInfo>>>> _dataProviderFactory;
        private readonly AppConfigWrapper _appConfigWrapper;
        private readonly ILogger _logger;
        //опции для создания IProduser через фабрику

        #endregion




        #region ctor

        public MediatorForStorages(DeviceStorage<TIn> deviceStorage,
            ExchangeStorage<TIn> exchangeStorage,
            BackgroundStorage backgroundStorage,
            TransportStorage transportStorage,
            ProduserUnionStorage<TIn> produserUnionStorage,
            IEventBus eventBus,     
            IIndex<string, Func<ProviderOption, Owned<IDataProvider<TIn, ResponseInfo>>>> dataProviderFactory,
            AppConfigWrapper appConfigWrapper,
            ILogger logger)
        {
            _transportStorage = transportStorage;
            _produserUnionStorage = produserUnionStorage;
            _backgroundStorage = backgroundStorage;
            _exchangeStorage = exchangeStorage;
            _deviceStorage = deviceStorage;
            _eventBus = eventBus;
            _dataProviderFactory = dataProviderFactory;
            _appConfigWrapper = appConfigWrapper;
            _logger = logger;
        }

        #endregion




        #region Methode

        /// <summary>
        /// Вернуть устройство по  имени устройства.
        /// </summary>
        /// <param name="deviceName">Имя ус-ва, он же ключ к хранилищу</param>
        /// <returns>Верунть ус-во</returns>
        public Device<TIn> GetDevice(string deviceName)
        {
            var device = _deviceStorage.Get(deviceName);
            return device;
        }


        public IReadOnlyList<Device<TIn>> GetDevices()
        {
            return _deviceStorage.Values.ToList();
        }


        /// <summary>
        /// Получить список устройств, использующих этот обмен по ключу
        /// </summary>
        /// <param name="exchnageKey"></param>
        /// <returns></returns>
        public IReadOnlyList<Device<TIn>> GetDevicesUsingExchange(string exchnageKey)
        {
            return _deviceStorage.Values.Where(dev=>dev.Option.ExchangeKeys.Contains(exchnageKey)).ToList();
        }


        /// <summary>
        /// Вернуть все обмены использующие данный транспорт
        /// </summary>
        /// <param name="keyTransport"></param>
        /// <returns></returns>
        public IReadOnlyList<IExchange<TIn>> GetExchangesUsingTransport(KeyTransport keyTransport)
        {
           return _exchangeStorage.Values.Where(exch => exch.KeyTransport.Equals(keyTransport)).ToList();
        }


        public IExchange<TIn> GetExchange(string exchnageKey)
        {
            return _exchangeStorage.Get(exchnageKey);
        }


        public ITransportBackground GetBackground(KeyTransport keyTransport)
        {
            return _backgroundStorage.Get(keyTransport);
        }


        /// <summary>
        /// Создать устройство на базе optionAgregator.
        /// Созданное ус-во добавляется в StorageDevice. 
        /// Если для создания ус-ва нужно создать ОБМЕН и/или ТРАНСПОРТ, то созданные объекты тоже добавляются в StorageExchange или StorageTransport
        /// </summary>
        /// <param name="optionAgregator">Цепочка настроек одного устройства. Настройкам этим полностью доверяем (не валидируем).</param>
        /// <returns> Новое созданное ус-во, добавленное в хранилище</returns>
        public Device<TIn> BuildAndAddDevice(OptionAgregator optionAgregator)
        {
            var deviceOption = optionAgregator.DeviceOptions.First();
            if (_deviceStorage.IsExist(deviceOption.Name))
            {
                throw new StorageHandlerException($"Устройство с таким именем уже существует: {deviceOption.Name}");
            }

            //ДОБАВИТЬ НОВЫЙ ТРАНСПОРТ И БЕКГРАУНД ДЛЯ НЕГО-----------------------------------------------------------------------
            foreach (var spOption in optionAgregator.TransportOptions.SerialOptions)
            {
                var keyTransport = new KeyTransport(spOption.Port, TransportType.SerialPort);
                var sp = _transportStorage.Get(keyTransport);
                if (sp == null)
                {
                    sp = new SpWinSystemIo(spOption, keyTransport);
                    _transportStorage.AddNew(keyTransport, sp);
                    var bg = new HostingBackgroundTransport(keyTransport, spOption.AutoStartBg, spOption.DutyCycleTimeBg, _logger);
                    _backgroundStorage.AddNew(keyTransport, bg);
                }
            }
            foreach (var tcpIpOption in optionAgregator.TransportOptions.TcpIpOptions)
            {
                var keyTransport = new KeyTransport(tcpIpOption.Name, TransportType.TcpIp);
                var tcpIp = _transportStorage.Get(keyTransport);
                if (tcpIp == null)
                {
                    var bg = new HostingBackgroundTransport(keyTransport, tcpIpOption.AutoStartBg, tcpIpOption.DutyCycleTimeBg, _logger);
                    _backgroundStorage.AddNew(keyTransport, bg);
                    tcpIp = new TcpIpTransport(bg, tcpIpOption, keyTransport, _logger);
                    _transportStorage.AddNew(keyTransport, tcpIp);

                }
            }
            foreach (var httpOption in optionAgregator.TransportOptions.HttpOptions)
            {
                var keyTransport = new KeyTransport(httpOption.Name, TransportType.Http);
                var http = _transportStorage.Get(keyTransport);
                if (http == null)
                {
                    http = new HttpTransport(httpOption, keyTransport);
                    _transportStorage.AddNew(keyTransport, http);
                    var bg = new HostingBackgroundTransport(keyTransport, httpOption.AutoStartBg, httpOption.DutyCycleTimeBg, _logger);
                    _backgroundStorage.AddNew(keyTransport, bg);
                }
            }

            //ДОБАВИТЬ НОВЫЕ ОБМЕНЫ---------------------------------------------------------------------------
            foreach (var exchOption in optionAgregator.ExchangeOptions)
            {
                var exch = _exchangeStorage.Get(exchOption.Key);
                if (exch != null)
                    continue;

                var keyTransport = exchOption.KeyTransport;
                var bg = _backgroundStorage.Get(keyTransport);
                var transport = _transportStorage.Get(keyTransport);

                try
                {
                    var dataProvider = _dataProviderFactory[exchOption.Provider.Name](exchOption.Provider);
                    exch = new Exchange<TIn>(exchOption, transport, bg, dataProvider, _logger);
                    _exchangeStorage.AddNew(exchOption.Key, exch);
                }
                catch (Exception)
                {
                    throw new StorageHandlerException($"Провайдер данных не найденн в системе: {exchOption.Provider.Name}");
                } 
            }

            //ДОБАВИТЬ УСТРОЙСТВО--------------------------------------------------------------------------
            var excanges = _exchangeStorage.GetMany(deviceOption.ExchangeKeys).ToList();
            var device = new Device<TIn>(deviceOption, excanges, _eventBus, _produserUnionStorage, _logger);
            _deviceStorage.AddNew(device.Option.Name, device);
            return device;
        }



        /// <summary>
        /// Удалить устройство,
        /// Если использовались уникальные обмены, то удалить и их. 
        /// Если удаленный (уникальный) обмен использовал уникальный транспорт, то отсановить обмен и удалить транспорт.
        /// </summary>
        public async Task<Device<TIn>> RemoveDevice(string deviceName)
        {
            var device = GetDevice(deviceName);
            if (device == null)
                throw new StorageHandlerException($"Устройство с таким именем НЕ существует: {deviceName}");

            var exchangeKeys = _deviceStorage.Values.SelectMany(dev => dev.Option.ExchangeKeys).ToList();
            var keyTransports = _exchangeStorage.Values.Select(exc => exc.KeyTransport).ToList();
            foreach (var exchKey in device.Option.ExchangeKeys)
            {
                if (exchangeKeys.Count(key => key == exchKey) == 1)
                {
                    var removingExch = _exchangeStorage.Get(exchKey);
                    if (removingExch.CycleExchnageStatus != CycleExchnageStatus.Off)
                    {
                        removingExch.StopCycleExchange();
                    }
                    if (keyTransports.Count(tr => tr == removingExch.KeyTransport) == 1)
                    {
                        await RemoveAndStopTransport(removingExch.KeyTransport);
                    }
                    _exchangeStorage.Remove(exchKey);
                    removingExch.Dispose();
                }
            }

            //УДАЛИМ УСТРОЙСТВО
            _deviceStorage.Remove(deviceName);
            device.Dispose(); //???
            return device;
        }


        /// <summary>
        /// Вернуть транспорт по ключу
        /// </summary>
        public ITransport GetTransport(KeyTransport keyTransport)
        {
            var transport = _transportStorage.Get(keyTransport);
            return transport;
        }


        /// <summary>
        /// Ищет транспорт по ключу в нужном хранилище и Удаляет его.
        /// </summary>
        private async Task RemoveAndStopTransport(KeyTransport keyTransport)
        {
            var bg = _backgroundStorage.Get(keyTransport);
            if (bg.IsStarted)
            {
                _backgroundStorage.Remove(keyTransport);
                await bg.StopAsync(CancellationToken.None);
                bg.Dispose();
            }

            var transport = _transportStorage.Get(keyTransport);
            if (transport.IsCycleReopened)
            {
                transport.CycleReOpenedExecCancelation();
            }
            _transportStorage.Remove(keyTransport);
            transport.Dispose();
        }


        /// <summary>
        /// Вренуть продюссер ответа по имени
        /// </summary>
        public ProdusersUnion<TIn> GetProduserUnion(string produserName)
        {
            var produser = _produserUnionStorage.Get(produserName);
            return produser;
        }


        /// <summary>
        /// Вернуть всех продюссеров ответов.
        /// </summary>
        public IReadOnlyList<ProdusersUnion<TIn>> GetProduserUnions()
        {
            return _produserUnionStorage.Values.ToList();
        }


        /// <summary>
        /// Добавить или обновить ProdusersUnion в Storage
        /// </summary>
        public DictionaryCrudResult AddOrUpdateProduserUnion(string key, ProdusersUnion<TIn> value)
        {
            return _produserUnionStorage.IsExist(key) ?
                _produserUnionStorage.Update(key, value) :
                _produserUnionStorage.AddNew(key, value);
        }


        /// <summary>
        /// Добавить или обновить ProdusersUnion в Storage
        /// </summary>
        public DictionaryCrudResult RemoveProduserUnion(string key)
        {
            return _produserUnionStorage.IsExist(key) ? _produserUnionStorage.Remove(key) : DictionaryCrudResult.KeyNotExist;
        }
        #endregion
    }
}