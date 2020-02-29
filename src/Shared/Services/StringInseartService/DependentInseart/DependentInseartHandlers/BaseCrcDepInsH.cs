using System;
using System.Text;
using CSharpFunctionalExtensions;
using Shared.CrcCalculate;
using Shared.Enums;

namespace Shared.Services.StringInseartService.DependentInseart.DependentInseartHandlers
{
    public abstract class BaseCrcDepInsH : BaseDepInsH
    {
        protected readonly (string startCh, string endCh, bool includeBorder) Border;
        protected readonly ByteHexDelemiter HexDelemiter;

        protected BaseCrcDepInsH(StringInsertModel requiredModel) : base(requiredModel)
        {
            var partsOptions = requiredModel.FindPartsOptions();
            switch (partsOptions.Count)
            {
                case 1:
                    Border = CrcHelper.CalcBorderSubString(partsOptions[0]);
                    break;

                case 2:
                    Border = CrcHelper.CalcBorderSubString(partsOptions[0]);
                    Enum.TryParse(partsOptions[1], out HexDelemiter);
                    break;
            }
        }
    }
}