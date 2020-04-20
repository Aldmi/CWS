using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;

namespace Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Handlers
{
    public class MonthInsH : BaseInsH
    {
        public MonthInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override string GetInseart(Lang lang, AdInputType uit)
        {
            var month = DateTime.Now.Month;
            var format = InsertModel.Format;
            return month.Convert2StrByFormat(format);
        }
    }
}