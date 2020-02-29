using System;
using System.Text;
using CSharpFunctionalExtensions;
using Shared.CrcCalculate;
using Shared.Enums;
using Shared.Extensions;

namespace Shared.Services.StringInseartService.DependentInseart.DependentInseartHandlers
{
    public class Crc16CcittDepInsH : BaseCrcDepInsH
    {
        public Crc16CcittDepInsH(StringInsertModel requiredModel) : base(requiredModel){}


        protected override Result<string> GetInseart(StringBuilder sb, string format)
        {
            var crc16Ccitt = new CrcCalc.Crc16Ccitt(0xFFFF, 0x1021);
            var (_, isFailure, arr, error) = CrcHelper.CalcCrc(sb, Border, format, RequiredModel.Replacement, crc16Ccitt.Calc);
            if (isFailure)
                return Result.Failure<string>(error);
           
            var resStr = arr.BitConverter2StrByFormat(RequiredModel.Format, HexDelemiter);
            return Result.Ok(resStr);
        }
    }
}