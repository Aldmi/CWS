using Infrastructure.Storages;

namespace Domain.Device.Produser
{
    public class ProduserUnionStorage<TIn> : BaseStorage<string, ProdusersUnion<TIn>>
    {
        
    }
}