using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using Domain.InputDataModel.Base.Response;
using Infrastructure.Transport.Base.DataProvidert;
using Shared.Extensions;
using Shared.Helpers;

namespace Domain.InputDataModel.Base.ProvidersAbstract
{
    /// <summary>
    /// Результат работы провайдера.
    /// Связующий объект между провайдером и транспортом.
    /// Провайдер создает ProviderTransfer и на базе него создает ProviderCore.
    /// ProviderCore Реализует все возможные варианта обмена даныыми и проверки ответов для всех транспортов через реализацию ITransportDataProvider.
    /// </summary>
    /// <typeparam name="TIn">тип входных данных для провайдера.</typeparam>
    public class ProviderResult<TIn> : ITransportDataProvider
    {
        #region field
        private readonly ProviderTransfer<TIn> _transfer;
        private readonly IStronglyTypedResponseFactory _stronglyTypedResponseFactory;
        #endregion


        #region prop
        public Dictionary<string, string> StatusDict { get; }
        public int TimeRespone => _transfer.Response.Option.TimeRespone;         //Время на ответ
        public int CountSetDataByte => _transfer.Response.Option.Lenght;        //Кол-во принимаемых байт в ответе
        public InDataWrapper<TIn> InputData =>  new InDataWrapper<TIn> { Datas = _transfer.BatchedData?.ToList(), Command = _transfer.Command}; 
        public ResponseInfo OutputData { get; private set; }
        public bool IsOutDataValid { get; private set; }
        /// <summary>
        /// Результат работы провайдера, обработанные и выставленные в протокол данные из InputData
        /// </summary>
        public ProcessedItemsInBatch<TIn> ProcessedItemsInBatch => _transfer.Request.ProcessedItemsInBatch;
        #endregion


        #region ctor
        public ProviderResult(ProviderTransfer<TIn> transfer, IDictionary<string, string> providerStatusDict, IStronglyTypedResponseFactory stronglyTypedResponseFactory)
        {
            _transfer = transfer;
            StatusDict = providerStatusDict == null ? new Dictionary<string, string>() : new Dictionary<string, string>(providerStatusDict);
            _stronglyTypedResponseFactory = stronglyTypedResponseFactory;
        }
        #endregion


        #region Byte[]
        public byte[] GetDataByte()
        {
            var stringRequset = _transfer.Request.StrRepresent.Str;
            var format = _transfer.Request.StrRepresent.Format;
            var resultBuffer = stringRequset.ConvertString2ByteArray(format); //Преобразовываем КОНЕЧНУЮ строку в массив байт
            StatusDict["GetDataByte.Request"] = $"[{stringRequset}] Lenght= {stringRequset.Length}  Format={format}";
            StatusDict["GetDataByte.RequestBase"] = _transfer.Request.EqualStrRepresent ? null : $"[{_transfer.Request.StrRepresentBase.Str}]  Lenght= {_transfer.Request.StrRepresentBase.Str.Length}   Format= {_transfer.Request.StrRepresentBase.Format}";
            StatusDict["GetDataByte.ByteRequest"] = $"{ resultBuffer.ArrayByteToString("X2")} Lenght= {resultBuffer.Length}";
            StatusDict["TimeResponse"] = $"{TimeRespone}";
            return resultBuffer;
        }

        public bool SetDataByte(byte[] data)
        {
            var stringResponseRef = _transfer.Response.StrRepresent.Str;
            var format = _transfer.Response.StrRepresent.Format;
            if (data == null)
            {
                IsOutDataValid = false;
                OutputData = new ResponseInfo
                {
                    ResponseData = null,
                    Encoding = format,
                    IsOutDataValid = IsOutDataValid
                };
                return false;
            }
            var stringResponse = data.ArrayByteToString(format);
            //Создать строго типизитрованный ответ на базе строки сырого ответа
            var stronglyTypedResponse = CreateStronglyTypedResponseByOption(_transfer.Response.Option.StronglyTypedName, stringResponse);
            IsOutDataValid = (stringResponse == stringResponseRef); //TODO: как лутчше сравнивать строки???
            OutputData = new ResponseInfo
            {
                ResponseData = stringResponse,
                Encoding = format,
                IsOutDataValid = IsOutDataValid,
                StronglyTypedResponse = stronglyTypedResponse
            };
            var diffResp = (!IsOutDataValid) ? $"ПринятоБайт/ОжидаемБайт= {data.Length}/{_transfer.Response.Option.Lenght}" : string.Empty;
            StatusDict["SetDataByte.StringResponse"] = $"{stringResponseRef} ?? {stringResponse}   diffResp=  {diffResp}";
            return IsOutDataValid;
        }
        #endregion


        #region String
        public string GetString()
        {
            var stringRequset = _transfer.Request.StrRepresent.Str;
            return stringRequset;
        }

        public bool SetString(string stringResponse)
        {
            var stringResponseRef = _transfer.Response.StrRepresent.Str;
            var format = _transfer.Response.StrRepresent.Format;
            if (stringResponse == null)
            {
                IsOutDataValid = false;
                OutputData = new ResponseInfo
                {
                    ResponseData = null,
                    Encoding = format,
                    IsOutDataValid = IsOutDataValid
                };
                return false;
            }
            //Создать строго типизитрованный ответ на базе строки сырого ответа
            var stronglyTypedResponse = CreateStronglyTypedResponseByOption(_transfer.Response.Option.StronglyTypedName, stringResponse);
            IsOutDataValid = (stringResponse == stringResponseRef); //TODO: как лутчше сравнивать строки???
            OutputData = new ResponseInfo
            {
                ResponseData = stringResponse,
                Encoding = format,
                IsOutDataValid = IsOutDataValid,
                StronglyTypedResponse = stronglyTypedResponse
            };
            var diffResp = (!IsOutDataValid) ? $"ПринятоСимволов/ОжидаемСимволов= {stringResponse.Length}/{_transfer.Response.Option.Lenght}" : string.Empty;
            StatusDict["SetDataByte.StringResponse"] = $"{stringResponseRef} ?? {stringResponse}   diffResp=  {diffResp}";
            return IsOutDataValid;
        }
        #endregion


        #region Stream
        public Stream GetStream()
        {
            throw new NotImplementedException();
        }

        public bool SetStream(Stream stream)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region OtherMethode
        /// <summary>
        ///Если указанно имя типа для ответоа, то мы его создаем через фабрику.
        /// </summary>
        private StronglyTypedRespBase CreateStronglyTypedResponseByOption(string stronglyTypedName, string stringResponse)
        {
            StronglyTypedRespBase stronglyTypedResponse = null;
            StatusDict["SetDataByte.StronglyTypedResponse"] = null;
            if (!string.IsNullOrEmpty(_transfer.Response.Option.StronglyTypedName))
            {
                try
                {
                    stronglyTypedResponse = _stronglyTypedResponseFactory.CreateStronglyTypedResponse(stronglyTypedName, stringResponse);
                    StatusDict["SetDataByte.StronglyTypedResponse"] = stronglyTypedResponse.ToString();
                }
                catch (NotSupportedException ex)
                {
                    StatusDict["SetDataByte.StronglyTypedResponse"] = $"ОШИБКА= {ex}";
                }
            }
            return stronglyTypedResponse;
        }
        #endregion
    }
}