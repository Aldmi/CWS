using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.ProvidersOption;
using Domain.InputDataModel.Base.Response;
using Infrastructure.Transport.Base.DataProvidert;

namespace Domain.InputDataModel.Base.ProvidersAbstract
{
    public interface IDataProvider<TInput, TOutput> : ITransportDataProvider
    {
        InDataWrapper<TInput> InputData { get; set; }                //передача входных даных внешним кодом.
        TOutput OutputData { get; set; }                             //возврат выходных данных во внешний код.
        bool IsOutDataValid { get; }                                 //флаг валидности выходных данных (OutputData)
         
        string ProviderName { get;  }                                 //Название провайдера
        Dictionary<string, string> StatusDict{ get; }                 //Статус провайдера.
        int TimeRespone { get; }                                      //Время на ответ

        Task StartExchangePipeline(InDataWrapper<TInput> inData);                     //Запустить конвеер обмена. После окончания подготовки порции данных конвеером, срабатывает RaiseSendDataRx.
        Subject<IDataProvider<TInput, TOutput>> RaiseSendDataRx { get; }             //Событие отправки данных, в процессе обработки их конвеером.

        ProviderOption GetCurrentOptionRt();                                         //Вернуть спсиок текущих опций (опции могут быть поменены и отличатсч от опций из БД)

        //TODO:удалить SetCurrentOptionRt, обмен принимаетуже созданного провайдера (провайдер имммутабелен)
        bool SetCurrentOptionRt(ProviderOption optionNew);                           //Установить новые настройки для провайдера. (конкретный провайдер сам возьмет нужные ему настройки)

    }
}