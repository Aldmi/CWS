using Domain.Exchange.Enums;
using Domain.Exchange.Services;

namespace Domain.Exchange.RxModel
{
    public class InputDataStateRxModel
    {
        public string KeyExchange { get;  }
        public InputDataStatus InputDataStatus { get; }



        #region ctor

        public InputDataStateRxModel(string keyExchange, InputDataStatus inputDataStatus)
        {
            KeyExchange = keyExchange;
            InputDataStatus = inputDataStatus;
        }

        #endregion
    }
}