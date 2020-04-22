using Domain.InputDataModel.Shared.StringInseartService.Model;

namespace Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers
{
    public interface IIndependentInseartsHandlersFactory
    {
        IIndependentInsertsHandler Create(StringInsertModel insertModel);
    }
}