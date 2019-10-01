using System.Linq;
using Domain.InputDataModel.Base.Response;
using Shared.Helpers;

namespace Domain.Device.Produser
{
    public class ProdusersUnionResponseConverter<TIn>
    {
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
                        Message = HelpersJson.Serialize2RawJson(responsesItems)
                    };
                    break;
            }

            var rawJson = HelpersJson.Serialize2RawJson(convertedResp);
            return rawJson;
        }
    }
}