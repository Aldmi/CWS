using System.Collections.Generic;
using Shared.Extensions;
using Shared.Mathematic;

namespace Domain.InputDataModel.Base.InseartServices.IndependentInsearts.IndependentInseartsHandlers
{
    public class RowNumberIndependentInsertsHandler : IIndependentInsertsHandler
    {
        public string CalcInserts(IndependentInsertModel inseart, object inData)
        {
            if (!(inData is Dictionary<string, string> dict))
                return null;
            
            var varName = inseart.VarName;
            if (varName.Contains("rowNumber"))                             //обработка сложных значений (формул например)
            {
                if (dict.TryGetValue("rowNumber", out var rowNumberStr))  //например rowNumber задан как формула {(rowNumber+64):X1}
                {
                    if (int.TryParse(rowNumberStr, out int rowNumber))
                    {
                        var calcVal = MathematicFormat.CalculateMathematicFormat(varName, rowNumber);
                        var format = inseart.Format;
                        var res = calcVal.Convert2StrByFormat(format);
                        return res;
                    }
                }
            }
            //Нет базовой подстановки.
            return null;
        }
    }
}