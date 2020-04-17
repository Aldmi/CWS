using System.Linq;
using System.Text;
using CSharpFunctionalExtensions;
using Shared.Extensions;
using Shared.MiddleWares;

namespace Shared.Services.StringInseartService.DependentInseart.DependentInseartHandlers
{
    public abstract  class BaseDepInsH
    {
        protected readonly StringInsertModel RequiredModel;

        protected BaseDepInsH(StringInsertModel requiredModel)
        {
            RequiredModel = requiredModel;
        }


        /// <summary>
        /// Вычислить ЗАВИСМУЮ подстановку.
        /// </summary>
        /// <param name="sb">входная строка</param>
        /// <param name="format">кодировка строки для перевода в byte[]</param>
        /// <returns>кортеж: результат подстановки и модель подстановки</returns>
        public Result<StringBuilder> CalcInsert(StringBuilder sb, string format = null)
        {
            if(!sb.Contains(RequiredModel.Replacement))
                return Result.Failure<StringBuilder>($"Обработчик Dependency Inseart не может найти Replacement переменную {RequiredModel.Replacement} в строке {sb}");
            
            var (isSuccess, _, value, error) = GetInseart(sb, format);
            var sbAfterInseart= sb.ReplaceFirstOccurrence(RequiredModel.Replacement, value);
            return isSuccess ?
                Result.Ok(sbAfterInseart) :
                Result.Failure<StringBuilder>(error);
        }
        
        
        /// <summary>
        /// АЛГОРИТМ зависмой подстановки
        /// </summary>
        protected abstract Result<string> GetInseart(StringBuilder sb, string format);
    }
}