﻿using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.CrcCalculate;
using Shared.Extensions;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers
{
    public class CrcXorDepInsH : BaseCrcDepInsH
    {
        public CrcXorDepInsH(StringInsertModel requiredModel) : base(requiredModel){}

        protected override Result<string> GetInseart(StringBuilder sb, string format)
        { 
           var (_, isFailure, arr, error) = CrcHelper.CalcCrc(sb, Border, format, RequiredModel.Replacement, CrcCalc.CalcXor);
           if (isFailure)
                return Result.Failure<string>(error);

           var resStr = arr.BitConverter2StrByFormat(RequiredModel.Format, HexDelemiter);
           return Result.Ok<string>(resStr);
        }
    }
}