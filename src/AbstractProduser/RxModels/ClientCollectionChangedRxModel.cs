using System.Collections.Specialized;

namespace Infrastructure.Produser.AbstractProduser.RxModels
{
    public class ClientCollectionChangedRxModel
    {
        public readonly string Key;
        public readonly NotifyCollectionChangedAction NotifyCollectionChangedAction;


        public ClientCollectionChangedRxModel(string key, NotifyCollectionChangedAction notifyCollectionChangedAction)
        {
            Key = key;
            NotifyCollectionChangedAction = notifyCollectionChangedAction;
        }
    }
}