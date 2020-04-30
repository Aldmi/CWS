using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.CrcCalculate;
using Shared.Extensions;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers
{
    public class CrcMod256AlphaTimeDepInsH : BaseCrcDepInsH
    {
        public CrcMod256AlphaTimeDepInsH(StringInsertModel requiredModel) : base(requiredModel) { }


        protected override Result<string> GetInseart(StringBuilder sb, string format)
        { 
            var (_, isFailure, arr, error) = CrcHelper.CalcCrc(sb, Border, format, RequiredModel.Replacement, CrcCalc.CalcMod256AlphaTime);
            if (isFailure)
                return Result.Failure<string>(error);

            var resStr = RequiredModel.Ext.CalcFinishValue(arr);
            return Result.Ok(resStr);
        }
    }
}