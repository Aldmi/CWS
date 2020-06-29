using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers
{
    public class NumberOfCharactersDepInsH : BaseDepInsH
    {
        public NumberOfCharactersDepInsH(StringInsertModel requiredModel) : base(requiredModel) { }


        /// <summary>
        /// </summary>
        /// <param name="borderedStr"></param>
        /// <param name="format"></param>
        /// <param name="sbMutable">Необходимо удалить симолы \" в базовой строке при вычмслении numberOfCharactersBetweenBrackets</param>
        /// <returns></returns>
        protected override Result<string> GetInseart(string borderedStr, string format, StringBuilder sbMutable)
        {
            //Удалить маркерные симолы, выделяющие зону подсчета NumberOfCharacters \"
            const string bracket = "\\\""; // \"
            var (_, numberOfCharactersBetweenBrackets) = sbMutable.ReplaceBrackets(bracket, bracket, "");
            var resStr = RequiredModel.Ext.CalcFinishValue(numberOfCharactersBetweenBrackets);
            return Result.Ok(resStr);
        }



        /// <summary>
        /// NOT USE
        /// </summary>
        protected override Result<string> GetSubString4Handle(string str)
        {
            return Result.Ok(str);
        }
    }
}