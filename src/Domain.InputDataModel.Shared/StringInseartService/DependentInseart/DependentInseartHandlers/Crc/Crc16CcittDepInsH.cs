using System.Collections.Generic;
using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.CrcCalculate;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers.Crc
{
    public class Crc16CcittDepInsH : BaseCrcDepInsH
    {
        public Crc16CcittDepInsH(StringInsertModel requiredModel) : base(requiredModel) {}


        protected override byte[] CrcAlgoritm(IReadOnlyList<byte> arg)
        {
            var crc16Ccitt = new CrcCalc.Crc16Ccitt(0xFFFF, 0x1021);
            return crc16Ccitt.Calc(arg);
        }
    }
}