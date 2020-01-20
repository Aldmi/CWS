using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Shared.Helpers;
using Shared.Services.StringInseartService;
using Shared.Services.StringInseartService.IndependentInseart;

namespace Domain.InputDataModel.Autodictor.IndependentInsearts.Handlers
{
    public class StationsInsH : BaseInsH
    {
        public StationsInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override string GetInseart(Lang lang, AdInputType uit)
        {
           var str = uit.Stations?.GetName(lang);
           return str.GetSpaceOrString();
        }
    }
}