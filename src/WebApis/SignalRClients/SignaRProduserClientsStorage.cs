using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Subjects;
using Domain.Exchange.RxModel;

namespace WebApiSwc.SignalRClients
{
    /// <summary>
    /// Хранит информацию о клиентах подключенных к SignaR.
    /// Concurrentsy
    /// </summary>
    public class SignaRProduserClientsStorage<T> where T : SignaRClientsInfoBase
    {
        private readonly ConcurrentDictionary<string, T> _clientsInfos  = new ConcurrentDictionary<string, T>();


        #region prop
        public List<T> GetClientsInfo => _clientsInfos.Values.ToList();
        public bool Any => _clientsInfos.Count > 0;
        #endregion


        #region Rx
        /// <summary>
        /// СОБЫТИЕ изменения коллекции Клиентов.
        /// </summary>
        public ISubject<NotifyCollectionChangedAction> CollectionChangedRx { get; } = new Subject<NotifyCollectionChangedAction>();
        #endregion



        #region Methode

        public bool AddClient(string key, T value)
        {
           var res= _clientsInfos.TryAdd(key, value);
           if (res)
           {
               CollectionChangedRx.OnNext(NotifyCollectionChangedAction.Add);
           }
           return res;
        }


        public bool RemoveClient(string key)
        {
            var res = _clientsInfos.TryRemove(key, out var value);
            if (res)
            {
                CollectionChangedRx.OnNext(NotifyCollectionChangedAction.Remove);
            }
            return res;
        }


        public bool GetClient(string key, out T value)
        {
            var res = _clientsInfos.TryGetValue(key, out var val);
            value = val;
            return res;
        }

        #endregion
    }
}