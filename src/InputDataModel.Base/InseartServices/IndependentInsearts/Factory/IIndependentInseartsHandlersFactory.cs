using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Handlers;
using Shared.Helpers;

namespace Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Factory
{
    public interface IIndependentInseartsHandlersFactory
    {
        IIndependentInsertsHandler Create(StringInsertModel insertModel);
    }
}