using System.Collections.Generic;
using Domain.InputDataModel.Base.Response;

namespace Domain.Device.Produser
{
    public enum ProduserSendingDataType { Init, BoardData, Info, Warning }

    public class ProduserData<TIn>
    {
        #region prop
        public ProduserSendingDataType DataType { get; }
        public List<ResponsePieceOfDataWrapper<TIn>> Datas { get; }
        public object MessageObj { get; }
        #endregion



        #region ctor
        private ProduserData(ProduserSendingDataType dataType, List<ResponsePieceOfDataWrapper<TIn>> datas = null, object messageObj = null)
        {
            DataType = dataType;
            Datas = datas;
            MessageObj = messageObj;
        }
        #endregion



        #region Factories
        public static ProduserData<TIn> CreateInit(List<ResponsePieceOfDataWrapper<TIn>> initDatas)
        {
            return new ProduserData<TIn>(ProduserSendingDataType.Init, initDatas);
        }


        public static ProduserData<TIn> CreateBoardData(ResponsePieceOfDataWrapper<TIn> boardData)
        {
            return new ProduserData<TIn>(ProduserSendingDataType.BoardData, new List<ResponsePieceOfDataWrapper<TIn>>(1){ boardData });
        }


        public static ProduserData<TIn> CreateInfo(object infoObj)
        {
            return new ProduserData<TIn>(ProduserSendingDataType.Info, null, infoObj);
        }


        public static ProduserData<TIn> CreateWarning(object warningObj)
        {
            return new ProduserData<TIn>(ProduserSendingDataType.Warning, null, warningObj);
        }
        #endregion

    }   
}