using System.Linq;
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
        public ResponsePieceOfDataWrapper<TIn> Data { get; }


        public ExchangeFullState(string keyExchange, string deviceName, bool isOpen, bool isCycleReopened, bool isStartedTransportBg, bool isConnect, ResponsePieceOfDataWrapper<TIn> data)
        {
            KeyExchange = keyExchange;
            DeviceName = deviceName;
            IsOpen = isOpen;
            IsCycleReopened = isCycleReopened;
            IsStartedTransportBg = isStartedTransportBg;
            IsConnect = isConnect;
            Data = data;
        }

        //TODO: добавить проесеты преобразования к различным dto (полная форма, сокрпшеная и тд)

        public object GetPresenterFull()
        {
            var presenter = new
            {
                DeviceName,
                KeyExchange,
                IsConnect,
                transport = new
                {
                    IsOpen,
                    IsCycleReopened,
                    IsStartedTransportBg,
                },

                data = Data == null ? (object)"НЕТ ДАННЫХ ДЛЯ ОБМЕНА" :
                    new
                    {
                        Data.TimeAction,
                        Data.IsValidAll,
                        Data.MessageDict,
                        Data.ResponsesItems
                    }
            };
            return presenter;
        }

        public object GetPresenterWithoutProcessedItemsInBatch()
        {
            var presentert = new
            {
                DeviceName,
                KeyExchange,
                IsConnect,
                transport = new
                {
                    IsOpen,
                    IsCycleReopened,
                    IsStartedTransportBg,
                },

                data = Data == null ? (object)"НЕТ ДАННЫХ ДЛЯ ОБМЕНА" :
                    new
                    {
                        Data.TimeAction,
                        Data.IsValidAll,
                        Data.MessageDict,
                        ResponsesItems = Data.ResponsesItems.Select(r=> new
                        {
                            r.MessageDict,
                            r.Status,
                            r.StatusStr,
                            r.ResponseInfo
                        }).ToArray()
                    }
            };

            return presentert;
        }
    }
}