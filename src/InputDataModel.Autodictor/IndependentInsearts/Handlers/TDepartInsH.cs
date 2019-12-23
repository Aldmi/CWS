﻿using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Shared.Extensions;
using Shared.Services.StringInseartService;

namespace Domain.InputDataModel.Autodictor.IndependentInsearts.Handlers
{
    public class TDepartInsH : BaseInsH
    {
        public TDepartInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override string GetInseart(Lang lang, AdInputType uit)
        {
            var departureTime = uit.DepartureTime ?? DateTime.MinValue;
            var format = InsertModel.Format;
            return departureTime.Convert2StrByFormat(format);
        }
    }
}