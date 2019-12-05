using System.Collections.Generic;
using Shared.Extensions;

namespace Domain.InputDataModel.Base.InseartServices.IndependentInsearts.IndependentInseartsHandlers
{
    public class AddressDeviceIndependentInsertsHandler : IIndependentInsertsHandler
    {
        public string CalcInserts(IndependentInsertModel inseart, object inData)
        {
            if (!(inData is Dictionary<string, string> dict))
                return null;

            var varName = inseart.VarName;
            if (dict.TryGetValue(varName, out var value))
            {
                switch (varName)
                {
                    case "AddressDevice":
                        if (int.TryParse(value, out int address))
                        {
                            var format = inseart.Format;
                            var res =  address.Convert2StrByFormat(format);
                            return res;
                        }
                        break;
                }
            }
            //Нет базовой подстановки.
            return null;
        }
    }
}