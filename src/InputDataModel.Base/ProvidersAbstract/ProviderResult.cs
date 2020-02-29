﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using Domain.InputDataModel.Base.Response;
using Domain.InputDataModel.Base.Response.ResponseInfos;
using Domain.InputDataModel.Base.Response.ResponseValidators;
using Infrastructure.Transport.Base.DataProvidert;
using Shared.Extensions;
using Shared.Helpers;
using Shared.Types;

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
        /// <summary>
        /// Результат работы провайдера, обработанные и выставленные в протокол данные из InputData
        /// </summary>
        public ProcessedItemsInBatch<TIn> ProcessedItemsInBatch => _transfer.Request.ProcessedItemsInBatch;
        #endregion


        #region ctor
        public ProviderResult(ProviderTransfer<TIn> transfer, IDictionary<string, string> providerStatusDict)
        {
            _transfer = transfer;
            StatusDict = providerStatusDict == null ? new Dictionary<string, string>() : new Dictionary<string, string>(providerStatusDict);
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
}