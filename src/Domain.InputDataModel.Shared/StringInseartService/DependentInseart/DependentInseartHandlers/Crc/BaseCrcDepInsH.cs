using System.Collections.Generic;
using System.Text;
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
        /// <param name="sb"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        protected override Result<string> GetInseart(StringBuilder sb, string format)
        {
            var (_, isFailure, arr, error) = CrcHelper.CalcCrc(sb, RequiredModel.Ext.CalcBorderSubString, format, RequiredModel.Replacement, CrcAlgoritm);
            if (isFailure)
                return Result.Failure<string>(error);

            var resStr = RequiredModel.Ext.CalcFinishValue(arr);
            return Result.Ok(resStr);
        }

        /// <summary>
        /// Задает алгоритм вычисления CRC
        /// </summary>
        protected abstract byte[] CrcAlgoritm(IReadOnlyList<byte> arg);
    }
}