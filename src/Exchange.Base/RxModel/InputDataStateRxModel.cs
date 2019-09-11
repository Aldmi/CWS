using Domain.Exchange.Services;

namespace Domain.Exchange.RxModel
{
    public class InputDataStateRxModel
    {
        public string KeyExchange { get;  }
        public InputDataState InputDataState { get; }



        #region ctor

        public InputDataStateRxModel(string keyExchange, InputDataState inputDataState)
        {
            KeyExchange = keyExchange;
            InputDataState = inputDataState;
        }

        #endregion
    }
}