using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;

namespace Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Handlers
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