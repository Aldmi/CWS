using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using CSharpFunctionalExtensions;
using Domain.Exchange.Behaviors;
using Domain.Exchange.Enums;
using Domain.Exchange.RxModel;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.Response;
using Infrastructure.Transport.Base.RxModel;
using Shared.Types;
using ProviderOption = Domain.InputDataModel.Base.ProvidersOption.ProviderOption;

namespace Domain.Exchange
{
    /// <summary>
    /// УНИВЕРСАЛЬНЫЙ ОБМЕН ДАННЫМИ СО ВСЕМИ УСТРОЙСТВАМИ.
    /// </summary>
    public interface IExchange<T> : ISupportKeyTransport, IDisposable where T : InputTypeBase
    {
        #region ByOption
        string KeyExchange { get; }
        string ProviderName { get; }
        int NumberErrorTrying { get;}                                           //Кол-во ошибочных запросов до переоткрытия соединения. IsConnect=false. ReOpenTransport()
        int NumberTimeoutTrying { get; }                                        //Кол-во запросов без ответов (таймаут ответа).IsConnect=false. ПЕРЕОТКРЫТИЕ НЕ ПРОИСХОДИТ.
        #endregion


        #region dataProvider
        ProviderOption GetProviderOption { get; }
        Result SetNewProvider(ProviderOption option);
        #endregion


        #region StateExchange
        bool IsOpen { get; }                                                      //Соединение открыто
        bool IsCycleReopened { get; }                                             //Соединение НЕ открыто, находится в состоянии цикличесих попыток ОТКРЫТИЯ (флаг нужен т.к. цикл переоткрытия можно отменить и тогда будет IsOpen= false, IsCycleReopened = false )
        bool IsConnect { get; }                                                   //Устройсвто на связи по открытому соединению (определяется по правильным ответам от ус-ва)
        bool IsStartedTransportBg { get; }                                        //Запущен бекграунд на транспорте
        LastSendPieceOfDataRxModel<T> LastSendData { get; }                                    //Последние отосланные данные 
        #endregion


        #region Behaviors
        CycleBehavior<T> CycleBehavior { get; }
        OnceBehavior<T> OnceBehavior { get; }
        CommandBehavior<T> CommandBehavior { get; }
        #endregion


        #region SendData
        void SendCommand(Command4Device command);                                  //однократно выполняемая команда
        void SendOneTimeData(IEnumerable<T> inData, string directHandlerName);     //однократно отсылаемые данные (если указанны правила, то только для этих правил)
        void SendCycleTimeData(IEnumerable<T> inData, string directHandlerName);   //циклически отсылаемые данные
        #endregion


        #region ExchangeRx
        ISubject<ConnectChangeRxModel> IsConnectChangeRx { get; }                   //СОБЫТИЕ СМЕНЫ КОННЕКТА IsConnect. МЕНЯЕТСЯ В ПРОЦЕССЕ ОБМЕНА.
        ISubject<LastSendPieceOfDataRxModel<T>> LastSendDataChangeRx { get; }        //СОБЫТИЕ ИЗМЕНЕНИЯ ПОСЛЕД ОТПРАВЕЛННЫХ ДАННЫХ ProcessedItemsInBatch.
        #endregion


        #region TransportRx
        ISubject<IsOpenChangeRxModel> IsOpenChangeTransportRx { get; }                                  //ПРОКИНУТОЕ СОБЫТИЕ ТРАНСПОРТА. ОТКРЫТИЯ/ЗАКРЫТИЯ ПОДКЛЮЧЕНИЯ.
        ISubject<StatusDataExchangeChangeRxModel> StatusDataExchangeChangeTransportRx { get; }          //ПРОКИНУТОЕ СОБЫТИЕ ТРАНСПОРТА. СМЕНЫ СОСТОЯНИЯ СТАТУСА ОБМЕНА ДАННЫМИ.
        ISubject<StatusStringChangeRxModel> StatusStringChangeTransportRx { get; }                      //ПРОКИНУТОЕ СОБЫТИЕ ТРАНСПОРТА. СМЕНЫ СТРОКИ СТАТУСА ПОРТА.
        #endregion         
    }
}