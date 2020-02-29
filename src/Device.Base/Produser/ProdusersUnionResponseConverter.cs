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
        public  string Convert(string converterName, ResponsePieceOfDataWrapper<TIn> response)
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
                    convertedResp = new
                    {
                        response.DeviceName,
                        result = response.IsValidAll ? 1 : 0,
                        Message = HelpersJson.Serialize2RawJson(responsesItems)
                    };
                    break;
            }

            var rawJson = HelpersJson.Serialize2RawJson(convertedResp);
            return rawJson;
        }


        /// <summary>
        /// конвертер для строковых сообщений
        /// </summary>
        /// <param name="converterName"></param>
        /// <param name="objectName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public string Convert(string converterName, string objectName, string message)
        {
            object convertedMessage = null;
            switch (converterName)
            {
                case "Full":
                case "Medium":
                case "Lite":
                    convertedMessage = new
                    {
                        DeviceName = objectName,
                        Message = message
                    };
                    break;

                case "Indigo":  //{"result": 1, "message": "", "DeviceName": "fff"}
                    convertedMessage = new
                    {
                        result = 0,
                        DeviceName = objectName,
                        Message = message
                    };
                    break;
            }

            var rawJson = HelpersJson.Serialize2RawJson(convertedMessage);
            return rawJson;
        }

    }
}