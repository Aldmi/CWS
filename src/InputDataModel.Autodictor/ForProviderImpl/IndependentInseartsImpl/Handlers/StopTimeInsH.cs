using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Helpers;
using Shared.Types;

namespace Domain.InputDataModel.Autodictor.ForProviderImpl.IndependentInseartsImpl.Handlers
{
    public class StopTimeInsH : BaseInsH
    {
        public StopTimeInsH(StringInsertModel insertModel) : base(insertModel) { }

        protected override Change<string> GetInseart(Lang lang, AdInputType uit)
        {
            var s = uit.StopTime.HasValue ? new DateTime(uit.StopTime.Value.Ticks) : DateTime.MinValue;
            var f = InsertModel.Ext.CalcFinishValue(s);
            return new Change<string>(s.ToString("t"), f.GetEmptyOrString());
        }
    }
}