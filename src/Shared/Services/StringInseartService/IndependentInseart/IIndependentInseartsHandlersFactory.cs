namespace Shared.Services.StringInseartService.IndependentInseart
{
    public interface IIndependentInseartsHandlersFactory
    {
        IIndependentInsertsHandler Create(StringInsertModel insertModel);
    }
}