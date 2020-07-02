using System;
using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;


namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers
{
    /// <summary>
    /// Подсчет СИМВОЛОВ Nchar. Подстрока определяется границами.
    /// </summary>
    public class NcharDepInsH : BaseDepInsH
    {
        public NcharDepInsH(StringInsertModel requiredModel) : base(requiredModel) { }


        protected override Result<string> GetInseart(string borderedStr, string format, StringBuilder sbMutable)
        {
            var lenght = borderedStr.Length;
            var resStr = RequiredModel.Ext.CalcFinishValue(lenght);
            return Result.Ok(resStr);
        }


        /// <summary>
        /// ТОЛЬКО BorderSubString ОПРЕДЕЛЯЕТ ПОДСТРОКУ ДЛЯ GetInseart
        /// </summary>
        protected override Result<string> GetSubString4Handle(string str)
        {
            return Result.Failure<string>("Для NcharDepInsH подстрока определяется BorderSubString. ОБЯЗАТЕЛЬНО ЗАДАЙТЕ BorderSubString.");
        }
    }
}