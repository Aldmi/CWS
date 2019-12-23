using System;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Handlers;
using Shared.Helpers;
using Shared.Services.StringInseartService;

namespace Domain.InputDataModel.Autodictor.IndependentInsearts.Handlers
{
    public class TypeNameInsH : BaseInsH
    {
        public TypeNameInsH(StringInsertModel insertModel) : base(insertModel){ }

        protected override string GetInseart(Lang lang, AdInputType uit)
        {
            var str = uit.TrainType?.GetName(lang);
            return str.GetSpaceOrString();
        }
    }
}