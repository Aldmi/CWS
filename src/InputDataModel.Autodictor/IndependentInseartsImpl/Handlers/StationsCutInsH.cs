using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Helpers;

namespace Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Handlers
{
    public class StationsCutInsH : BaseInsH
    {
        public StationsCutInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override string GetInseart(Lang lang, AdInputType uit)
        {
            //var stationsCut = uit.StationsСut?.GetName(lang);
            //return string.IsNullOrEmpty(stationsCut) ? "ПОСАДКИ НЕТ" : stationsCut;
            var str = uit.StationsCut?.GetName(lang);
            return str.GetSpaceOrString();
        }
    }
}