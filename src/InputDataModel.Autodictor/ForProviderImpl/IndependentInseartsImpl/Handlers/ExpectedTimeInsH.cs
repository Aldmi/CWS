﻿using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Types;

namespace Domain.InputDataModel.Autodictor.ForProviderImpl.IndependentInseartsImpl.Handlers
{
    public class ExpectedTimeInsH : BaseInsH
    {
        public ExpectedTimeInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override Change<string> GetInseart(Lang lang, AdInputType uit)
        {
           var arrivalTime = uit.ArrivalTime ?? DateTime.MinValue;
           var departureTime = uit.DepartureTime ?? DateTime.MinValue;
           var time = (uit.Event?.Num != null && uit.Event.Num == 0) ? arrivalTime : departureTime;
           var s = (uit.ExpectedTime == DateTime.MinValue) ? time : uit.ExpectedTime;
           var f = InsertModel.Ext.CalcFinishValue(s);
           return new Change<string>(s.ToString("t"), f);
        }
    }
}