using System;
using System.IO;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.Response.ResponseInfos;
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
        private readonly ProviderStatus.Builder _providerStatusBuilder; //передаем именно Builder, чтобы ProviderResult мог добавить нужные свойства к ProviderStatus.
        #endregion


        #region prop
        public int TimeRespone => _transfer.Response.Option.TimeRespone;         //Время на ответ
        public BaseResponseInfo ResponseInfo { get; private set; }
        public bool IsOutDataValid { get; private set; }
        public ProviderStatus ProviderStatus { get; private set; }
        /// <summary>
        /// Результат работы провайдера, обработанные и выставленные в протокол данные из InputData
        /// </summary>
        public ProcessedItemsInBatch<TIn> ProcessedItemsInBatch => _transfer.Request.ProcessedItemsInBatch;
        #endregion


        #region ctor
        public ProviderResult(ProviderTransfer<TIn> transfer, ProviderStatus.Builder providerStatusBuilder)
        {
            _providerStatusBuilder = providerStatusBuilder;
            _transfer = transfer;
        }
        #endregion


        #region Byte[]
        public byte[] GetDataByte()
        {
            var stringRequset = _transfer.Request.StrRepresent.Str;
            var format = _transfer.Request.StrRepresent.Format;
            var resultBuffer = stringRequset.ConvertString2ByteArray(format); //Преобразовываем КОНЕЧНУЮ строку в массив байт

            // StatusDict["GetDataByte.ByteRequest"] = $"{ resultBuffer.ArrayByteToString("X2")} Lenght= {resultBuffer.Length}";
            _providerStatusBuilder.SetByteRequest(new StringRepresentation(resultBuffer.ArrayByteToString("X2"), "HEX")); //TODO: указывать еще ращзмер буфера в байтах enght= {resultBuffer.Length}
            ProviderStatus= _providerStatusBuilder.Build();
            return resultBuffer;
        }

        public bool SetDataByte(byte[] data)
        {
            var validator = _transfer.Response.Validator;
            var respInfo = validator.Validate(data);
            IsOutDataValid = respInfo.IsOutDataValid;
            ResponseInfo = respInfo;
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
            ResponseInfo = respInfo;
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
        public string SendingUnitName { get; }
        public Command4Device Command { get; }
        public StringRepresentation Request { get; }
        public StringRepresentation RequestBase { get; }
        public StringRepresentation ByteRequest { get; }
        public int TimeResponse { get; }
        public StringRepresentation GetRequestBaseIfHeNotEqualRequest => Request.Equals(RequestBase) ? null : RequestBase;


        #region ctor
        private ProviderStatus(Builder builder)
        {
            if (string.IsNullOrEmpty(builder.SendingUnitName))
                throw new ArgumentException("sendingUnitName не может быть NULL или Empty");

            if (builder.Request == null)
                throw new ArgumentException("Request не может быть NULL");

            if (builder.RequestBase == null)
                throw new ArgumentException("RequestBase не может быть NULL");

            if (builder.ByteRequest == null)
                throw new ArgumentException("ByteRequest не может быть NULL");

            if (builder.TimeResponse <= 0)
                throw new ArgumentException("TimeResponse не может быть отрицательным");

            SendingUnitName = builder.SendingUnitName;
            Command = builder.Command;
            Request = builder.Request;
            RequestBase = builder.RequestBase;
            ByteRequest = builder.ByteRequest;
            TimeResponse = builder.TimeResponse;
        }
        #endregion


        public class Builder
        {
            internal string SendingUnitName { get; }
            internal StringRepresentation Request { get;  }
            internal StringRepresentation RequestBase { get;  }
            internal int TimeResponse { get; }
            internal Command4Device Command { get; private set; }
            internal StringRepresentation ByteRequest { get; private set; }
 

            public Builder(string sendingUnitName, StringRepresentation request, StringRepresentation requestBase, int timeResponse)
            {
                SendingUnitName = sendingUnitName;
                Request = request;
                RequestBase = requestBase;
                TimeResponse = timeResponse;
            }


            public Builder SetCommand(Command4Device command)
            {
                Command = command;
                return this;
            }

            public Builder SetByteRequest(StringRepresentation byteRequest)
            {
                ByteRequest = byteRequest;
                return this;
            }

            public ProviderStatus Build()
            {
                return new ProviderStatus(this);
            }
        }
    }
}