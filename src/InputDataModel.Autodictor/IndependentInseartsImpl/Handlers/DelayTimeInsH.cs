using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;
using Shared.Helpers;

namespace Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Handlers
{
    public class DelayTimeInsH : BaseInsH
    {
        public DelayTimeInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override string GetInseart(Lang lang, AdInputType uit)
        {
            var delayTime = uit.DelayTime ?? DateTime.MinValue;
            var res = InsertModel.Ext.CalcFinishValue(delayTime);
            return res;
        }
    }
}