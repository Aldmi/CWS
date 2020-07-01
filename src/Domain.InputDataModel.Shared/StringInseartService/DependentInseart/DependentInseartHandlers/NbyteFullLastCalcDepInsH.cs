using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Helpers;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers
{
    /// <summary>
    /// Кол-во байт во всей строке (включая вставленое ЗАРАНЕЕ CRC)
    /// </summary>
    public class NbyteFullLastCalcDepInsH : BaseDepInsH
    {
        private const int LenghtNbyteFullLastCalcInbytes = 1;

        public NbyteFullLastCalcDepInsH(StringInsertModel requiredModel) : base(requiredModel) { }
        
        protected override Result<string> GetInseart(string borderedStr, string format, StringBuilder sbMutable)
        {
            var buf = borderedStr.ConvertStringWithHexEscapeChars2ByteArray(format);
            var fullLenght = buf.Length + LenghtNbyteFullLastCalcInbytes;
            var resStr = RequiredModel.Ext.CalcFinishValue(fullLenght);
            return Result.Ok(resStr);
        }


        /// <summary>
        /// ТОЛЬКО BorderSubString ОПРЕДЕЛЯЕТ ПОДСТРОКУ ДЛЯ GetInseart
        /// </summary>
        protected override Result<string> GetSubString4Handle(string str)
        {
            return Result.Failure<string>("Для NbyteFullLastCalcDepInsH подстрока определяется BorderSubString. ОБЯЗАТЕЛЬНО ЗАДАЙТЕ BorderSubString.");
            //var res = RequiredModel.CalcSubStringBeetween2Models(_crcModel, str);
            //return res;
        }
    }
}