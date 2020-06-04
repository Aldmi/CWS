using System.Linq;
using Domain.InputDataModel.Base.Response;
using Shared.Helpers;

namespace Domain.Device.Produser
{
    public class ProdusersUnionResponseConverter<TIn>
    {
        /// <summary>
        /// Конвертер для ответов.
        /// </summary>
        /// <param name="converterName"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public object Convert(string converterName, ResponsePieceOfDataWrapper<TIn> response)
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
                        Type= "boardData", 
                        response.DeviceName,
                        response.KeyExchange,
                        DataAction= response.DataAction.ToString("G"),
                        response.ExceptionExchangePipline,
                        response.IsValidAll,
                        response.TimeAction,
                        ResponsesItems = response.ResponsesItems.Select(item => new
                        {
                            item.RequestId,
                            item.StatusStr,
                            item.TransportException,

                            item.ProcessedItemsInBatch.StartItemIndex,
                            item.ProcessedItemsInBatch.BatchSize,
                            InseartedData = item.ProcessedItemsInBatch.ProcessedItems.Select(p=>p.InseartedData)
                        }).ToList()
                    };
                    break;

                case "Medium":
                    convert = new
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
                    convert = new
                    {
                        response.DeviceName,
                        response.ExceptionExchangePipline,
                        response.IsValidAll,
                        response.TimeAction,
                        ResponsesItems = response.ResponsesItems.Select(item => new
                        {
                            item.RequestId,
                            item.StatusStr,
                            Info = item.ResponseInfo?.ToString()
                        }).ToList()
                    };
                    break;

                case "Indigo":  //{"result": 1, "message": "", "DeviceName": "fff"}
                    var responsesItems = response.ResponsesItems.Select(item => new
                    {
                        item.RequestId,
                        item.StatusStr,
                        item.TransportException,
                        Info = item.ResponseInfo?.ToString()
                    }).ToList();
                    convert = new
                    {
                        response.DeviceName,
                        result = response.IsValidAll ? 1 : 0,
                        Message = HelpersJson.Serialize2RawJson(responsesItems)
                    };
                    break;
            }

            return convert;
        }


        /// <summary>
        /// конвертер для строковых сообщений
        /// </summary>
        /// <param name="converterName"></param>
        /// <param name="objectName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public object Convert(string converterName, string objectName, string message)
        {
            object converted = null;
            switch (converterName)
            {
                case "Full":
                case "Medium":
                case "Lite":
                    converted = new
                    {
                        DeviceName = objectName,
                        Message = message
                    };
                    break;

                case "RenderItems":
                    converted = new
                    {
                        Type="Message",
                        DeviceName = objectName,
                        Message = message
                    };
                    break;

                case "Indigo":  //{"result": 1, "message": "", "DeviceName": "fff"}
                    converted = new
                    {
                        result = 0,
                        DeviceName = objectName,
                        Message = message
                    };
                    break;
            }

            return converted;
        }
    }
}