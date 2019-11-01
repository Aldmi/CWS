using Autofac.Features.OwnedInstances;
using Domain.InputDataModel.Base.InData;
using Infrastructure.Storages;

namespace Domain.Exchange
{
    public class ExchangeStorage<TIn> : BaseStorage<string, Owned<IExchange<TIn>>> where TIn : InputTypeBase
    {

    }
}