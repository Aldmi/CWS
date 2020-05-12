using System.Collections.Generic;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.CrcCalculate;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers.Crc
{
    public class CrcMod256AlphaTimeDepInsH : BaseCrcDepInsH
    {
        public CrcMod256AlphaTimeDepInsH(StringInsertModel requiredModel) : base(requiredModel) { }


        protected override byte[] CrcAlgoritm(IReadOnlyList<byte> arg)
        {
            return CrcCalc.CalcMod256AlphaTime(arg);
        }
    }
}