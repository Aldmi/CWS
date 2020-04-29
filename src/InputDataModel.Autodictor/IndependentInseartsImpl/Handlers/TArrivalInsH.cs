using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;
using Shared.Helpers;

namespace Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Handlers
{
    public class TArrivalInsH : BaseInsH
    {
        public TArrivalInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override string GetInseart(Lang lang, AdInputType uit)
        {
            var arrivalTime = uit.ArrivalTime ?? DateTime.MinValue;
            var res = InsertModel.Ext.CalcFinishValue(arrivalTime);
            return res.GetSpaceOrString();
        }
    }
}