using System;
using System.Collections.Generic;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.Response.ResponseInfos;
using Shared.Enums;


namespace Domain.InputDataModel.Base.Response
{
    /// <summary>
    /// Ответ от устройства на отправку порции данных.
    /// Содержит общую инфу и коллекцию ответов на каждый запрос ResponseDataItem
    /// </summary>
    public class ResponsePieceOfDataWrapper<TIn>
    {
        public string DeviceName { get; set; }                     //Название ус-ва
        public string KeyExchange { get; set; }                    //Ключ обмена
        public DataAction DataAction { get; set; }                 //Действие

        public long TimeAction { get; set; }                       //Время выполнения обмена (на порцию данных)
        public bool IsValidAll { get; set; }                       //Флаг валидности всех ответов

        public Exception ExceptionExchangePipline { get; set; }    //Критическая Ошибка обработки данных в конвеере.
        public Dictionary<string, string> MessageDict { get; set; } //Доп. информация
        public List<ResponseDataItem<TIn>> ResponsesItems { get; set; } = new List<ResponseDataItem<TIn>>();
    }


    /// <summary>
    /// Единица ответа от устройства на единицу запроса
    /// </summary>
    public class ResponseDataItem<TIn>
    {
        public string RequestId { get; set; }
        public Dictionary<string, string> MessageDict { get; set; }   //Доп. информация
        public StatusDataExchange Status { get; set; }
        public string StatusStr => Status.ToString();

        public Exception TransportException { get; set; }      //Ошибка передачи данных
        public BaseResponseInfo ResponseInfo { get; set; }     //Ответ

        /// <summary>
        /// Результат работы провайдера, обработанные и выставленные в протокол данные из InputData
        /// </summary>
        public ProcessedItemsInBatch<TIn> ProcessedItemsInBatch { get; set; }
    }


}