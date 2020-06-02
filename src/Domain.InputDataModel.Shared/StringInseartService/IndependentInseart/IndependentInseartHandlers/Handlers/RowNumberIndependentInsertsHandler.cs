using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;
using Shared.Mathematic;
using Shared.Types;

namespace Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers.Handlers
{
    public class RowNumberIndependentInsertsHandler : IIndependentInsertsHandler
    {
        private readonly StringInsertModel _insertModel;

        public RowNumberIndependentInsertsHandler(StringInsertModel insertModel)
        {
            _insertModel = insertModel;
        }

        public Result<(Change<string>, StringInsertModel)> CalcInserts(object inData)
        {
            if (!(inData is Dictionary<string, string> dict))
                return Result.Ok<(Change<string>, StringInsertModel)>((null, _insertModel));

            if (dict.TryGetValue("rowNumber", out var value))  //в данных ЕСТЬ нужное значение
            {
                try
                {
                    var rowNumber = int.Parse(value);
                    var mathExpressions = _insertModel.Option;
                    var calcVal = MathematicFormat.CalculateMathematicFormat(mathExpressions, rowNumber);
                    var finishValue = _insertModel.Ext.CalcFinishValue(calcVal);
                    return Result.Ok<(Change<string>, StringInsertModel)>((new Change<string>(calcVal.ToString(), finishValue), _insertModel));
                }
                catch (FormatException ex)
                {
                    return Result.Failure<(Change<string>, StringInsertModel)>($"RowNumberIndependentInsertsHandler.  value= {value}   format= {_insertModel.Ext.Format}    {ex.Message}");
                }
            }
            //Нет базовой подстановки.
            return Result.Ok<(Change<string>, StringInsertModel)>((null, _insertModel));
        }
    }
}