using Shared.Helpers;

namespace Shared.Services.StringInseartService
{
    public interface IIndependentInseartsHandlersFactory
    {
        IIndependentInsertsHandler Create(StringInsertModel insertModel);
    }
}