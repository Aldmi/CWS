using System.Collections.Generic;
using System.Linq;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.Response;

namespace Domain.Exchange.RxModel
{
    public class LastSendPieceOfDataRxModel<T>
    {
        private readonly ResponsePieceOfDataWrapper<T> _response;

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
        public LastSendPieceOfDataRxModel(ResponsePieceOfDataWrapper<T> response)
        {
            _response = response;
            DeviceName = response.DeviceName;
            KeyExchange = response.KeyExchange;
            DataAction = response.DataAction;
            TimeAction = response.TimeAction;
            IsValidAll = response.IsValidAll;
            ProcessedItemsInBatch = response.ResponsesItems.Select(item => item.ProcessedItemsInBatch).ToList();
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


        /// <summary>
        /// Получить полный объект состояние обмена
        /// </summary>
        public ResponsePieceOfDataWrapper<T> GetResponsePieceOfDataWrapper()
        {
            return _response;
        }
    }
}