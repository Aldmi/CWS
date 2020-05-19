using System.Collections.Generic;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.CrcCalculate;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers.Crc
{
    public class CrcXorInverseDepInsH : BaseCrcDepInsH
    {
        public CrcXorInverseDepInsH(StringInsertModel requiredModel) : base(requiredModel) { }


        protected override byte[] CrcAlgoritm(IReadOnlyList<byte> arg)
        {
            return CrcCalc.CalcXorInverse(arg);
        }
    }
}