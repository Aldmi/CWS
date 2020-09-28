using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
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
        public string DeviceName { get; set; }                                  //Название ус-ва
        public string KeyExchange { get; set; }                                 //Ключ обмена
        public DataAction DataAction { get; set; }                              //Действие
        public long TimeAction { get; set; }                                    //Время выполнения обмена (на порцию данных)
        public EvaluateResponsesItemsResult Evaluation { get; private set; }    //Оценка ответов
        public Exception ExceptionExchangePipline { get; set; }                 //Критическая Ошибка обработки данных в конвеере.
        public List<ResponseDataItem<TIn>> ResponsesItems { get; set; } = new List<ResponseDataItem<TIn>>();



        public Result EvaluateResponsesItems()
        {
            var numberPreparedPackages =ResponsesItems.Count;                                                                                            //кол-во подготовленных к отправке пакетов  
            var countAll = ResponsesItems.Count(resp => resp.Status != StatusDataExchange.EndWithTimeout);                        //кол-во ВСЕХ полученных ответов
            var countIsValid = ResponsesItems.Count(resp => resp.ResponseInfo != null && resp.ResponseInfo.IsOutDataValid);       //кол-во ВАЛИДНЫХ ответов
            Evaluation = new EvaluateResponsesItemsResult(numberPreparedPackages, countAll, countIsValid, ResponsesItems.Select(r => r.Status));
            return Evaluation.IsValidAll ? Result.Ok() : Result.Failure(Evaluation.ErrorStat);
        }
    }


    public class EvaluateResponsesItemsResult
    {
        public readonly int NumberPreparedPackages;
        public readonly int CountAll;
        public readonly int CountIsValid;
        public readonly bool IsValidAll;
        public readonly string ErrorStat;

        public EvaluateResponsesItemsResult(int numberPreparedPackages, int countAll, int countIsValid, IEnumerable<StatusDataExchange> responseStatuses)
        {
            NumberPreparedPackages = numberPreparedPackages;
            CountAll = countAll;
            CountIsValid = countIsValid;

            if (countIsValid < numberPreparedPackages)
            {
                ErrorStat = responseStatuses.Select(status => status.ToString()).Aggregate((i, j) => i + " | " + j);
                IsValidAll = false;
            }
            else
            {
                IsValidAll = true;
                ErrorStat = string.Empty;
            }
        }
    }


    /// <summary>
    /// Единица ответа от устройства на единицу запроса
    /// </summary>
    public class ResponseDataItem<TIn> //TODO: Сильно много статусов, оптимизиршовать поля, объект создается на базе providerResult
    {
        public string RequestId { get; set; }
        public ProviderStatus ProviderStatus { get; set; }   
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