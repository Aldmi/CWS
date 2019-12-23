﻿using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Shared.Helpers;
using Shared.Services.StringInseartService;

namespace Domain.InputDataModel.Autodictor.IndependentInsearts.Handlers
{
    public class PathNumberInsH : BaseInsH
    {
        public PathNumberInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override string GetInseart(Lang lang, AdInputType uit)
        {
            var str = uit.PathNumber;
            return str.GetSpaceOrString();
        }
    }
}