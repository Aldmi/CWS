﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Produser.AbstractProduser.AbstractProduser;
using Infrastructure.Produser.AbstractProduser.Helpers;
using Infrastructure.Produser.AbstractProduser.Options;
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
        /// Отправить всем продюссерам.
        /// </summary>
        public async Task<IList<Result<string, ErrorWrapper>>> Send4AllProdusers(ProduserData<TIn> produserData)
        {
            var converted = _responseConverter.Convert(_unionOption.ConverterName, produserData);
            var dataType = produserData.DataType;
            var tasks = _produsersDict.Values.Select(produserOwner => produserOwner.Produser.Send(converted, dataType)).ToList();
            var results = await Task.WhenAll(tasks);
            return results;
        }


        /// <summary>
        /// Отправить продюсеру по ключу.
        /// </summary>
        public async Task<Result<string, ErrorWrapper>> Send4Produser(string key, ProduserData<TIn> produserData)
        {
            if (!_produsersDict.ContainsKey(key))
                throw new KeyNotFoundException(key);

            var dataType = produserData.DataType;
            var converted = _responseConverter.Convert(_unionOption.ConverterName, produserData);
            var result = await _produsersDict[key].Produser.Send(converted, dataType);
            return result;
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