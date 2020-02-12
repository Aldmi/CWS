using System.Collections.Generic;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.ProvidersOption;
using Domain.InputDataModel.Base.Response.ResponseValidators;
using KellermanSoftware.CompareNetObjects;
using Shared.Types;

namespace Domain.InputDataModel.Base.ProvidersAbstract
{
    /// <summary>
    /// Единица запроса.
    /// </summary>
    public class ProviderTransfer<TIn>
    {
        public RequestTransfer<TIn> Request { get; set; }           //Строка запроса, созданная по правилам RequestOption.
        public ResponseTransfer Response { get; set; }              //Строка ответа, созданная по правилам ResponseOption.
        public Command4Device Command { get; set; }                 //Команда
    }



    public abstract class BaseTransfer
    {
        private readonly RequestResonseOption _option;                         //Base опции.
        public StringRepresentation StrRepresentBase { get; set; }             //Строковое представление данных, созданная по правилам Option (В ФОРМАТЕ ИЗ Option).
        public StringRepresentation StrRepresent { get; set; }                 //Строковое представление данных, созданная по правилам Option (ВОЗМОЖНО В ИЗМЕНЕННОМ ФОРМАТЕ)

        public bool EqualStrRepresent
        {
            get
            {
                var compareLogic = new CompareLogic();
                var result = compareLogic.Compare(StrRepresentBase, StrRepresent);
                return result.AreEqual;
            }
        }

        protected BaseTransfer(RequestResonseOption option)
        {
            _option = option;
        }
    }



    public class RequestTransfer<TIn> : BaseTransfer
    {
        public RequestOption Option { get; }
        public ProcessedItemsInBatch<TIn> ProcessedItemsInBatch { get; set; }
        public RequestTransfer(RequestOption option) : base(option)
        {
            Option = option;
        }
    }


    public class ResponseTransfer : BaseTransfer
    {
        // public BaseResponseValidator Validator { get; }
        //TODO: BaseResponseValidator вместо ResponseOption
        public ResponseOption Option { get; }
        public ResponseTransfer(ResponseOption option) : base(option)
        {
            Option = option;
        }
    }



    /// <summary>
    /// Обработанные входные данные в БАТЧЕ
    /// </summary>
    public class ProcessedItemsInBatch<TIn>
    {
        public int StartItemIndex { get; set; }                      //Начальный индекс (в базовом массиве, после TakeItems) элемента после разбиения на батчи.
        public int BatchSize { get; set; }                           //Размер батча.
        public List<ProcessedItem<TIn>> ProcessedItems { get; set; } //Обработанные входные данные.

        public ProcessedItemsInBatch(int startItemIndex, int batchSize, List<ProcessedItem<TIn>> processedItems)
        {
            StartItemIndex = startItemIndex;
            BatchSize = batchSize;
            ProcessedItems = processedItems;
        }
    }


    /// <summary>
    /// Единица обработанного элемента входных данных.
    /// </summary>
    public class ProcessedItem<TIn>
    {
        public TIn InDataItem { get; set; }
        public Dictionary<string, string> InseartedData { get; set; }  //Обработанные и выставленные в протокол данные из InDataItem

        public ProcessedItem(TIn inDataItem, Dictionary<string, string> inseartedData)
        {
            InDataItem = inDataItem;
            InseartedData = inseartedData;
        }
    }
}