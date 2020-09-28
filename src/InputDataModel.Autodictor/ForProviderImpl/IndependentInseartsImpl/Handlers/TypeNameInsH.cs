using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Helpers;
using Shared.Types;

namespace Domain.InputDataModel.Autodictor.ForProviderImpl.IndependentInseartsImpl.Handlers
{
    public class TypeNameInsH : BaseInsH
    {
        public TypeNameInsH(StringInsertModel insertModel) : base(insertModel){ }

        protected override Change<string> GetInseart(Lang lang, AdInputType uit)
        {
            var s = uit.TrainType?.GetName(lang);
            var f = InsertModel.Ext.CalcFinishValue(s);
            return new Change<string>(s.GetSpaceOrString(), f.GetSpaceOrString());
        }
    }
}