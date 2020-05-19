using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;
using Shared.Helpers;

namespace Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Handlers
{
    public class SecondInsH : BaseInsH
    {
        public SecondInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override string GetInseart(Lang lang, AdInputType uit)
        {
            var second = DateTime.Now.Second;
            var res = InsertModel.Ext.CalcFinishValue(second);
            return res.GetSpaceOrString();
        }
    }
}