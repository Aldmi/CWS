using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;
using Shared.Helpers;

namespace Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Handlers
{
    public class YearInsH : BaseInsH
    {
        public YearInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override string GetInseart(Lang lang, AdInputType uit)
        {
            var year = DateTime.Now.Year;
            var res = InsertModel.Ext.CalcFinishValue(year);
            return res.GetSpaceOrString();
        }
    }
}