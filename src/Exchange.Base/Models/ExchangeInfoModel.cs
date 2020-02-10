namespace Domain.Exchange.Models
{
    public class ExchangeInfoModel
    {
        public readonly string KeyExchange;
        public readonly bool IsConnect;
        public readonly bool IsOpen;


        public ExchangeInfoModel(string keyExchange, bool isConnect, bool isOpen)
        {
            KeyExchange = keyExchange;
            IsConnect = isConnect;
            IsOpen = isOpen;
        }
    }
}