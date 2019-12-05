using Domain.Exchange.Enums;
using Domain.Exchange.Services;

namespace Domain.Exchange.RxModel
{
    public class CycleBehaviorStateRxModel
    {
        public string KeyExchange { get;  }
        public CycleBehaviorState CycleBehaviorState { get; }



        #region ctor
        public CycleBehaviorStateRxModel(string keyExchange, CycleBehaviorState cycleBehaviorState)
        {
            KeyExchange = keyExchange;
            CycleBehaviorState = cycleBehaviorState;
        }
        #endregion
    }
}