using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;
using Shared.Mathematic;

namespace Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers.Handlers
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
            
            if (dict.TryGetValue("rowNumber", out var value))  //в данных ЕСТЬ нужное значение
            {
                try
                {
                    var rowNumber = int.Parse(value);
                   // var mathExpressions = _insertModel.Options[0];
                   // var calcVal = MathematicFormat.CalculateMathematicFormat(mathExpressions, rowNumber);
                    var res = "DEBUG";//calcVal.Convert2StrByFormat(_insertModel.Format);
                    return Result.Ok((res, _insertModel));
                }
                catch (FormatException ex)
                {
                    return Result.Failure<ValueTuple<string, StringInsertModel>>($"RowNumberIndependentInsertsHandler.  value= {value}   format= {_insertModel.Ext.Format}    {ex.Message}");
                }
            }
            //Нет базовой подстановки.
            return Result.Ok<ValueTuple<string, StringInsertModel>>((null, _insertModel));
        }
    }
}