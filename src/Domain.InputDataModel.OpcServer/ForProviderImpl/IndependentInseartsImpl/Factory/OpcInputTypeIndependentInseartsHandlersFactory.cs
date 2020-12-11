using Domain.InputDataModel.OpcServer.ForProviderImpl.IndependentInseartsImpl.Handlers;
using Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers;
using Domain.InputDataModel.Shared.StringInseartService.Model;

namespace Domain.InputDataModel.OpcServer.ForProviderImpl.IndependentInseartsImpl.Factory
{
    public class OpcInputTypeIndependentInseartsHandlersFactory : IIndependentInseartsHandlersFactory
    {
        public IIndependentInsertsHandler Create(StringInsertModel insertModel)
        {
            return insertModel.VarName switch
            {
                "Temp" => new TemperatureInsH(insertModel),
                _ => null
            };
        }
    }
}