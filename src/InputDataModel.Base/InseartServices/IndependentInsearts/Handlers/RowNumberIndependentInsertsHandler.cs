﻿using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Shared.Extensions;
using Shared.Helpers;
using Shared.Mathematic;
using Shared.Services.StringInseartService;
using Shared.Services.StringInseartService.IndependentInseart;

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
            
            if (dict.TryGetValue("rowNumber", out var value))  //в данных ЕСТЬ нужное значение
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
                    return Result.Failure<ValueTuple<string, StringInsertModel>>($"RowNumberIndependentInsertsHandler.  value= {value}   format= {_insertModel.Format}    {ex.Message}");
                }
            }
            //Нет базовой подстановки.
            return Result.Ok<ValueTuple<string, StringInsertModel>>((null, _insertModel));
        }
    }
}