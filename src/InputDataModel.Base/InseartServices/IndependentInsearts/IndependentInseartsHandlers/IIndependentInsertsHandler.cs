using Shared.Helpers;

namespace Domain.InputDataModel.Base.InseartServices.IndependentInsearts.IndependentInseartsHandlers
{
    public interface IIndependentInsertsHandler
    {
        string CalcInserts(StringInsertModel inseart, object inData);
    }
}