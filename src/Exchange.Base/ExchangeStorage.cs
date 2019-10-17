using Autofac.Features.OwnedInstances;
using Infrastructure.Storages;

namespace Domain.Exchange
{
    public class ExchangeStorage<TIn> : BaseStorage<string, Owned<IExchange<TIn>>>
    {

    }
}