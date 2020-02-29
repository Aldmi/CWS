using System.Text;
using CSharpFunctionalExtensions;
using Shared.CrcCalculate;
using Shared.Extensions;

namespace Shared.Services.StringInseartService.DependentInseart.DependentInseartHandlers
{
    public class Crc8BitDepInsH : BaseCrcDepInsH
    {
        public Crc8BitDepInsH(StringInsertModel requiredModel) : base(requiredModel) { }

        protected override Result<string> GetInseart(StringBuilder sb, string format)
        { 
            var (_, isFailure, arr, error) = CrcHelper.CalcCrc(sb, Border, format, RequiredModel.Replacement, CrcCalc.Calc8Bit);
            if (isFailure)
                return Result.Failure<string>(error);

            var resStr = arr.BitConverter2StrByFormat(RequiredModel.Format, HexDelemiter);
            return Result.Ok(resStr);
        }
    }
}