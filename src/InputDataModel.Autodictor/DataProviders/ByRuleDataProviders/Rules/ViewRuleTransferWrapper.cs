﻿using System.Collections.Generic;
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
        public RequestTransfer Request { get; set; }                //Строка запроса, созданная по правилам RequestOption.
        public ResponseTransfer Response { get; set; }              //Строка ответа, созданная по правилам ResponseOption.


        //public string StringRequest { get; set; }                   //Строка запроса, созданная по правилам RequestOption.
        //public string StringResponse { get; set; }                  //Строка ответа, созданная по правилам ResponseOption.
        //public int BodyLenght { get; set; }                         //Размер тела запроса todo: НЕ ИСПОЛЬЗУЕТСЯ ???
        //public RequestOption RequestOption { get; set; }            //Запрос.
        //public ResponseOption ResponseOption { get; set; }          //Ответ.

    }



    public abstract class BaseTransfer
    {
        private readonly RequestResonseOption _option;               //Base опции.


        #region prop

        public StringRepresentation StrRepresentBase { get; set; }             //Строковое представление данных, созданная по правилам Option (В ФОРМАТЕ ИЗ Option).
        public StringRepresentation StrRepresent { get; set; }                 //Строковое представление данных, созданная по правилам Option (ВОЗМОЖНО В ИЗМЕНЕННОМ ФОРМАТЕ)

        public bool EqualStrRepresent => StrRepresent.Equals(StrRepresentBase); //TODO: StringRepresentation реализовать IEquable
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


    public class StringRepresentation
    {
        public string Str { get;  }
        public string Format { get;  }

        public StringRepresentation(string str, string format)
        {
            Str = str;
            Format = format;
        }
    }
}