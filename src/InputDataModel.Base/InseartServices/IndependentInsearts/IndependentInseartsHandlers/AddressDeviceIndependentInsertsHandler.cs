using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core.Exceptions;
using CSharpFunctionalExtensions;
using Shared.Extensions;
using Shared.Helpers;

namespace Domain.InputDataModel.Base.InseartServices.IndependentInsearts.IndependentInseartsHandlers
{
    public class AddressDeviceIndependentInsertsHandler : IIndependentInsertsHandler
    {
        private readonly StringInsertModel insertModel; //TODO: инициализация в ctor


        public string CalcInserts(StringInsertModel inseart, object inData)
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
                        throw new ParseException($"parse to int Exception. value= {value}",1);  //TODO: return Result
                }
            }
            //Нет базовой подстановки.
            return null;
        }



        /// <summary>
        /// Выполнить подстановку данных их inData
        /// </summary>
        /// <param name="inData">Данные для подстановки</param>
        /// <returns>Result.Ok(null)- Входные данные не подходят для этого обработчика подстановки. Result.Fail - Данные подходяд для подстановки, но данные или формат данных НЕ верный</returns>
        public Result<string> CalcInserts(object inData)
        {
            if (!(inData is Dictionary<string, string> dict))
                return Result.Ok<string>(null);

            if (dict.TryGetValue(insertModel.VarName, out var value))  //в данных ЕСТЬ нужное значение
            {
                try
                {
                    var address = int.Parse(value);
                    var res =  address.Convert2StrByFormat(insertModel.Format);
                    return Result.Ok(res);
                }
                catch (FormatException ex)
                {
                    return Result.Fail<string>($"AddressDeviceIndependentInsertsHandler.  value= {value}   format= {insertModel.Format}    {ex.Message}");
                }
            }

            //Нет базовой подстановки.
            return Result.Ok<string>(null);
        }
    }
}