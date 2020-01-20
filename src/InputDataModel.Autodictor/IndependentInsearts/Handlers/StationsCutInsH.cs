using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Shared.Helpers;
using Shared.Services.StringInseartService;
using Shared.Services.StringInseartService.IndependentInseart;

namespace Domain.InputDataModel.Autodictor.IndependentInsearts.Handlers
{
    public class StationsCutInsH : BaseInsH
    {
        public StationsCutInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override string GetInseart(Lang lang, AdInputType uit)
        {
            //var stationsCut = uit.StationsСut?.GetName(lang);
            //return string.IsNullOrEmpty(stationsCut) ? "ПОСАДКИ НЕТ" : stationsCut;
            var str = uit.StationsСut?.GetName(lang);
            return str.GetSpaceOrString();
        }
    }
}