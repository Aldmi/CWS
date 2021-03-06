﻿using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Transport.Base.DataProvidert;
using Infrastructure.Transport.Base.RxModel;
using Shared.Enums;
using Shared.Types;

namespace Infrastructure.Transport.Base.Abstract
{
    public interface ITransport : ISupportKeyTransport, IDisposable
    {
        bool IsOpen { get; }                                                                   // ФЛАГ ОТКРЫТИЯ ПОРТА ТРАНСОРТА
        string StatusString { get; }                                                           // СОСТОЯНИЕ ТРАНСПОРТА
        StatusDataExchange StatusDataExchange { get; }                                         // СТАТУС ПОСЛЕДНЕГО ОБМЕНА
        bool IsCycleReopened { get; }                                                          // ФЛАГ НАХОЖДЕНИЯ ПОРТА В ЦИКЛЕ ПЕРЕОТКРЫТИЯ  

        ISubject<IsOpenChangeRxModel> IsOpenChangeRx { get; }                                  // СОБЫТИЕ ОТКРЫТИЯ/ЗАКРЫТИЯ ЛИНИИ КОММУНИКАЦИИ УСТРОЙСТВА
        ISubject<StatusDataExchangeChangeRxModel> StatusDataExchangeChangeRx { get; }          // СОБЫТИЕ СМЕНЫ СОСТОЯНИЯ СТАТУСА ОБМЕНА 
        ISubject<StatusStringChangeRxModel> StatusStringChangeRx { get; }                      // СОБЫТИЕ СМЕНЫ СТРОКИ СТАТУСА ПОРТА

        Task CycleReOpenedExec();                                                              // ПОПЫТКИ ЦИКЛИЧЕСКОГО ПЕРЕОТКРЫТИЯ ПОРТА (С УНИЧТОЖЕНИЕМ ТЕКУЩЕГО ЭКЗЕМПЛЯРА ПОРТА)
        void CycleReOpenedExecCancelation();                                                   // ОТМЕНА ЦИКЛИЧЕСКОГО ПЕРЕОТКРЫТИЯ
        Task<StatusDataExchange> DataExchangeAsync(ITransportDataProvider dataProvider, CancellationToken ct);   // ЗАПРОС/ОТВЕТ через переданный ITransportDataProvider
    }
}