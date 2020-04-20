using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers
{
    public class NumberOfCharactersDepInsH : BaseDepInsH
    {
        public NumberOfCharactersDepInsH(StringInsertModel requiredModel) : base(requiredModel) { }
        
        protected override Result<string> GetInseart(StringBuilder sb, string format)
        { 
           //Удалить маркерные симолы, выделяющие зону подсчета NumberOfCharacters \"
           const string bracket = "\\\""; // \"
           var (_, numberOfCharactersBetweenBrackets) = sb.ReplaceBrackets(bracket, bracket, "");
           var resStr = numberOfCharactersBetweenBrackets.Convert2StrByFormat(RequiredModel.Format);
           return Result.Ok(resStr);
        }
    }
}