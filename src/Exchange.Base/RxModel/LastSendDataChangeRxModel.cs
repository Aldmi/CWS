using Domain.InputDataModel.Base.InData;

namespace Domain.Exchange.RxModel
{
    public class LastSendDataChangeRxModel<T>
    {
        public InDataWrapper<T> LastSendData { get; set;} 
        public string KeyExchange { get; set; }
    }
}