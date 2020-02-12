using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.ProvidersOption;
using Domain.InputDataModel.Base.Response;

namespace Domain.InputDataModel.Autodictor.ProvidersSpecial
{
    public class VidorBinaryDataProvider : BaseDataProvider<AdInputType>, IDataProvider<AdInputType, ResponseInfo>
    {
        #region field

        private readonly ManualProviderOption _providerOption;
        private Subject<ProviderResult<AdInputType>> _raiseSendDataRx;

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

        public bool SetString(string stream)
        {
            throw new NotImplementedException();
        }

        public bool SetString(Stream stream)
        {
            throw new System.NotImplementedException();
        }

        public int CountGetDataByte { get; }
        public int CountSetDataByte { get; }


        public Task StartExchangePipelineAsync(InDataWrapper<AdInputType> inData, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        Subject<ProviderResult<AdInputType>> IDataProvider<AdInputType, ResponseInfo>.RaiseSendDataRx => _raiseSendDataRx;

        public Subject<IDataProvider<AdInputType, ResponseInfo>> RaiseSendDataRx { get; }

        public void SendDataIsCompleted()
        {
            throw new NotImplementedException();
        }

        public InDataWrapper<AdInputType> InputData { get; set; }

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

        public ProviderOption GetCurrentOption()
        {
            throw new NotImplementedException();
        }

        public bool SetCurrentOptionRt(ProviderOption optionNew)
        {
            throw new NotImplementedException();
        }

        public int TimeRespone { get; }
        public CancellationTokenSource Cts { get; set; }

        public override void Dispose()
        {
            _raiseSendDataRx?.Dispose();
            RaiseSendDataRx?.Dispose();
            OutputDataChangeRx?.Dispose();
            Cts?.Dispose();
        }
    }
}