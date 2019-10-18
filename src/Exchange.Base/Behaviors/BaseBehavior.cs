using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Domain.Exchange.Repository.Entities;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.Response;
using Infrastructure.Background.Abstarct;
using Serilog;
using Shared.Collections;
using Shared.Enums;
using Shared.Types;

namespace Domain.Exchange.Behaviors
{
    public abstract class BaseBehavior<TIn> where TIn : InputTypeBase
    {
        #region field
        private const int MaxDataInQueue = 5;
        protected readonly ITransportBackground TransportBackground;
        protected readonly Func<InDataWrapper<TIn>, CancellationToken, Task<ResponsePieceOfDataWrapper<TIn>>> PieceOfDataSender;
        protected readonly ILogger Logger;
        protected readonly LimitConcurrentQueueWithoutDuplicate<InDataWrapper<TIn>> DataQueue;
        #endregion



        #region prop
        public string KeyExchange { get; }
        public bool IsFullDataQueue => DataQueue.IsFullLimit;
        #endregion



        #region ctor
        protected BaseBehavior(string keyExchange,
            ITransportBackground transportBackground,
            QueueMode queueMode,
            Func<InDataWrapper<TIn>, CancellationToken, Task<ResponsePieceOfDataWrapper<TIn>>> pieceOfDataSender,
            ILogger logger)
        {
            KeyExchange = keyExchange;
            TransportBackground = transportBackground;
            PieceOfDataSender = pieceOfDataSender;
            Logger = logger;
            DataQueue = new LimitConcurrentQueueWithoutDuplicate<InDataWrapper<TIn>>(queueMode, MaxDataInQueue);
        }
        #endregion



        #region Rx
        public ISubject<ResponsePieceOfDataWrapper<TIn>> ResponseReadyRx { get; } = new Subject<ResponsePieceOfDataWrapper<TIn>>();
        #endregion
    }
}