using System.Collections.Generic;
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
        /// ТОЛЬКО BorderSubString ОПРЕДЕЛЯЕТ ПОДСТРОКУ ДЛЯ GetInseart.
        /// </summary>
        protected override Result<string> GetSubString4Handle(string str)
        {
            return Result.Failure<string>("Для любого алгоритма CRC подстрока определяется BorderSubString. ОБЯЗАТЕЛЬНО ЗАДАЙТЕ BorderSubString.");
            //var res = RequiredModel.CalcSubStringBeetweenStartStringAndModel(str);
            //return res;
        }

        /// <summary>
        /// Задает алгоритм вычисления CRC.
        /// </summary>
        protected abstract byte[] CrcAlgoritm(IReadOnlyList<byte> arg);
    }
}