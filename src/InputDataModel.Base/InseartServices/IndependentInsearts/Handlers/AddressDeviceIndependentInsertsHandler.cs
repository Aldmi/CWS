﻿using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Shared.Extensions;
using Shared.Helpers;
using Shared.Services.StringInseartService;
using Shared.Services.StringInseartService.IndependentInseart;

namespace Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Handlers
{
    public class AddressDeviceIndependentInsertsHandler : IIndependentInsertsHandler
    {
        private readonly StringInsertModel _insertModel;

        public AddressDeviceIndependentInsertsHandler(StringInsertModel insertModel)
        {
            this._insertModel = insertModel;
        }

        /// <summary>
        /// Выполнить подстановку данных их inData
        /// </summary>
        /// <param name="inData">Данные для подстановки</param>
        /// <returns>Result.Ok(null)- Входные данные не подходят для этого обработчика подстановки. Result.Failure - Данные подходяд для подстановки, но данные или формат данных НЕ верный</returns>
        public Result<ValueTuple<string, StringInsertModel>> CalcInserts(object inData)
        {
            if (!(inData is Dictionary<string, string> dict))
                return Result.Ok<ValueTuple<string, StringInsertModel>>((null, _insertModel));

            if (dict.TryGetValue(_insertModel.VarName, out var value))  //в данных ЕСТЬ нужное значение
            {
                try
                {
                    var address = int.Parse(value);
                    var res = address.Convert2StrByFormat(_insertModel.Format);
                    return Result.Ok((res, _insertModel));
                }
                catch (FormatException ex)
                {
                    return Result.Failure<ValueTuple<string, StringInsertModel>>($"AddressDeviceIndependentInsertsHandler.  value= {value}   format= {_insertModel.Format}    {ex.Message}");
                }
            }
            //Нет базовой подстановки.
            return Result.Ok<ValueTuple<string, StringInsertModel>>((null, _insertModel));
        }
    }
}