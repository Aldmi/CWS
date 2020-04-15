using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Shared.Extensions;
using Shared.Services.StringInseartService;


namespace Domain.InputDataModel.Autodictor.IndependentInsearts.Handlers
{
    public class DelayTimeInsH : BaseInsH
    {
        public DelayTimeInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override string GetInseart(Lang lang, AdInputType uit)
        {
            var delayTime = uit.DelayTime ?? DateTime.MinValue;
            var format = InsertModel.Format;

           return delayTime.Convert2StrByFormat(format);
        }
    }
}