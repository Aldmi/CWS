using System;
using System.Text;
using CSharpFunctionalExtensions;
using Shared.CrcCalculate;
using Shared.Extensions;

namespace Shared.Services.StringInseartService.DependentInseart.DependentInseartHandlers
{
    public class Crc16CcittDepInsH : BaseDepInsH
    {
        private readonly (string startCh, string endCh, bool includeBorder) _border;
        private readonly ByteHexDelemiter _hexDelemiter;


        public Crc16CcittDepInsH(StringInsertModel requiredModel) : base(requiredModel)
        {
            var partsOptions = requiredModel.FindPartsOptions();
            switch (partsOptions.Count)
            {
                case 1:
                    _border = CrcHelper.CalcBorderSubString(partsOptions[0]);
                    break;

                case 2:
                    _border = CrcHelper.CalcBorderSubString(partsOptions[0]);
                    Enum.TryParse(partsOptions[1], out _hexDelemiter);
                    break;
            }
        }


        protected override Result<string> GetInseart(StringBuilder sb, string format)
        {
            var crc16Ccitt = new CrcCalc.Crc16Ccitt(0xFFFF, 0x1021);
            var (_, isFailure, arr, error) = CrcHelper.CalcCrc(sb, _border, format, RequiredModel.Replacement, crc16Ccitt.Calc);
            if (isFailure)
                return Result.Failure<string>(error);
           
            var resStr = arr.BitConverter2StrByFormat(RequiredModel.Format, _hexDelemiter);
            return Result.Ok(resStr);
        }
    }


    public enum ByteHexDelemiter { None, Hex}
}