﻿using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.CrcCalculate;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers.Crc
{

    public abstract class BaseCrcDepInsH : BaseDepInsH
    {
        #region ctor
        protected BaseCrcDepInsH(StringInsertModel requiredModel) : base(requiredModel) { }
        #endregion


        /// <summary>
        /// Задает базовые шаги вычисления CRC
        /// </summary>
        /// <param name="borderedStr"></param>
        /// <param name="format"></param>
        /// <param name="sbMutable"></param>
        /// <returns></returns>
        protected override Result<string> GetInseart(string borderedStr, string format, StringBuilder sbMutable)
        {
            var (_, isFailure, arr, error) = CrcHelper.CalcCrc(borderedStr, format, RequiredModel.Replacement, CrcAlgoritm);
            if (isFailure)
                return Result.Failure<string>(error);

            var resStr = RequiredModel.Ext.CalcFinishValue(arr);
            return Result.Ok(resStr);
        }


        /// <summary>
        /// ЕСЛИ BorderSubString НЕ УКАЗАН.
        /// Подстрока - от начала строки до блока {CRC...}
        /// </summary>
        protected override Result<string> GetSubString4Handle(string str)
        {
            var nativeBorder = RequiredModel.Replacement;
            var matchString = Regex.Match(str, $"(.*){nativeBorder}").Groups[1].Value;
            return Result.Ok(matchString);
        }

        /// <summary>
        /// Задает алгоритм вычисления CRC
        /// </summary>
        protected abstract byte[] CrcAlgoritm(IReadOnlyList<byte> arg);
    }
}