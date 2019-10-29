using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InData;

namespace Domain.Exchange.RxModel
{
    public class LastSendPieceOfDataRxModel<T>
    {
        #region prop
        public string DeviceName { get; }                     //Название ус-ва
        public string KeyExchange { get;}                     //Ключ обмена
        public DataAction DataAction { get; }                 //Действие
        public long TimeAction { get; }                       //Время выполнения обмена (на порцию данных)
        public bool IsValidAll { get; }                       //Флаг валидности всех ответов
        public string Status { get; private set; }
        public InDataWrapper<T> Data { get; }
        #endregion


        #region ctor
        public LastSendPieceOfDataRxModel(string deviceName, string keyExchange, DataAction dataAction, long timeAction, bool isValidAll, InDataWrapper<T> data)
        {
            DeviceName = deviceName;
            KeyExchange = keyExchange;
            DataAction = dataAction;
            TimeAction = timeAction;
            IsValidAll = isValidAll;
            Data = data;
            CalcStatus();
        }
        #endregion



        private void CalcStatus()
        {
            if (Data == null)
                Status = "Отправлены данные по умолчанию";
            else
            if(IsValidAll)
            {
                Status = "Успешно";
            }
        }
    }
}