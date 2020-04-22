using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;

namespace Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Handlers
{
    public class TArrivalInsH : BaseInsH
    {
        public TArrivalInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override string GetInseart(Lang lang, AdInputType uit)
        {
            var arrivalTime = uit.ArrivalTime ?? DateTime.MinValue;
            var format = InsertModel.Format;
            return arrivalTime.Convert2StrByFormat(format);
        }
    }
}