using System;
using System.Collections.Specialized;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Produser.AbstractProduser.Enums;
using Infrastructure.Produser.AbstractProduser.Helpers;
using Infrastructure.Produser.AbstractProduser.Options;
using Infrastructure.Produser.AbstractProduser.RxModels;

namespace Infrastructure.Produser.AbstractProduser.AbstractProduser
{
    /// <summary>
    /// Базовый класс продюссера.
    /// Определяет стратегию отправки сообщений
    /// Определяет стратегию уничтожения объекта
    /// </summary>
    public abstract class BaseProduser<TOption> : IProduser<TOption> where TOption : BaseProduserOption
    {
        #region field
        private readonly TimeSpan _timeRequest;
        #endregion



        #region prop
        public TrottlingCounter TrottlingCounter { get; set; }
        public TOption Option { get; }
        #endregion



        #region Rx
        /// <summary>
        /// Rx событие Подключение/Отключение новго клиента к Produser
        /// </summary>
        public ISubject<ClientCollectionChangedRxModel> ClientCollectionChangedRx { get; } = new Subject<ClientCollectionChangedRxModel>();
        #endregion



        #region RxEventHandler
        protected void ClientCollectionChangedRxEh(NotifyCollectionChangedAction collectionChanged)
        {
            ClientCollectionChangedRx.OnNext(new ClientCollectionChangedRxModel(Option.Key, collectionChanged));
        }
        #endregion




        #region ctor
        protected BaseProduser(TOption baseOption)
        {
            _timeRequest = baseOption.TimeRequest;
            TrottlingCounter= new TrottlingCounter(baseOption.TrottlingQuantity);
            Option = baseOption;
        }
        #endregion



        #region Methode
        public async Task<Result<string, ErrorWrapper>> Send(object obj, ProduserSendingDataType dataType)
        {
            TrottlingCounter++;
            if (TrottlingCounter.IsTrottle)
                return Result.Failure<string, ErrorWrapper>(new ErrorWrapper(Option.Key, ResultError.Trottling));

            var cts = new CancellationTokenSource(_timeRequest);
            try
            {
                return dataType switch
                {
                    ProduserSendingDataType.Init => await SendInit(obj, cts.Token),
                    ProduserSendingDataType.BoardData => await SendBoardData(obj, cts.Token),
                    ProduserSendingDataType.Info => await SendInfo(obj, cts.Token),
                    ProduserSendingDataType.Warning => await SendWarning(obj, cts.Token),
                    _ => throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null),
                };
            }
            catch (TaskCanceledException)
            {
                return Result.Failure<string, ErrorWrapper>(new ErrorWrapper(Option.Key, ResultError.Timeout));
            }
            catch (Exception ex)
            {
                return Result.Failure<string, ErrorWrapper>(new ErrorWrapper(Option.Key, ResultError.SendException, ex));
            }
            finally
            {
                cts.Dispose();
                TrottlingCounter--;
            }
        }
        #endregion



        #region AbstractMembers
        protected abstract Task<Result<string, ErrorWrapper>> SendInit(object message, CancellationToken ct = default(CancellationToken));
        protected abstract Task<Result<string, ErrorWrapper>> SendBoardData(object message, CancellationToken ct = default(CancellationToken));
        protected abstract Task<Result<string, ErrorWrapper>> SendInfo(object message, CancellationToken ct = default(CancellationToken));
        protected abstract Task<Result<string, ErrorWrapper>> SendWarning(object message, CancellationToken ct = default(CancellationToken));
        #endregion



        #region Disposable
        public abstract void Dispose();
        #endregion
    }
}