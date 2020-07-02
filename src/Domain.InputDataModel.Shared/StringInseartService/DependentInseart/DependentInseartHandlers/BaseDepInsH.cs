using System;
using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Shared.Extensions;

namespace Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers
{
    public abstract class BaseDepInsH
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
            if (!sb.Contains(RequiredModel.Replacement))
                return Result.Failure<StringBuilder>($"Обработчик Dependency Inseart не может найти Replacement переменную {RequiredModel.Replacement} в строке {sb}");

            var (_, isFailureSubStr, subStrValue, errorSubStr) = CalcSubString4Handle(sb.ToString());
            if(isFailureSubStr)
                return Result.Failure<StringBuilder>(errorSubStr);

            var (isSuccess, _, value, error) = GetInseart(subStrValue, format, sb);
            var sbAfterInseart = sb.ReplaceFirstOccurrence(RequiredModel.Replacement, value);
            return isSuccess ?
                Result.Ok(sbAfterInseart) :
                Result.Failure<StringBuilder>(error);
        }


        /// <summary>
        /// Вычислить строку для обработки.
        /// Если указан BorderSubString в формате, то вычисляем строку по нему.
        /// Иначе Потомок определяет подстроку 
        /// </summary>
        private Result<string> CalcSubString4Handle(string str)
        {
            //TODO: ??? убирать из строки все вставки {} там где это нужнго, например для вычисления BorderSubString
            //1. ЕСЛИ УКАЗАН BorderSubString => ВЫЧИСЛИМ ПОДСТРОКУ С ЕГО ПОМОЩЬЮ
            if (RequiredModel.Ext.BorderSubString != null)
            {
                var res = RequiredModel.Ext.BorderSubString.Calc(str, RequiredModel.Replacement);
                return res;
            }

            //2. ВЫЧИСЛЯЕТСЯ ПОТОМКОМ
            var resChild = GetSubString4Handle(str);
            return resChild;
        }


        /// <summary>
        /// АЛГОРИТМ зависмой подстановки
        /// </summary>
        /// <param name="borderedStr">строка для вычисления</param>
        /// <param name="format">формат</param>
        /// <param name="sbMutable">базовая строка для изменения</param>
        /// <returns></returns>
        protected abstract Result<string> GetInseart(string borderedStr, string format, StringBuilder sbMutable);


        /// <summary>
        /// Если BorderSubString не задан форматом. Потомок возвращает нужную подстроку для вычисления.
        /// </summary>
        protected abstract Result<string> GetSubString4Handle(string str);
    }
}