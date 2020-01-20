using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Shared.Extensions;
using Shared.Services.StringInseartService;
using Shared.Services.StringInseartService.IndependentInseart;

namespace Domain.InputDataModel.Autodictor.IndependentInsearts.Handlers
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
           var format = InsertModel.Format;
           return expectedTime.Convert2StrByFormat(format);
        }
    }
}