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
    public class OnceBehavior<TIn> : BaseBehavior<TIn> where TIn : InputTypeBase
    {
        #region ctor
        public OnceBehavior(string keyExchange,
            ITransportBackground transportBackground,
            Func<DataAction, InDataWrapper<TIn>, CancellationToken, Task<ResponsePieceOfDataWrapper<TIn>>> pieceOfDataSender,
            ILogger logger) : base(keyExchange, transportBackground, QueueMode.QueueExtractLastItem, pieceOfDataSender, logger)
        {
        }
        #endregion



        #region Methode
        public void SendData(IEnumerable<TIn> inData, string directHandlerName)
        {
            if (inData == null)
                throw new ArgumentNullException($"{nameof(inData)} НЕ может быть NULL");

            var dataWrapper = new InDataWrapper<TIn> { Datas = inData.ToList(), DirectHandlerName = directHandlerName };
            var result = DataQueue.Enqueue(dataWrapper);
            if (result.IsSuccess)
            {
                TransportBackground.AddOneTimeAction(OneTimeActionAsync);
            }
            else
            {
                //_logger.Debug($"SendOneTimeData in Queue Error: {result.Error}");
            }
        }
        #endregion



        #region OneTimeActions
        /// <summary>
        /// Однократно вызываемая функция.
        /// </summary>
        private async Task OneTimeActionAsync(CancellationToken ct)
        {
            var result = DataQueue.Dequeue();
            if (result.IsSuccess)
            {
                var inData = result.Value;
                var transportResponseWrapper = await PieceOfDataSender(DataAction.OneTimeAction, inData, ct);
                ResponseReadyRx.OnNext(transportResponseWrapper);
            }
        }
        #endregion
    }
}