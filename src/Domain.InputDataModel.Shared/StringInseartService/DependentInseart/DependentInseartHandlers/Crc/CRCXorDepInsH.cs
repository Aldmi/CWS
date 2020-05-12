using System.Collections.Generic;
using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.CrcCalculate;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers.Crc
{
    public class CrcXorDepInsH : BaseCrcDepInsH
    {
        public CrcXorDepInsH(StringInsertModel requiredModel) : base(requiredModel){}

        protected override byte[] CrcAlgoritm(IReadOnlyList<byte> arg)
        {
            return CrcCalc.CalcXor(arg);
        }
    }
}