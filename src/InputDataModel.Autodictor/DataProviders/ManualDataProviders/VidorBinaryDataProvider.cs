using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAL.Abstract.Entities.Options.Exchange.ProvidersOption;
using Domain.Exchange.DataProviderAbstract;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.Providers;
using Domain.InputDataModel.Base.Response;

namespace Domain.InputDataModel.Autodictor.DataProviders.ManualDataProviders
{
    public class VidorBinaryDataProvider : BaseDataProvider<AdInputType>, IExchangeDataProvider<AdInputType, ResponseInfo>
    {
        #region field

        private readonly ManualProviderOption _providerOption;

        #endregion




        #region ctor

        public VidorBinaryDataProvider(ProviderOption providerOption) : base(null, null)
        {
            _providerOption = providerOption.ManualProviderOption;
            if(_providerOption == null)
                throw new ArgumentNullException(providerOption.Name);
        }

        #endregion


        //public VidorBinaryDataProvider()
        //{
        //    ProviderName = "VidorBinaryDataProvider";
        //}

        public byte[] GetDataByte()
        {
            throw new System.NotImplementedException();
        }

        public bool SetDataByte(byte[] data)
        {
            throw new System.NotImplementedException();
        }

        public Stream GetStream()
        {
            throw new System.NotImplementedException();
        }

        public bool SetStream(Stream stream)
        {
            throw new System.NotImplementedException();
        }

        public string GetString()
        {
            throw new System.NotImplementedException();
        }

        public bool SetString(Stream stream)
        {
            throw new System.NotImplementedException();
        }

        public int CountGetDataByte { get; }
        public int CountSetDataByte { get; }
        Task IExchangeDataProvider<AdInputType, ResponseInfo>.StartExchangePipeline(InDataWrapper<AdInputType> inData)
        {
            return StartExchangePipeline(inData);
        }

        public Subject<IExchangeDataProvider<AdInputType, ResponseInfo>> RaiseSendDataRx { get; }

        public void SendDataIsCompleted()
        {
            throw new NotImplementedException();
        }

        public InDataWrapper<AdInputType> InputData { get; set; }
        InDataWrapper<AdInputType> IExchangeDataProvider<AdInputType, ResponseInfo>.InputData
        {
            get => InputData;
            set => InputData = value;
        }

        public ResponseInfo OutputData { get; set; }
        public bool IsOutDataValid { get; }
        public Subject<ResponseDataItem<AdInputType>> OutputDataChangeRx { get; }
        public string ProviderName { get; set; }
        public Dictionary<string, string> StatusDict { get; }
        public StringBuilder StatusString { get; set; }
        public string Message { get; }


        public Task StartExchangePipeline(InDataWrapper<AdInputType> inData)
        {
            throw new NotImplementedException();
        }

        public ProviderOption GetCurrentOptionRt()
        {
            throw new NotImplementedException();
        }

        public bool SetCurrentOptionRt(ProviderOption optionNew)
        {
            throw new NotImplementedException();
        }

        public int TimeRespone { get; }
        public CancellationTokenSource Cts { get; set; }

    }
}