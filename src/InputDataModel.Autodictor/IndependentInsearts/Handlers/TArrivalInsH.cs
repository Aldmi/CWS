using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Shared.Extensions;
using Shared.Services.StringInseartService;

namespace Domain.InputDataModel.Autodictor.IndependentInsearts.Handlers
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