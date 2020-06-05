using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Base.Response;
using Infrastructure.Produser.AbstractProduser.AbstractProduser;
using Infrastructure.Produser.AbstractProduser.Helpers;
using Infrastructure.Produser.AbstractProduser.Options;
using Shared.Helpers;
using ProduserUnionOption = Domain.Device.Repository.Entities.ResponseProduser.ProduserUnionOption;

namespace Domain.Device.Produser
{
    /// <summary>
    /// Объединение продюссеров.
    /// Под одним ключем находится Объединение продюссеров для отправки ответов.
    /// </summary>
    public class ProdusersUnion<TIn> : IDisposable
    {
        #region fields
        private readonly ProduserUnionOption _unionOption;
        private readonly ProdusersUnionResponseConverter<TIn> _responseConverter;
        private readonly ConcurrentDictionary<string, ProduserOwner> _produsersDict = new ConcurrentDictionary<string, ProduserOwner>();
        #endregion



        #region prop
        public ReadOnlyDictionary<string, IProduser<BaseProduserOption>> GetProduserDict => new ReadOnlyDictionary<string, IProduser<BaseProduserOption>>(_produsersDict.ToDictionary(d => d.Key, d => d.Value.Produser));
        public int GetProdusersCount => _produsersDict.Count;
        public string GetKey => _unionOption.Key;
        #endregion



        #region ctor
        public ProdusersUnion(ProduserUnionOption unionOption, ProdusersUnionResponseConverter<TIn> responseConverter)
        {
            _unionOption = unionOption;
            _responseConverter = responseConverter;
        }
        #endregion



        #region Methods
        /// <summary>
        /// Дгобавить продюссера.
        /// </summary>
        /// <param name="key">ключ</param>
        /// <param name="value">продюссер</param>
        /// <param name="owner">объект для управления временем жизни продюссера</param>
        public void AddProduser(string key, IProduser<BaseProduserOption> value, IDisposable owner)
        {
            _produsersDict[key] = new ProduserOwner { Produser = value, Owner = owner };
        }


        /// <summary>
        /// Удалить продюссера по ключу
        /// </summary>
        /// <param name="key">ключ</param>
        /// <returns></returns>
        public bool RemoveProduser(string key)
        {
            if (!_produsersDict.ContainsKey(key))
                return false;

            var owner = _produsersDict[key].Owner;
            var res = _produsersDict.TryRemove(key, out var val);
            owner.Dispose();
            return res;
        }


        /// <summary>
        /// Отправить всем продюссерам ответ на обмен порцией данных.
        /// </summary>
        public async Task<IList<Result<string, ErrorWrapper>>> SendResponse4AllProdusers(ResponsePieceOfDataWrapper<TIn> response, string invokerName = null)
        {
            var converted = _responseConverter.Convert(_unionOption.ConverterName, response);
            var tasks = _produsersDict.Values.Select(produserOwner => produserOwner.Produser.Send(converted, invokerName)).ToList();
            var results = await Task.WhenAll(tasks);
            return results;
        }


        /// <summary>
        /// Отправить продюсеру по ключу, ответ на обмен порцией данных.
        /// </summary>
        public async Task<Result<string, ErrorWrapper>> SendResponse4Produser(string key, ResponsePieceOfDataWrapper<TIn> response, string invokerName = null)
        {
            if (!_produsersDict.ContainsKey(key))
                throw new KeyNotFoundException(key);

            var converted = _responseConverter.Convert(_unionOption.ConverterName, response);
            var result = await _produsersDict[key].Produser.Send(converted, invokerName);
            return result;
        }

        /// <summary>
        /// Отправить продюсеру по ключу, коллекцию данных.
        /// </summary>
        public async Task<Result<string, ErrorWrapper>> SendResponseCollection4Produser(string key, IEnumerable<ResponsePieceOfDataWrapper<TIn>> responseCollection, string invokerName = null)
        {
            if (!_produsersDict.ContainsKey(key))
                throw new KeyNotFoundException(key);

            var converted = responseCollection.Select(response=> _responseConverter.Convert(_unionOption.ConverterName, response)).ToList();  
            var result = await _produsersDict[key].Produser.Send(converted, invokerName);
            return result;
        }


        /// <summary>
        /// Отправить всем продюссерам сообщение об ошибки.
        /// </summary>
        public async Task<IList<Result<string, ErrorWrapper>>> SendMessage4AllProdusers(object obj, string invokerName = null)
        {
            //var messageConverted = _responseConverter.Convert(_unionOption.ConverterName, objectName, message);
            var tasks = _produsersDict.Values.Select(produserOwner => produserOwner.Produser.Send(obj, invokerName)).ToList();
            var results = await Task.WhenAll(tasks);
            return results;
        }

        #endregion



        #region Disposable

        public void Dispose()
        {
            foreach (var produserOwner in _produsersDict.Values)
            {
                produserOwner.Owner.Dispose();
            }
        }

        #endregion



        #region NestedClasses
        private class ProduserOwner
        {
            public IProduser<BaseProduserOption> Produser { get; set; }
            public IDisposable Owner { get; set; }
        }
        #endregion
    }
}