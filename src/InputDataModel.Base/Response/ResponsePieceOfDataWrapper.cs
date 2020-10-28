using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        //TODO: 1. для каждого ResponsesItems вызвать UnionBatchItems и объединить списки по ключу, добавив к значения словаря StatusDataExchange - чтобы понимать отправлен успешно или не успешно батч
        // ReadOnlyDictionary<int,  List<ProcessedItem<TIn>> processedItems> 
        // Создавать 2 списка с нормальнл оотработанными запросами Status == "End" и с ошибочными.
    }


    public class EvaluateResponsesItemsResult
    {
        public int NumberPreparedPackages { get; }
        public  int CountAll { get; }
        public  int CountIsValid { get; }
        public  bool IsValidAll { get; }
        public  string ErrorStat { get; }

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
        public ProviderStatus ProviderStatus { get; set; }   
        public StatusDataExchange Status { get; set; }
        public string StatusStr => Status.ToString();

        public Exception TransportException { get; set; }      //Ошибка передачи данных
        public BaseResponseInfo ResponseInfo { get; set; }     //Ответ

        /// <summary>
        /// Результат работы провайдера, обработанные и выставленные в протокол данные из InputData
        /// </summary>
        public ProcessedItemsInBatch<TIn> ProcessedItemsInBatch { get; set; }



        /// <summary>
        /// Развертка элементов в батче.
        /// Список ProcessedItemsInBatch.ProcessedItems преоюразуется в словарь, где ключ, это индекс элемента выделенный батчем.
        /// key- индекс переменной в списке
        /// value - ProcessedItem, т.е. обертка над словарем всех обработанных значений.
        /// </summary>
        public ReadOnlyDictionary<int, ProcessedItem<TIn>> SweepBatch()
        {
            var dict= new Dictionary<int, ProcessedItem<TIn>>();
            for (int i = 0; i < ProcessedItemsInBatch.BatchSize; i++)
            {
                var key = ProcessedItemsInBatch.StartItemIndex + i;
                var value = ProcessedItemsInBatch.ProcessedItems[i];
                dict[key] = value;
            }
            return new ReadOnlyDictionary<int, ProcessedItem<TIn>>(dict);
        }
    }
}