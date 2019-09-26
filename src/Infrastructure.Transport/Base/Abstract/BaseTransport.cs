using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Background.Abstarct;
using Infrastructure.Background.Enums;
using Infrastructure.Transport.Base.DataProvidert;
using Infrastructure.Transport.Base.RxModel;
using Serilog;
using Shared.Enums;
using Shared.Types;


namespace Infrastructure.Transport.Base.Abstract
{
    public abstract class BaseTransport : ITransport
    {
        #region fields

        private const int TimeCycleReOpened = 500; //Большее время занимает ожидание ответа от ConnectAsync()
        private readonly ITransportBackground _transportBg;
        protected readonly ILogger Logger;

        #endregion



        #region ctor

        protected BaseTransport(ITransportBackground transportBg, KeyTransport keyTransport, ILogger logger)
        {
            _transportBg = transportBg;
            KeyTransport = keyTransport;
            Logger = logger;
        }

        #endregion



        #region prop

        public KeyTransport KeyTransport { get; }
        public bool IsOpen { get; private set; }   //TODO: IsOpen и IsCycleReopened заменить на enum
        public bool IsCycleReopened { get; private set; }
        public string StatusString { get; protected set; }
        public StatusDataExchange StatusDataExchange { get; protected set; }

        #endregion



        #region Rx

        public ISubject<IsOpenChangeRxModel> IsOpenChangeRx { get; } = new Subject<IsOpenChangeRxModel>();                                        //СОБЫТИЕ ИЗМЕНЕНИЯ ОТКРЫТИЯ/ЗАКРЫТИЯ ПОРТА
        public ISubject<StatusDataExchangeChangeRxModel> StatusDataExchangeChangeRx { get; } = new Subject<StatusDataExchangeChangeRxModel>();    //СОБЫТИЕ ИЗМЕНЕНИЯ ОТПРАВКИ ДАННЫХ ПО ПОРТУ
        public ISubject<StatusStringChangeRxModel> StatusStringChangeRx { get; } = new Subject<StatusStringChangeRxModel>();                      //СОБЫТИЕ ИЗМЕНЕНИЯ СТРОКИ СТАТУСА ПОРТА

        #endregion



        #region Methode

        /// <summary>
        /// Циклическое открытие подключения
        /// </summary>
        private CancellationTokenSource _cycleReOpenedCts;
        public async Task CycleReOpenedExec()
        {
            if (IsCycleReopened)
            {
                Logger.Error("{Type} KeyTransport: \"{KeyTransport}\" ", "ТРАНСПОРТ УЖЕ НАХОДИТСЯ В ЦИКЛЕ ПЕРЕОТКРЫТИЯ", KeyTransport);
                return;
            }

            //дожидаемся Перевода БГ в режим ожидания.
            var resStendBy= await _transportBg.PutOnStendBy();
            switch (resStendBy)
            {
                case StatusBackground.StandByStarting:
                    Logger.Error("{Type} KeyTransport: \"{KeyTransport}\" ", "БЕКГРАУНД НЕ ЗАКОНЧИЛ ПЕРЕВОД В РЕЖИМ ОЖИДАНИЯ ГОТОВНОСТИ (StandByStarted)", KeyTransport);
                    return;

                case StatusBackground.StandByStarted:
                    //Запускаем задачу циклического переоткрытия соединения.
                    _cycleReOpenedCts = new CancellationTokenSource();
                    var resReOpened = await Task.Run(async () => await CycleReOpened(_cycleReOpenedCts.Token), _cycleReOpenedCts.Token);
                    //Успешный реконнект. Перевести БГ в режим работы.
                    if (resReOpened)
                    {
                        _transportBg.PutOnWork();
                    }
                    _cycleReOpenedCts?.Dispose();
                    _cycleReOpenedCts = null;
                    break;
            }
        }


        /// <summary>
        /// Отмена задачи циклического открытия подключения
        /// </summary>
        public void CycleReOpenedExecCancelation()
        {
            if (IsCycleReopened)
            {
                _cycleReOpenedCts.Cancel();
            }
        }


        private async Task<bool> CycleReOpened(CancellationToken ct)
        {
            IsCycleReopened = true;
            IsOpen = false;
            try
            {
                while (!ct.IsCancellationRequested && !IsOpen)
                {
                    IsOpen = await ReOpen();
                    if (!IsOpen)
                    {
                        Logger.Warning("{Type} {KeyTransport}", $"Connect для транспорта НЕ ОТКРЫТ: {StatusString}", KeyTransport);
                        await Task.Delay(TimeCycleReOpened, ct);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Logger.Information("{Type} {KeyTransport}", "ОТМЕНА ПЕРЕОТКРЫТИЯ СОЕДИНЕНИЯ ДЛЯ ТРАНСПОРТА: ", KeyTransport);
                IsCycleReopened = false;
                return false;
            }

            Logger.Information("{Type} {KeyTransport}", "Connect для транспорта ОТКРЫТ: ", KeyTransport);
            IsCycleReopened = false;
            return true;
        }

        #endregion



        #region abstracts

        protected abstract Task<bool> ReOpen();
        public abstract Task<StatusDataExchange> DataExchangeAsync(ITransportDataProvider dataProvider, CancellationToken ct); //TODO: Постараться выделить шаблонный метод (по аналогии с CycleReOpenedExec)
        protected abstract void DisposeTransport();

        #endregion



        #region Disposable

        public void Dispose()
        {
            _cycleReOpenedCts?.Cancel();
            _cycleReOpenedCts?.Dispose();
            DisposeTransport();
        }

        #endregion
    }
}