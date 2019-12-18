using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Handlers;
using Shared.Helpers;

namespace Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Factory
{
    public class BaseIndependentInseartsHandlersFactory : IIndependentInseartsHandlersFactory
    {
        public  IIndependentInsertsHandler Create(StringInsertModel insertModel)
        {
            switch (insertModel.VarName)
            {
                case "AddressDevice":
                    return new AddressDeviceIndependentInsertsHandler(insertModel);

                case "rowNumber":
                    return new AddressDeviceIndependentInsertsHandler(insertModel);

                default:
                    return null;
            }
        }
    }
}