using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;

namespace Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Handlers
{
    public class ExpectedTimeInsH : BaseInsH
    {
        public ExpectedTimeInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override string GetInseart(Lang lang, AdInputType uit)
        {
           var arrivalTime = uit.ArrivalTime ?? DateTime.MinValue;
           var departureTime = uit.DepartureTime ?? DateTime.MinValue;
           var time = (uit.Event?.Num != null && uit.Event.Num == 0) ? arrivalTime : departureTime;
           var expectedTime = (uit.ExpectedTime == DateTime.MinValue) ? time : uit.ExpectedTime;
           var res = InsertModel.Ext.CalcFinishValue(expectedTime);
           return res;
        }
    }
}