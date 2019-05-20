using System.Collections.Generic;
using DAL.Abstract.Entities.Options.Exchange.ProvidersOption;
using InputDataModel.Autodictor.Model;

namespace InputDataModel.Autodictor.DataProviders.ByRuleDataProviders.Rules
{
    /// <summary>
    /// Единица запроса обработанная ViewRule.
    /// </summary>
    public class ViewRuleTransferWrapper
    {
        public int StartItemIndex { get; set; }                     //Начальный индекс (в базовом массиве, после TakeItems) элемента после разбиения на батчи.
        public int BatchSize { get; set; }                          //Размер батча.
        public IEnumerable<AdInputType> BatchedData { get; set; }   //Набор входных данных на базе которых созданна StringRequest.

        //TODO: все что ниже заменить на RequestTransfer и ResponseTransfer. (RequestOption, ResponseOption - оставить имутабельными)
        public string StringRequest { get; set; }                   //Строка запроса, созданная по правилам RequestOption.
        public string StringResponse { get; set; }                  //Строка ответа, созданная по правилам ResponseOption.
        public int BodyLenght { get; set; }                         //Размер тела запроса todo: НЕ ИСПОЛЬЗУЕТСЯ ???
        public RequestOption RequestOption { get; set; }            //Запрос.
        public ResponseOption ResponseOption { get; set; }          //Ответ.
    }



    public class BaseTransfer
    {
        #region prop

        public string StringRequestBase { get; set; }               //Строка запроса, созданная по правилам RequestOption (В ФОРМАТЕ ИЗ RequestOption).
        public string StringRequest { get; set; }                   //Строка запроса, созданная по правилам RequestOption. (ВОЗМОЖНО В ИЗМЕНЕННОМ ФОРМАТЕ)
        public RequestResonseOption Option { get; set; }            //Base опции запроса

        #endregion


        #region ctor

        public BaseTransfer(RequestResonseOption option)
        {
            Option = option;
        }

        #endregion


        #region DynamicProp

        private string _dymamicFormat;
        public string Format
        {
            get => string.IsNullOrEmpty(_dymamicFormat) ? Option.Format : _dymamicFormat;
            set => _dymamicFormat = value;
        }

        #endregion
    }



    public class RequestTransfer : BaseTransfer
    {
        #region prop

        public RequestOption RequestOption { get; }

        #endregion



        #region ctor

        public RequestTransfer(RequestOption requestOption) : base(requestOption)
        {
            RequestOption = requestOption;
        }

        #endregion
    }


    public class ResponseTransfer : BaseTransfer
    {
        #region prop

        public ResponseOption RequestOption { get; }

        #endregion



        #region ctor

        public ResponseTransfer(ResponseOption requestOption) : base(requestOption)
        {
            RequestOption = requestOption;
        }

        #endregion
    }
}