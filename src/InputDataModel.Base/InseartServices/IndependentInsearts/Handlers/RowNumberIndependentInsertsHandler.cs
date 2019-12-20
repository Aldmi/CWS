using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Shared.Extensions;
using Shared.Helpers;
using Shared.Mathematic;
using Shared.Services.StringInseartService;

namespace Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Handlers
{
    public class RowNumberIndependentInsertsHandler : IIndependentInsertsHandler
    {
        private readonly StringInsertModel _insertModel;

        public RowNumberIndependentInsertsHandler(StringInsertModel insertModel)
        {
            _insertModel = insertModel;
        }

        public Result<ValueTuple<string, StringInsertModel>> CalcInserts(object inData)
        {
            if (!(inData is Dictionary<string, string> dict))
                return Result.Ok<ValueTuple<string, StringInsertModel>>((null, _insertModel));
            
            if (dict.TryGetValue(_insertModel.VarName, out var value))  //в данных ЕСТЬ нужное значение
            {
                try
                {
                    var rowNumber = int.Parse(value);
                    var calcVal = MathematicFormat.CalculateMathematicFormat(_insertModel.VarName, rowNumber);
                    var res = calcVal.Convert2StrByFormat(_insertModel.Format);
                    return Result.Ok((res, _insertModel));
                }
                catch (FormatException ex)
                {
                    return Result.Fail<ValueTuple<string, StringInsertModel>>($"RowNumberIndependentInsertsHandler.  value= {value}   format= {_insertModel.Format}    {ex.Message}");
                }
            }
            //Нет базовой подстановки.
            return Result.Ok<ValueTuple<string, StringInsertModel>>((null, _insertModel));
        }


        //DEL
        //public  Result<string> CalcInserts(object inData)
        //{
        //    if (!(inData is Dictionary<string, string> dict))
        //        return Result.Ok<string>(null);
            
        //    var varName = inseart.VarName;
        //    if (varName.Contains("rowNumber"))                             //обработка сложных значений (формул например)
        //    {
        //        if (dict.TryGetValue("rowNumber", out var rowNumberStr))  //например rowNumber задан как формула {(rowNumber+64):X1}
        //        {
        //            if (int.TryParse(rowNumberStr, out int rowNumber))
        //            {
        //                var calcVal = MathematicFormat.CalculateMathematicFormat(varName, rowNumber);
        //                var format = inseart.Format;
        //                var res = calcVal.Convert2StrByFormat(format);
        //                return res;
        //            }
        //        }
        //    }
        //    //Нет базовой подстановки.
        //    return null;
        //}
    }
}