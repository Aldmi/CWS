using System.Text.RegularExpressions;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Handlers;
using Shared.Helpers;
using Shared.Services.StringInseartService;
using Shared.Services.StringInseartService.IndependentInseart;

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
                
                case string s when s.Contains("rowNumber"):
                    return new RowNumberIndependentInsertsHandler(insertModel);

                default:
                    return null;
            }
        }
    }
}