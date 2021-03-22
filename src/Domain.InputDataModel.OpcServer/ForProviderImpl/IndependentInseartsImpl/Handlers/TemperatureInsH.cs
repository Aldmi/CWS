using Domain.InputDataModel.OpcServer.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Helpers;
using Shared.Types;

namespace Domain.InputDataModel.OpcServer.ForProviderImpl.IndependentInseartsImpl.Handlers
{
    public class TemperatureInsH : BaseInsH
    {
        public TemperatureInsH(StringInsertModel insertModel) : base(insertModel){}

        protected override Change<string> GetInseart(OpcInputType uit)
        {
            var s = uit.Temperature;
            var f = InsertModel.Ext.CalcFinishValue(s);
            f = $"{f}\u00B0C"; //симол градусов по цельсию
            return new Change<string>(s.ToString().GetSpaceOrString(), f.GetSpaceOrString());
        }
    }
}