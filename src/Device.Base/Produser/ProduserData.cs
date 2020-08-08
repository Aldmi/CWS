using System.Collections.Generic;
using Domain.Exchange.Models;
using Domain.InputDataModel.Base.Response;
using Infrastructure.Produser.AbstractProduser.AbstractProduser;

namespace Domain.Device.Produser
{


    public class ProduserData<TIn>
    {
        #region prop
        public ProduserSendingDataType DataType { get; }
        public List<ExchangeFullState<TIn>> InitDatas { get; }
        public ResponsePieceOfDataWrapper<TIn> Data { get; }
        public object MessageObj { get; }
        #endregion



        #region ctor
        private ProduserData(
            ProduserSendingDataType dataType,
            List<ExchangeFullState<TIn>> initDatas,
            ResponsePieceOfDataWrapper<TIn> data,
            object messageObj)
        {
            DataType = dataType;
            InitDatas = initDatas;
            Data = data;
            MessageObj = messageObj;
        }
        #endregion



        #region Factories
        public static ProduserData<TIn> CreateInit(List<ExchangeFullState<TIn>> initDatas)
        {
            return new ProduserData<TIn>(ProduserSendingDataType.Init, initDatas, null, null);
        }


        public static ProduserData<TIn> CreateBoardData(ResponsePieceOfDataWrapper<TIn> boardData)
        {
            return new ProduserData<TIn>(ProduserSendingDataType.BoardData, null, boardData, null );
        }


        public static ProduserData<TIn> CreateInfo(object infoObj)
        {
            return new ProduserData<TIn>(ProduserSendingDataType.Info, null, null, infoObj);
        }


        public static ProduserData<TIn> CreateWarning(object warningObj)
        {
            return new ProduserData<TIn>(ProduserSendingDataType.Warning, null, null, warningObj);
        }
        #endregion
    }   
}