using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

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



        #region Methode

        public bool AddClient(string key, T value)
        {
           var res= _clientsInfos.TryAdd(key, value);
           return res;
        }


        public bool RemoveClient(string key)
        {
            var res = _clientsInfos.TryRemove(key, out var value);
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