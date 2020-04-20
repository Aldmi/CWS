using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Services.Config;
using App.Services.Exceptions;
using Autofac.Features.OwnedInstances;
using Domain.Device;
using Domain.Device.Produser;
using Domain.Device.Repository.Entities;
using Domain.Exchange;
using Domain.Exchange.Enums;
using Domain.Exchange.Repository.Entities;
using Domain.InputDataModel.Base.InData;
using Infrastructure.Background;
using Infrastructure.Background.Abstarct;
using Infrastructure.Background.Concrete.HostingBackground;
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
        private readonly Func<string, ExchangeOption, ITransport, ITransportBackground, Owned<IExchange<TIn>>> _exchangeFactory;
        private readonly Func<DeviceOption, IEnumerable<IExchange<TIn>>, Owned<Device<TIn>>> _deviceFactory;
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
            Func<string, ExchangeOption, ITransport, ITransportBackground, Owned<IExchange<TIn>>> exchangeFactory,
            Func<DeviceOption, IEnumerable<IExchange<TIn>>,  Owned<Device<TIn>>> deviceFactory,
            AppConfigWrapper appConfigWrapper,
            ILogger logger)
        {
            _transportStorage = transportStorage;
            _produserUnionStorage = produserUnionStorage;
            _backgroundStorage = backgroundStorage;
            _exchangeStorage = exchangeStorage;
            _deviceStorage = deviceStorage;
            _exchangeFactory = exchangeFactory;
            _deviceFactory = deviceFactory;
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
            var device = _deviceStorage.Get(deviceName)?.Value;
            return device;
        }


        public IReadOnlyList<Device<TIn>> GetDevices()
        {
            return _deviceStorage.Values.Select(owned=> owned.Value).ToList();
        }


        /// <summary>
        /// Получить список устройств, использующих этот обмен по ключу
        /// </summary>
        /// <param name="exchnageKey"></param>
        /// <returns></returns>
        public IReadOnlyList<Device<TIn>> GetDevicesUsingExchange(string exchnageKey)
        {
            return _deviceStorage.Values.Select(owned => owned.Value).Where(dev=>dev.Option.ExchangeKeys.Contains(exchnageKey)).ToList();
        }


        /// <summary>
        /// Вернуть все обмены использующие данный транспорт
        /// </summary>
        /// <param name="keyTransport"></param>
        /// <returns></returns>
        public IReadOnlyList<IExchange<TIn>> GetExchangesUsingTransport(KeyTransport keyTransport)
        {
           return _exchangeStorage.Values.Select(owned => owned.Value).Where(exch => exch.KeyTransport.Equals(keyTransport)).ToList();
        }


        public IExchange<TIn> GetExchange(string exchnageKey)
        {
            return _exchangeStorage.Get(exchnageKey)?.Value;
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
                    exch= _exchangeFactory(deviceOption.Name, exchOption, transport, bg);
                    _exchangeStorage.AddNew(exchOption.Key, exch);
                }
                catch (Exception)
                {
                    throw new StorageHandlerException($"Провайдер данных не найденн в системе: {exchOption.Provider.Name}");
                } 
            }

            //ДОБАВИТЬ УСТРОЙСТВО--------------------------------------------------------------------------
            var excanges = _exchangeStorage.GetMany(deviceOption.ExchangeKeys).Select(owned =>owned.Value).ToList();
            var deviceOwner = _deviceFactory(deviceOption, excanges);
            _deviceStorage.AddNew(deviceOwner.Value.Option.Name, deviceOwner);
            return deviceOwner.Value;
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

            var exchangeKeys = _deviceStorage.Values.Select(owned => owned.Value).SelectMany(dev => dev.Option.ExchangeKeys).ToList();
            var keyTransports = _exchangeStorage.Values.Select(owned => owned.Value.KeyTransport).ToList();
            foreach (var exchKey in device.Option.ExchangeKeys)
            {
                if (exchangeKeys.Count(key => key == exchKey) == 1)
                {
                    var removingExch = _exchangeStorage.Get(exchKey).Value;
                    if (removingExch.CycleBehavior.CycleBehaviorState != CycleBehaviorState.Off)
                    {
                        removingExch.CycleBehavior.StopCycleExchange();
                    }
                    if (keyTransports.Count(tr => tr == removingExch.KeyTransport) == 1)
                    {
                        await RemoveAndStopTransport(removingExch.KeyTransport);
                    }
                    _exchangeStorage.Remove(exchKey);
                }
            }
            //УДАЛИМ УСТРОЙСТВО
            _deviceStorage.Remove(deviceName);
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
                await bg.StopAsync(CancellationToken.None);
                _backgroundStorage.Remove(keyTransport);
            }

            var transport = _transportStorage.Get(keyTransport);
            if (transport.IsCycleReopened)
            {
                transport.CycleReOpenedExecCancelation();
            }
            _transportStorage.Remove(keyTransport);
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