using System.Collections.Generic;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.CrcCalculate;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers.Crc
{
    public class Crc8MaximDepInsH : BaseCrcDepInsH
    {
        public Crc8MaximDepInsH(StringInsertModel requiredModel) : base(requiredModel) {}


        protected override byte[] CrcAlgoritm(IReadOnlyList<byte> arg)
        { 
            return CrcCalc.CalcCrc8Maxim(arg);
        }
    }
}