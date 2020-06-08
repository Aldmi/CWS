using Domain.InputDataModel.Base.Response;

namespace Domain.Exchange.Models
{
    public class ExchangeFullState<TIn>
    {
        public string KeyExchange { get; }
        public string DeviceName { get; }
        public bool IsOpen { get; }
        public bool IsCycleReopened { get; }
        public bool IsStartedTransportBg { get; }
        public bool IsConnect { get; }
        public ResponsePieceOfDataWrapper<TIn> Datas { get; }


        public ExchangeFullState(string keyExchange, string deviceName, bool isOpen, bool isCycleReopened, bool isStartedTransportBg, bool isConnect, ResponsePieceOfDataWrapper<TIn> datas)
        {
            KeyExchange = keyExchange;
            DeviceName = deviceName;
            IsOpen = isOpen;
            IsCycleReopened = isCycleReopened;
            IsStartedTransportBg = isStartedTransportBg;
            IsConnect = isConnect;
            Datas = datas;
        }
    }
}