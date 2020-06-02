using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;
using Shared.Helpers;
using Shared.Types;

namespace Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Handlers
{
    public class YearInsH : BaseInsH
    {
        public YearInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override Change<string> GetInseart(Lang lang, AdInputType uit)
        {
            var s = DateTime.Now.Year;
            var f = InsertModel.Ext.CalcFinishValue(s);
            return new Change<string>(s.ToString(), f.GetSpaceOrString());
        }
    }
}