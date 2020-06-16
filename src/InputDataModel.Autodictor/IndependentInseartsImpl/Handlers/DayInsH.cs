using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Types;

namespace Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Handlers
{
    public class DayInsH : BaseInsH
    {
        public DayInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override Change<string> GetInseart(Lang lang, AdInputType uit)
        {
            var day = DateTime.Now.Day;
            var f = InsertModel.Ext.CalcFinishValue(day);
            return new Change<string>(day.ToString(), f);
        }
    }
}