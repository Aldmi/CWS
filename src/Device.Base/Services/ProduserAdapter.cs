using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reactive.Linq;
using Domain.Device.Produser;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.Response;
using Infrastructure.Produser.AbstractProduser.RxModels;
using MoreLinq.Extensions;
using Serilog;

namespace Domain.Device.Services
{
    public class ProduserAdapter<TIn> : IDisposable where TIn : InputTypeBase
    {
        #region fields
        private readonly ProduserUnionStorage<TIn> _produserUnionStorage;
        private readonly ILogger _logger;
        private readonly string _key;
        private readonly List<IDisposable> _disposeProdusersEventHandlers = new List<IDisposable>();
        #endregion


        #region ctor
        public ProduserAdapter(string key, ProduserUnionStorage<TIn> produserUnionStorage, ILogger logger) //TODO: передавать еще функцию получения Init данных
        {
            _produserUnionStorage = produserUnionStorage;
            _logger = logger;
            _key = key;
        }
        #endregion




        /// <summary>
        /// Подписка на событие от продюссеров
        /// </summary>
        public bool SubscrubeOnProdusersEvents()
        {
            var produser = GetProduser();

            produser.GetProduserDict.Values.ForEach(prop =>
            {
                _disposeProdusersEventHandlers.Add(prop.ClientCollectionChangedRx.Where(p => p.NotifyCollectionChangedAction == NotifyCollectionChangedAction.Add).Subscribe(AddNewProdussersClientRxEh));
            });

            return true;
        }


        /// <summary>
        /// Отписка от событий обменов.
        /// </summary>
        public void UnsubscrubeOnProdusersEvents()
        {
            _disposeProdusersEventHandlers.ForEach(d => d.Dispose());
        }


        private void AddNewProdussersClientRxEh(ClientCollectionChangedRxModel rxModel)
        {
            SendInitData2ConcreteProduder(rxModel.Key, new ResponsePieceOfDataWrapper<TIn>
            {
                DeviceName = "DDD",
                KeyExchange = "KeyExchange111"
            }).GetAwaiter().GetResult();
        }







        private ProdusersUnion<TIn> GetProduser()
        {
            var produser = _produserUnionStorage.Get(_key);
            if (produser == null)
            {
                //_logger.Error($"Продюссер по ключу {_key} НЕ НАЙДЕНН для Устройства= {Option.Name}");
                return null;
            }
            return produser;
        }



        #region Disposable
        public void Dispose()
        {
            UnsubscrubeOnProdusersEvents();
        }
        #endregion
    }
}