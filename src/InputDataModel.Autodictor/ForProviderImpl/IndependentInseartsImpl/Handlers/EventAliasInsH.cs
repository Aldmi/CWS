using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Helpers;
using Shared.Types;

namespace Domain.InputDataModel.Autodictor.ForProviderImpl.IndependentInseartsImpl.Handlers
{
    public class EventAliasInsH : BaseInsH
    {
        public EventAliasInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override Change<string> GetInseart(Lang lang, AdInputType uit)
        {
            var str = uit.Event?.GetNameAlias(lang);
            var res = InsertModel.Ext.CalcFinishValue(str);
            return new Change<string>(str.GetSpaceOrString(), res.GetSpaceOrString());
        }
    }
}