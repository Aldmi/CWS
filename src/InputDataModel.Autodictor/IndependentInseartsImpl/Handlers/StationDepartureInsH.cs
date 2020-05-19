using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Helpers;

namespace Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Handlers
{
    public class StationDepartureInsH : BaseInsH
    {
        public StationDepartureInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override string GetInseart(Lang lang, AdInputType uit)
        {
            var str= uit.StationDeparture?.GetName(lang);
            var res = InsertModel.Ext.CalcFinishValue(str);
            return res.GetSpaceOrString();
        }
    }
}