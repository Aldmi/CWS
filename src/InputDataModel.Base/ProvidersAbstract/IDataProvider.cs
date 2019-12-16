using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.ProvidersOption;
using Domain.InputDataModel.Base.Response;
using Infrastructure.Transport.Base.DataProvidert;

namespace Domain.InputDataModel.Base.ProvidersAbstract
{
    public interface IDataProvider<TInput, TOutput> : IDisposable
    {
        string ProviderName { get;  }                                 //Название провайдера
        Dictionary<string, string> StatusDict{ get; }                 //Статус провайдера.

        Task StartExchangePipelineAsync(InDataWrapper<TInput> inData, CancellationToken ct);                   //Запустить конвеер обмена. После окончания подготовки порции данных конвеером, срабатывает RaiseSendDataRx.
        Subject<ProviderResult<TInput>> RaiseSendDataRx { get; }                   //Событие отправки данных, в процессе обработки их конвеером.

        ProviderOption GetCurrentOption();                                           //Вернуть список текущих опций (опции могут быть изменены и отличатся от опций из БД)
    }
}