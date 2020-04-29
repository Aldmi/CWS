using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Helpers;

namespace Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Handlers
{
    public class TypeNameInsH : BaseInsH
    {
        public TypeNameInsH(StringInsertModel insertModel) : base(insertModel){ }

        protected override string GetInseart(Lang lang, AdInputType uit)
        {
            var str = uit.TrainType?.GetName(lang);
            var res = InsertModel.Ext.CalcFinishValue(str);
            return res.GetSpaceOrString();
        }
    }
}