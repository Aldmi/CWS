using System.Collections.Generic;
using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.CrcCalculate;
using Shared.CrcCalculate.CrcClasses;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers.Crc
{
    public class Crc16CcittDepInsH : BaseCrcDepInsH
    {
        private readonly Crc16Ccitt _crc16Ccitt;

        public Crc16CcittDepInsH(StringInsertModel requiredModel) : base(requiredModel)
        {
            _crc16Ccitt= new Crc16Ccitt(0xFFFF, 0x1021);
        }


        protected override byte[] CrcAlgoritm(IReadOnlyList<byte> arg)
        {
            return _crc16Ccitt.Calc(arg);
        }
    }
}