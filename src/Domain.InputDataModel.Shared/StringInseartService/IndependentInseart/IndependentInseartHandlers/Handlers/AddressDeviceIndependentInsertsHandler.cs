using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Enums;
using Shared.Extensions;
using Shared.Types;

namespace Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers.Handlers
{
    public class AddressDeviceIndependentInsertsHandler : IIndependentInsertsHandler
    {
        private readonly StringInsertModel _insertModel;

        public AddressDeviceIndependentInsertsHandler(StringInsertModel insertModel)
        {
            _insertModel = insertModel;
        }

        /// <summary>
        /// Выполнить подстановку данных их inData
        /// </summary>
        /// <param name="inData">Данные для подстановки</param>
        /// <returns>Result.Ok(null)- Входные данные не подходят для этого обработчика подстановки. Result.Failure - Данные подходяд для подстановки, но данные или формат данных НЕ верный</returns>
        public Result<(Change<string>, StringInsertModel)> CalcInserts(object inData)
        {
            if (!(inData is Dictionary<string, string> dict))
                return Result.Ok<(Change<string>, StringInsertModel)>((null, _insertModel));

            if (dict.TryGetValue(_insertModel.VarName, out var value))  //в данных ЕСТЬ нужное значение
            {
                try
                {
                    var address = int.Parse(value);
                    var finishValue = _insertModel.Ext.CalcFinishValue(address);
                    return Result.Ok<(Change<string>, StringInsertModel)>((new Change<string>(address.ToString(), finishValue), _insertModel));
                }
                catch (FormatException ex)
                {
                    return Result.Failure<(Change<string>, StringInsertModel)>($"AddressDeviceIndependentInsertsHandler.  value= {value}   format= {_insertModel.Ext.Format}    {ex.Message}");
                }
            }
            //Нет базовой подстановки.
            return Result.Ok<(Change<string>, StringInsertModel)>((null, _insertModel));
        }
    }
}