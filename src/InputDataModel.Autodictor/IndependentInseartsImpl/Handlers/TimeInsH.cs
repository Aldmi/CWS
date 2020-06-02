using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;
using Shared.Helpers;
using Shared.Types;

namespace Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Handlers
{
    public class TimeInsH : BaseInsH
    {
        public TimeInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override Change<string> GetInseart(Lang lang, AdInputType uit)
        {
            var arrivalTime = uit.ArrivalTime ?? DateTime.MinValue;
            var departureTime = uit.DepartureTime ?? DateTime.MinValue;
            var s = (uit.Event?.Num != null && uit.Event.Num == 0) ? arrivalTime : departureTime;
            var f = InsertModel.Ext.CalcFinishValue(s);
            return new Change<string>(s.ToString("t"), f.GetSpaceOrString());
        }
    }
}