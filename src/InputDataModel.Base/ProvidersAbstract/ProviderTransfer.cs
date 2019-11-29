using System.Collections.Generic;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.ProvidersOption;
using Domain.InputDataModel.Base.Services;
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
        private readonly RequestResonseOption _option;                          //Base опции.


        #region prop

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

        #endregion


        #region ctor

        protected BaseTransfer(RequestResonseOption option)
        {
            _option = option;
        }

        #endregion
    }



    public class RequestTransfer<TIn> : BaseTransfer
    {
        #region prop
        public RequestOption Option { get; }
        public int BodyLenght { get; set; }                   //Размер тела запроса todo: НЕ ИСПОЛЬЗУЕТСЯ ???
        public ProcessedItemsInBatch<TIn> ProcessedItemsInBatch { get; set; }
        #endregion



        #region ctor
        public RequestTransfer(RequestOption option) : base(option)
        {
            Option = option;
        }
        #endregion
    }


    public class ResponseTransfer : BaseTransfer
    {
        #region prop

        public ResponseOption Option { get; }

        #endregion



        #region ctor

        public ResponseTransfer(ResponseOption option) : base(option)
        {
            Option = option;
        }

        #endregion
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
    /// Единица обработанного элемента входныз данных.
    /// </summary>
    public class ProcessedItem<TIn>
    {
        public TIn InDataItem { get; set; }
        public IndependentInserts InseartedData { get; set; }  //Обработанные и выставленные в протокол данные из InDataItem

        public ProcessedItem(TIn inDataItem, IndependentInserts inseartedData)
        {
            InDataItem = inDataItem;
            InseartedData = inseartedData;
        }
    }
}