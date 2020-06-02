using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Types;

namespace Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Handlers
{
    public class DelayTimeInsH : BaseInsH
    {
        public DelayTimeInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override Change<string> GetInseart(Lang lang, AdInputType uit)
        {
            var s = uit.DelayTime ?? DateTime.MinValue;
            var f = InsertModel.Ext.CalcFinishValue(s);
            return new Change<string>(s.ToString("t"), f);
        }
    }
}