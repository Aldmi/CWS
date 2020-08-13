using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Helpers;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers
{
    /// <summary>
    /// Кол-во байт во всей строке (включая вставленое ЗАРАНЕЕ CRC)
    /// </summary>
    public class NbyteLastCalcDepInsH : BaseDepInsH
    {
        public NbyteLastCalcDepInsH(StringInsertModel requiredModel) : base(requiredModel) { }
        
        protected override Result<string> GetInseart(string borderedStr, string format, StringBuilder sbMutable)
        {
            var buf = borderedStr.ConvertStringWithHexEscapeChars2ByteArray(format);
            var fullLenght = buf.Length;
            var resStr = RequiredModel.Ext.CalcFinishValue(fullLenght);
            return Result.Ok(resStr);
        }


        /// <summary>
        /// ТОЛЬКО BorderSubString ОПРЕДЕЛЯЕТ ПОДСТРОКУ ДЛЯ GetInseart
        /// </summary>
        protected override Result<string> GetSubString4Handle(string str)
        {
            return Result.Failure<string>("Для NbyteLastCalcDepInsH подстрока определяется BorderSubString. ОБЯЗАТЕЛЬНО ЗАДАЙТЕ BorderSubString.");
        }
    }
}