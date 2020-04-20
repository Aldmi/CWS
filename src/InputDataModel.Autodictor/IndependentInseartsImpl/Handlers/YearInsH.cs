using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;

namespace Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Handlers
{
    public class YearInsH : BaseInsH
    {
        public YearInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override string GetInseart(Lang lang, AdInputType uit)
        {
            var year = DateTime.Now.Year;
            var format = InsertModel.Format;
            return year.Convert2StrByFormat(format);
        }
    }
}