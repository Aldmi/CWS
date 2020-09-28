using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.ProvidersOption;

namespace Domain.InputDataModel.Base.ProvidersAbstract
{
    public interface IDataProvider<TInput> : IDisposable
    {
        string ProviderName { get;  }                                 //Название провайдера
        Task StartExchangePipelineAsync(InDataWrapper<TInput> inData, CancellationToken ct);                   //Запустить конвеер обмена. После окончания подготовки порции данных конвеером, срабатывает RaiseProviderResultRx.
        Subject<ProviderResult<TInput>> RaiseProviderResultRx { get; }                   //Событие отправки данных, в процессе обработки их конвеером.
        ProviderOption GetCurrentOption();                                           //Вернуть список текущих опций (опции могут быть изменены и отличатся от опций из БД)
    }
}