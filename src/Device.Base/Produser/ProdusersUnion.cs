using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AbstractProduser.AbstractProduser;
using AbstractProduser.Helpers;
using AbstractProduser.Options;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Base.Response;
using Newtonsoft.Json;
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
        private readonly ConcurrentDictionary<string, ProduserOwner> _produsersDict = new ConcurrentDictionary<string, ProduserOwner>();

        #endregion



        #region prop

        public ReadOnlyDictionary<string, IProduser<BaseProduserOption>> GetProduserDict => new ReadOnlyDictionary<string, IProduser<BaseProduserOption>>(_produsersDict.ToDictionary(d => d.Key, d => d.Value.Produser));
        public int GetProdusersCount => _produsersDict.Count;
        public string GetKey => _unionOption.Key;

        #endregion



        #region ctor

        public ProdusersUnion(ProduserUnionOption unionOption)
        {
            _unionOption = unionOption;
        }

        #endregion



        #region Methods

        public void AddProduser(string key, IProduser<BaseProduserOption> value, IDisposable owner)
        {
            _produsersDict[key] = new ProduserOwner { Produser = value, Owner = owner };
        }


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
        /// Отправить всем продюссерам
        /// </summary>
        public async Task<IList<Result<string, ErrorWrapper>>> SendAll(ResponsePieceOfDataWrapper<TIn> response, string invokerName = null)
        {
            var message = ConvertResponse(_unionOption.ConverterName, response);
            var tasks = _produsersDict.Values.Select(produserOwner => produserOwner.Produser.Send(message, invokerName)).ToList();
            var results = await Task.WhenAll(tasks);
            return results;
        }


        /// <summary>
        /// Отправить продюсеру по ключу
        /// </summary>
        public async Task<Result<string, ErrorWrapper>> Send(string key, ResponsePieceOfDataWrapper<TIn> response, string invokerName = null)
        {
            if (!_produsersDict.ContainsKey(key))
                throw new KeyNotFoundException(key);

            var message = Convert2RawJson(response);
            var result = await _produsersDict[key].Produser.Send(message, invokerName);
            return result;
        }


        //TODO: ConvertResponse вынести в отдельный сервис. 
        private static string ConvertResponse(string converterName, ResponsePieceOfDataWrapper<TIn> response)
        {
            object convertedResp = null;
            switch (converterName)
            {
                case "Full":
                    convertedResp = response;
                    break;

                case "Medium":
                    convertedResp = new
                    {
                        response.DeviceName,
                        response.DataAction,
                        response.ExceptionExchangePipline,
                        response.IsValidAll,
                        response.TimeAction,
                        ResponsesItems = response.ResponsesItems.Select(item => new
                        {
                            item.RequestId,
                            item.Status,
                            item.StatusStr,
                            item.TransportException,
                            item.ResponseInfo
                        }).ToList()
                    };
                    break;

                case "Lite":
                    convertedResp = new
                    {
                        response.DeviceName,
                        response.ExceptionExchangePipline,
                        response.IsValidAll,
                        response.TimeAction,
                        ResponsesItems = response.ResponsesItems.Select(item => new
                        {
                            item.RequestId,
                            item.StatusStr,
                            item.TransportException,
                            item.ResponseInfo.StronglyTypedResponse
                        }).ToList()
                    };
                    break;

                case "Indigo":  //{"result": 1, "message": "", "DeviceName": "fff"}
                    var responsesItems = response.ResponsesItems.Select(item => new
                    {
                        item.RequestId,
                        item.StatusStr,
                        item.TransportException,
                        item.ResponseInfo.StronglyTypedResponse
                    }).ToList();
                    convertedResp = new
                    {
                        response.DeviceName,
                        result = response.IsValidAll ? 1 : 0,
                        Message = Convert2RawJson(responsesItems)
                    };
                    break;
            }

            var rawJson = Convert2RawJson(convertedResp);
            return rawJson;
        }



        /// <summary>
        ///Конвертировать в JSON. Raw -для машин, без отступов  
        /// </summary>
        private static string Convert2RawJson(object response) //TODO: вынести в Shared
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,                     //Отступы дочерних элементов НЕТ
                NullValueHandling = NullValueHandling.Ignore      //Игнорировать пустые теги
            };
            try
            {
                var jsonResp = JsonConvert.SerializeObject(response, settings);
                return jsonResp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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