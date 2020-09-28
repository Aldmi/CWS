using System;
using CSharpFunctionalExtensions;
using KellermanSoftware.CompareNetObjects;

namespace Domain.InputDataModel.Base.Response.Comparators
{

    /// <summary>
    /// Проверка изменения данных.
    /// СВ-Ва для сравнения
    /// ResponsePieceOfDataWrapper.IsValidAll
    /// ResponsePieceOfDataWrapper.ResponsesItems
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    public class ResponsePieceOfDataWrapperComparator<TIn> : IEquatable<ResponsePieceOfDataWrapper<TIn>>
    {
        private ResponsePieceOfDataWrapper<TIn> _oldValue;
        public bool Equals(ResponsePieceOfDataWrapper<TIn> value)
        {
            if (_oldValue == null)
            {
                _oldValue = value;
                return true;
            }

            if (value == null)
                throw new NullReferenceException("агрумент не может быть null");

            if (ReferenceEquals(_oldValue,  value))
                return true;

            var isValidCmp = _oldValue.Evaluation.IsValidAll == value.Evaluation.IsValidAll;
            var (isSuccess, _) = Comparer(_oldValue.ResponsesItems, value.ResponsesItems);
            var equal= isValidCmp && isSuccess;
            if (!equal)
                _oldValue = value;

            return equal;
        }


        private static Result<bool> Comparer(object obj1, object obj2)
        {
            var compareLogic = new CompareLogic { Config = { MaxMillisecondsDateDifference = 1000 } };
            ComparisonResult result = compareLogic.Compare(obj1, obj2);
            return result.AreEqual ? Result.Ok(true) : Result.Failure<bool>(result.DifferencesString);
        }
    }
}