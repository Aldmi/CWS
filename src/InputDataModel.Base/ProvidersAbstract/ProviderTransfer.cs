using System.Collections.Generic;
using Domain.InputDataModel.Base.ProvidersOption;
using KellermanSoftware.CompareNetObjects;
using Shared.Types;

namespace Domain.InputDataModel.Base.ProvidersAbstract
{
    /// <summary>
    /// Единица запроса обработанная ViewRule.
    /// </summary>
    public class ProviderTransfer<TIn>
    {
        public int StartItemIndex { get; set; }                     //Начальный индекс (в базовом массиве, после TakeItems) элемента после разбиения на батчи.
        public int BatchSize { get; set; }                          //Размер батча.
        public IEnumerable<TIn> BatchedData { get; set; }           //Набор входных данных на базе которых созданна StringRequest.
        public RequestTransfer Request { get; set; }                //Строка запроса, созданная по правилам RequestOption.
        public ResponseTransfer Response { get; set; }              //Строка ответа, созданная по правилам ResponseOption.
    }



    public abstract class BaseTransfer
    {
        private readonly RequestResonseOption _option;               //Base опции.


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



    public class RequestTransfer : BaseTransfer
    {
        #region prop

        public RequestOption Option { get; }
        public int BodyLenght { get; set; }                   //Размер тела запроса todo: НЕ ИСПОЛЬЗУЕТСЯ ???

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


}