using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.Response;
using Infrastructure.Background.Abstarct;
using Serilog;
using Shared.Collections;

namespace Domain.Exchange.Behaviors
{
    public class CommandBehavior<TIn> : BaseBehavior<TIn> where TIn : InputTypeBase
    {
        #region ctor
        public CommandBehavior(string keyExchange,
            ITransportBackground transportBackground,
            Func<DataAction, InDataWrapper<TIn>, CancellationToken, Task<ResponsePieceOfDataWrapper<TIn>>> pieceOfDataSender,
            ILogger logger) : base(keyExchange, transportBackground, QueueMode.QueueExtractLastItem, pieceOfDataSender, logger)
        {
        }
        #endregion



        #region Methode
        /// <summary>
        /// Отправить команду. аналог однократно выставляемой функции.
        /// </summary>
        /// <param name="command"></param>
        public void SendCommand(Command4Device command)
        {
            if (command == Command4Device.None)
                return;

            var dataWrapper = new InDataWrapper<TIn> { Command = command };
            DataQueue.Enqueue(dataWrapper);
            TransportBackground.AddCommandAction(CommandActionAsync);
        }
        #endregion



        #region CommandActions
        /// <summary>
        ///Выполнить команду. Приоритетная однократно вызываемая функция.
        /// </summary>
        private async Task CommandActionAsync(CancellationToken ct)
        {
            var result = DataQueue.Dequeue();
            if (result.IsSuccess)
            {
                var inData = result.Value;
                var transportResponseWrapper = await PieceOfDataSender(DataAction.CommandAction,inData, ct);
                ResponseReadyRx.OnNext(transportResponseWrapper);
            }
        }
        #endregion
    }
}