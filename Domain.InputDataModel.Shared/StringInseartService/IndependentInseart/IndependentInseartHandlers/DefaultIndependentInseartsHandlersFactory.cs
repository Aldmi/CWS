using Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers.Handlers;
using Domain.InputDataModel.Shared.StringInseartService.Model;

namespace Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers
{
    public class DefaultIndependentInseartsHandlersFactory : IIndependentInseartsHandlersFactory
    {
        public IIndependentInsertsHandler Create(StringInsertModel insertModel)
        {
            return insertModel.VarName switch
            {
                "AddressDevice" => (IIndependentInsertsHandler) new AddressDeviceIndependentInsertsHandler(insertModel),
                "MATH" => new RowNumberIndependentInsertsHandler(insertModel),//string s when s.Contains("rowNumber"):
                _ => null
            };
        }
    }
}