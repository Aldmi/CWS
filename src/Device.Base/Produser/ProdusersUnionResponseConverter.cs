﻿using System;
using System.Linq;
using Domain.InputDataModel.Base.Response;
using Infrastructure.Produser.AbstractProduser.AbstractProduser;
using Shared.Helpers;

namespace Domain.Device.Produser
{
    public class ProdusersUnionResponseConverter<TIn>
    {
        /// <summary>
        /// Конвертер для ответов.
        /// </summary>
        /// <param name="converterName"></param>
        /// <param name="produserData"></param>
        /// <returns></returns>
        public object Convert(string converterName, ProduserData<TIn> produserData)
        {
            var strDataType = produserData.DataType.ToString("G");
            switch (produserData.DataType)
            {
                case ProduserSendingDataType.Init:
                    var finishExchFullState = produserData.InitDatas.Select(initdata => new
                    {
                        initdata.DeviceName,
                        initdata.KeyExchange,
                        initdata.IsOpen,
                        initdata.IsConnect,
                        initdata.IsCycleReopened,
                        initdata.IsStartedTransportBg,
                        Data = initdata.Data == null ? null : ConvertResponsePieceOfDataWrapper(converterName, initdata.Data)
                    }).ToList();
                    return new
                    {
                        Type = strDataType,
                        ExchangesState = finishExchFullState
                    };

                case ProduserSendingDataType.BoardData:
                    var finishResponse = produserData.Data == null ? null : ConvertResponsePieceOfDataWrapper(converterName, produserData.Data);
                    return new
                    {
                        Type = strDataType,
                        Data = finishResponse
                    };

                case ProduserSendingDataType.Info:
                case ProduserSendingDataType.Warning:
                    return new
                    {
                        Type = strDataType,
                        produserData.MessageObj
                    };

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private static object ConvertResponsePieceOfDataWrapper(string converterName, ResponsePieceOfDataWrapper<TIn> response)
        {
            object convert = null;
            switch (converterName)
            {
                case "Full":
                    convert = response;
                    break;

                case "RenderItems":
                    convert = new
                    {
                        response.DeviceName,
                        response.KeyExchange,
                        DataAction = response.DataAction.ToString("G"),
                        response.Evaluation,
                        response.TimeAction,
                        LinearizationBatchs = response.LinearizationBatchs().Select(pair=>pair)
                    };
                    break;

                case "RenderItemsFull":
                    convert = new
                    {
                        response.DeviceName,
                        response.KeyExchange,
                        DataAction = response.DataAction.ToString("G"),
                        response.Evaluation,
                        response.TimeAction,
                        ResponsesItems = response.ResponsesItems.Select(respItem => new
                        {
                            respItem.ProviderStatus.SendingUnitName,
                            respItem.StatusStr,
                            respItem.TransportException,

                            respItem.ProcessedItemsInBatch?.StartItemIndex,
                            respItem.ProcessedItemsInBatch?.BatchSize,
                            InseartedData = respItem.ProcessedItemsInBatch?.ProcessedItems.Select(p => p.InseartedData).ToList()
                        }).ToList(),

                        LinearizationBatchs = response.LinearizationBatchs().Select(pair => pair)
                    };
                    break;

                //Для передачи ответов от устройства
                case "OnlyResponseData":
                    convert = new
                    {
                        response.DeviceName,
                        response.KeyExchange,
                        ResponsesItems = response.ResponsesItems.Select(item => new
                        {
                            item.StatusStr,
                            obj = item.ResponseInfo.GetResponseData()
                        }).ToList()
                    };
                    break;

                case "Medium":
                    convert = new
                    {
                        response.DeviceName,
                        response.DataAction,
                        response.ExceptionExchangePipline,
                        response.Evaluation.IsValidAll,
                        response.TimeAction,
                        ResponsesItems = response.ResponsesItems.Select(item => new
                        {
                            item.Status,
                            item.StatusStr,
                            item.TransportException,
                            item.ResponseInfo
                        }).ToList()
                    };
                    break;

                case "Lite":
                    convert = new
                    {
                        response.DeviceName,
                        response.ExceptionExchangePipline,
                        response.Evaluation.IsValidAll,
                        response.TimeAction,
                        ResponsesItems = response.ResponsesItems.Select(item => new
                        {
                            item.StatusStr,
                            Info = item.ResponseInfo?.ToString()
                        }).ToList()
                    };
                    break;

                case "Indigo":  //{"result": 1, "message": "", "DeviceName": "fff"}
                    var responsesItems = response.ResponsesItems.Select(item => new
                    {
                        item.StatusStr,
                        item.TransportException,
                        Info = item.ResponseInfo?.ToString()
                    }).ToList();
                    convert = new
                    {
                        response.DeviceName,
                        result = response.Evaluation.IsValidAll ? 1 : 0,
                        Message = HelpersJson.Serialize2RawJson(responsesItems)
                    };
                    break;
            }

            return convert;
        }
    }
}