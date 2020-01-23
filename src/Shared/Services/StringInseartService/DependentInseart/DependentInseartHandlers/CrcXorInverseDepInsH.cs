using System.Text;
using CSharpFunctionalExtensions;
using Shared.CrcCalculate;
using Shared.Extensions;

namespace Shared.Services.StringInseartService.DependentInseart.DependentInseartHandlers
{
    public class CrcXorInverseDepInsH : BaseDepInsH
    {
        private readonly (string startCh, string endCh, bool includeBorder) _border;

        public CrcXorInverseDepInsH(StringInsertModel requiredModel) : base(requiredModel)
        {
            var crcOption = requiredModel.Options;
            _border = CrcHelper.CalcBorderSubString(crcOption);
        }

        protected override Result<string> GetInseart(StringBuilder sb, string format)
        {
            var (_, isFailure, arr, error) = CrcHelper.CalcCrc(sb, _border, format, RequiredModel.Replacement, CrcCalc.CalcXorInverse);
            if (isFailure)
                return Result.Failure<string>(error);
           
            var resStr = arr.BitConverter2StrByFormat(RequiredModel.Format);
            return Result.Ok<string>(resStr);
        }
    }
}