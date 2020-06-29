using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers
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
            
            //TODO: вычислять borderedString
           // var string4HandleRes = CalcSubString4Handle(sb);

            var (isSuccess, _, value, error) = GetInseart(sb, format);
            var sbAfterInseart= sb.ReplaceFirstOccurrence(RequiredModel.Replacement, value);
            return isSuccess ?
                Result.Ok(sbAfterInseart) :
                Result.Failure<StringBuilder>(error);
        }



        //private Result<string> CalcSubString4Handle(StringBuilder sb)
        //{
        //    var str = sb.ToString();
        //    if (RequiredModel.Ext.BorderSubString != null)
        //    {
        //        var res= RequiredModel.Ext.BorderSubString.Calc(str);
        //        return res;
        //    }

        //    return GetSubString4Handle(sb);
        //}


        /// <summary>
        /// АЛГОРИТМ зависмой подстановки
        /// </summary>
        protected abstract Result<string> GetInseart(StringBuilder sb, string format);

        /// <summary>
        /// Если BorderSubString не задан форматом.
        /// Потомок возвращает нужную подстроку для вычисления.
        /// </summary>
        // protected abstract Result<string> GetSubString4Handle(StringBuilder sb);
    }
}