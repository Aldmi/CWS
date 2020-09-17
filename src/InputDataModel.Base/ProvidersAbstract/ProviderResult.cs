using System;
using System.Collections.Generic;
using System.IO;
using Domain.InputDataModel.Base.Response.ResponseInfos;
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
        #endregion


        #region prop
        public Dictionary<string, string> StatusDict { get; }
        public int TimeRespone => _transfer.Response.Option.TimeRespone;         //Время на ответ
        public BaseResponseInfo OutputData { get; private set; }
        public bool IsOutDataValid { get; private set; }
        public ProviderStatus ProviderStatus { get; }

        /// <summary>
        /// Результат работы провайдера, обработанные и выставленные в протокол данные из InputData
        /// </summary>
        public ProcessedItemsInBatch<TIn> ProcessedItemsInBatch => _transfer.Request.ProcessedItemsInBatch;
        #endregion


        #region ctor
        public ProviderResult(ProviderTransfer<TIn> transfer, ProviderStatus providerStatus)
        {
            ProviderStatus = providerStatus;
            _transfer = transfer;
            StatusDict = new Dictionary<string, string>();//TODO: убрать
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
            var validator = _transfer.Response.Validator;
            var respInfo = validator.Validate(data);
            IsOutDataValid = respInfo.IsOutDataValid;
            OutputData = respInfo;
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
            var validator = _transfer.Response.Validator;
            var respInfo = validator.Validate(stringResponse);
            IsOutDataValid = respInfo.IsOutDataValid;
            OutputData = respInfo;
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

    }


    public class ProviderStatus
    {
        public readonly string SendingUnitName;

        public ProviderStatus(string sendingUnitName)
        {
            if(string.IsNullOrEmpty(sendingUnitName))
                throw new ArgumentException("sendingUnitName не может быть NULL или Empty");

            SendingUnitName = sendingUnitName;
        }
    }
}