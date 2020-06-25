using System.Collections.Generic;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.CrcCalculate.CrcClasses;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers.Crc
{
    public class Crc8FullPolyDepInsH : BaseCrcDepInsH
    {
        public Crc8FullPolyDepInsH(StringInsertModel requiredModel) : base(requiredModel) { }


        protected override byte[] CrcAlgoritm(IReadOnlyList<byte> arg)
        {
            return Crc8FullPoly.Calc(arg);
        }
    }
}