using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Domain.Device.Produser;
using Domain.Exchange.Models;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.Response;
using Infrastructure.Produser.AbstractProduser.RxModels;
using MoreLinq.Extensions;
using Serilog;

namespace Domain.Device.Services
{
    /// <summary>
    /// Адаптирует Использование ProduserUnion для Device.
    /// </summary>
    public class ProduserAdapter<TIn> : IDisposable where TIn : InputTypeBase
    {
        #region fields
        private readonly ProduserUnionStorage<TIn> _produserUnionStorage;
        private readonly ILogger _logger;
        private readonly string _key;
        private readonly string _deviceName;
        private readonly Func<List<ExchangeFullState<TIn>>> _getCurrentStatus4AllExchanges;
        private readonly List<IDisposable> _disposeProdusersEventHandlers = new List<IDisposable>();
        #endregion


        #region prop
        /// <summary>
        /// Наличие ProduserUnion в ProduserUnionStorage по ключу.
        /// </summary>
        public bool IsExistProduserUnion => !string.IsNullOrEmpty(_key) && _produserUnionStorage.ContainsKey(_key);
        #endregion


        #region ctor
        /// <param name="deviceOptions"> Ключ продюссера в produserUnionStorage.   Название устройства, использующего ProduserAdapter</param>
        /// <param name="getCurrentStatus4AllExchanges">Функция получения текущего статуса обменов</param>
        /// <param name="produserUnionStorage">Словарь продюсеров</param>
        public ProduserAdapter((string key, string deviceName) deviceOptions, Func<List<ExchangeFullState<TIn>>> getCurrentStatus4AllExchanges, ProduserUnionStorage<TIn> produserUnionStorage, ILogger logger)
        {
            _produserUnionStorage = produserUnionStorage;
            _logger = logger;
            _key = deviceOptions.key;
            _deviceName = deviceOptions.deviceName;
            _getCurrentStatus4AllExchanges = getCurrentStatus4AllExchanges;
        }
        #endregion


        
        #region RxEventHandlers
        private void AddNewProdusersClientRxEh(ClientCollectionChangedRxModel rxModel)
        {
            var initData = _getCurrentStatus4AllExchanges();
            SendInitData2ProduderAsync(rxModel.Key, initData).Wait(); //TODO: переделать RxEh под async
        }
        #endregion



        #region Methods
        /// <summary>
        /// Подписка на событие от продюссеров.
        /// </summary>
        public bool SubscrubeOnEvents()
        {
            if (!IsExistProduserUnion)
                return false;

            var produser = GetProduser();
            produser.GetProduserDict.Values.ForEach(prop =>
            {
                _disposeProdusersEventHandlers.Add(prop.ClientCollectionChangedRx.Where(p => p.NotifyCollectionChangedAction == NotifyCollectionChangedAction.Add).Subscribe(AddNewProdusersClientRxEh));
            });
            return true;
        }


        /// <summary>
        /// Отписка от событий от продюссеров.
        /// </summary>
        public void UnsubscrubeOnEvents()
        {
            _disposeProdusersEventHandlers.ForEach(d => d.Dispose());
        }


        /// <summary>
        /// Отправить ответ от обмена на ProduserUnion.
        /// </summary>
        public async Task SendData2ProduderUnionAsync(ResponsePieceOfDataWrapper<TIn> response)
        {
            if(!IsExistProduserUnion)
                return;

            var produser = GetProduser();
            var data = ProduserData<TIn>.CreateBoardData(response);
            var results = await produser.Send4AllProdusers(data);
            foreach (var (isSuccess, isFailure, _, error) in results)
            {
                if (isFailure)
                    _logger.Error($"Ошибки отправки ответов для Устройства= {response.DeviceName} через ProduderUnion = {_key}  {error}");

                if (isSuccess)
                    _logger.Information($"ОТПРАВКА ОТВЕТОВ УСПЕШНА для устройства {response.DeviceName} через ProduderUnion = {_key}");
            }
        }


        /// <summary>
        /// Отправить предупреждение о неверной работе устройства.
        /// </summary>
        public async Task SendWarningAsync(object warningObj)
        {
            var data = ProduserData<TIn>.CreateWarning(warningObj);
            if (!string.IsNullOrEmpty(_key))
                await SendMessage2ProduderUnion(data);
        }


        /// <summary>
        /// Отправить информационное сообщение
        /// </summary>
        public async Task SendInfoAsync(object warningObj)
        {
            if (!IsExistProduserUnion)
                return;

            var data = ProduserData<TIn>.CreateInfo(warningObj);
            await SendMessage2ProduderUnion(data);
        }


        /// <summary>
        /// Отправить сообщение от устройства на ProduserUnion.
        /// </summary>
        private async Task SendMessage2ProduderUnion(ProduserData<TIn> data)
        {
            if (!IsExistProduserUnion)
                return;

            var produser = GetProduser();
            var results = await produser.Send4AllProdusers(data);
            foreach (var (isSuccess, isFailure, _, error) in results)
            {
                if (isFailure)
                    _logger.Error($"Ошибки отправки сообщений для Устройства= {_deviceName} через ProduderUnion = {_key}  {error}");

                if (isSuccess)
                    _logger.Information($"ОТПРАВКА сообщений УСПЕШНА для устройства {_deviceName} через ProduderUnion = {_key}");
            }
        }


        /// <summary>
        /// Отправить данные ИНИЦИАЛИЗАЦИИ от устройства на Produser по ключу.
        /// </summary>
        private async Task SendInitData2ProduderAsync(string key, List<ExchangeFullState<TIn>> initDataCollection)
        {
            if (!IsExistProduserUnion)
                return;

            var produser = GetProduser();
            var data = ProduserData<TIn>.CreateInit(initDataCollection);
            var (_, isFailure, _, error) = await produser.Send4Produser(key, data);

            if (isFailure)
                _logger.Error($"Ошибки отправки сообщений для Устройства= {_deviceName} через ProduderUnion = {_key}  {error}");
            else
                _logger.Information($"ОТПРАВКА сообщений УСПЕШНА для устройства {_deviceName} через ProduderUnion = {_key}");
        }


        private ProdusersUnion<TIn> GetProduser()
        {
            var produser = _produserUnionStorage.Get(_key);
            if (produser == null)
            {
                _logger.Error($"Продюссер по ключу {_key} НЕ НАЙДЕНН для Устройства= {_deviceName}");
                return null;
            }
            return produser;
        }
        #endregion



        #region Disposable
        public void Dispose()
        {
            UnsubscrubeOnEvents();
        }
        #endregion
    }
}