using System.Collections.Generic;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.ProvidersAbstract;

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
        public List<ProcessedItemsInBatch<T>> ProcessedItemsInBatch { get; } //Батчи обработанных элементов.
        #endregion


        #region ctor
        public LastSendPieceOfDataRxModel(string deviceName, string keyExchange, DataAction dataAction, long timeAction, bool isValidAll, List<ProcessedItemsInBatch<T>> processedItemsInBatch)
        {
            DeviceName = deviceName;
            KeyExchange = keyExchange;
            DataAction = dataAction;
            TimeAction = timeAction;
            IsValidAll = isValidAll;
            ProcessedItemsInBatch = processedItemsInBatch;
            CalcStatus();
        }
        #endregion



        private void CalcStatus()
        {
            if (ProcessedItemsInBatch == null)
                Status = "Отправлены данные по умолчанию";
            else
            if(IsValidAll)
            {
                Status = "Успешно";
            }
        }
    }
}