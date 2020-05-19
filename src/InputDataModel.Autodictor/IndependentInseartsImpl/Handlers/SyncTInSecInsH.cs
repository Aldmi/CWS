using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;
using Shared.Helpers;

namespace Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Handlers
{
    public class SyncTInSecInsH : BaseInsH
    {
        public SyncTInSecInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override string GetInseart(Lang lang, AdInputType uit)
        {
            var syncTInSec = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
            var res = InsertModel.Ext.CalcFinishValue(syncTInSec);
            return res.GetSpaceOrString();
        }
    }
}