using System;
using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;
using Shared.Helpers;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers
{
    /// <summary>
    /// Кол-во байт во всей строке (включая вставленные байты CRC)
    /// </summary>
    public class NbyteFullLastCalcDepInsH : BaseDepInsH
    {
        private const int LenghtNbyteFullLastCalcInbytes = 1;

        public NbyteFullLastCalcDepInsH(StringInsertModel requiredModel) : base(requiredModel) { }
        
        protected override Result<string> GetInseart(StringBuilder sb, string format)
        {
            var str = sb.ToString();
            var res = StringInsertModel.CalcSubStringBeetweenModelAndEndString(str, RequiredModel);
            if (res.IsFailure)
                return res;

            var subStr = res.Value;
            var buf = subStr.ConvertStringWithHexEscapeChars2ByteArray(format);
            var fullLenght = buf.Length + LenghtNbyteFullLastCalcInbytes;
            var resStr = RequiredModel.Ext.CalcFinishValue(fullLenght);
            return Result.Ok(resStr);
        }
    }
}