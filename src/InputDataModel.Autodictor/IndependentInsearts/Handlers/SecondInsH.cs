using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Shared.Extensions;
using Shared.Services.StringInseartService;
using Shared.Services.StringInseartService.IndependentInseart;

namespace Domain.InputDataModel.Autodictor.IndependentInsearts.Handlers
{
    public class SecondInsH : BaseInsH
    {
        public SecondInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override string GetInseart(Lang lang, AdInputType uit)
        {
            var second = DateTime.Now.Second;
            var format = InsertModel.Format;
            return second.Convert2StrByFormat(format);
        }
    }
}