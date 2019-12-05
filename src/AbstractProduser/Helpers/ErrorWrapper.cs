using System;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Produser.AbstractProduser.Enums;

namespace Infrastructure.Produser.AbstractProduser.Helpers
{

    public class ErrorWrapper
    {
        #region fields

        public readonly string Key;
        public readonly ResultError ResultError;
        public readonly string ErrorStr;
        public readonly Exception Exception;

        #endregion



        #region ctor

        public ErrorWrapper(string key, ResultError resultError)
        {
            if (string.IsNullOrEmpty(key))
                throw new ValidationException("Key не указан");

            Key = key;
            this.ResultError = resultError;
        }

        public ErrorWrapper(string key, ResultError resultError, Exception exception) : this(key, resultError)
        {
            Exception = exception;
        }

        public ErrorWrapper(string key, ResultError resultError, string errorStr) : this(key, resultError)
        {
            ErrorStr = errorStr;
        }

        #endregion


        public override string ToString()
        {
            var errorStr = string.IsNullOrEmpty(ErrorStr) ? string.Empty : ErrorStr;
            var exceptionStr = Exception?.ToString() ?? string.Empty;
            return $"Key= \"{Key}\" {ResultError}  {errorStr}  {exceptionStr}";
        }
    }

}