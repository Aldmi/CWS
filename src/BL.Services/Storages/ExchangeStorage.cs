using Domain.Exchange;
using Infrastructure.Storages;

namespace App.Services.Storages
{
    public class ExchangeStorage<TIn> : BaseStorage<string, IExchange<TIn>>
    {

    }
}