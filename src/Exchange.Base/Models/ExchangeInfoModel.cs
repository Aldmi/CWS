namespace Domain.Exchange.Models
{
    public class ExchangeInfoModel
    {
        public readonly string keyExchange;
        public readonly bool IsConnect;
        public readonly bool IsOpen;


        public ExchangeInfoModel(string keyExchange, bool isConnect, bool isOpen)
        {
            keyExchange = keyExchange;
            IsConnect = isConnect;
            IsOpen = isOpen;
        }
    }
}