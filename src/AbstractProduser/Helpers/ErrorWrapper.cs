using System;

namespace AbstractProduser.Helpers
{

    public class ErrorWrapper
    {
        #region fields

        public readonly ResultError ResultError;
        public readonly string ErrorStr;
        public readonly Exception Exception;

        #endregion



        #region ctor

        public ErrorWrapper(ResultError resultError)
        {
            this.ResultError = resultError;
        }

        public ErrorWrapper(ResultError resultError, Exception exception) : this(resultError)
        {
            Exception = exception;
        }

        public ErrorWrapper(ResultError resultError, string errorStr) : this(resultError)
        {
            ErrorStr = errorStr;
        }

        #endregion



        public override string ToString()
        {
            var errorStr = string.IsNullOrEmpty(ErrorStr) ? string.Empty : ErrorStr;
            var exceptionStr = Exception?.ToString() ?? string.Empty;
            return $"{ResultError}  {errorStr}  {exceptionStr}";
        }
    }


    public enum ResultError
    {
        Trottling,
        SendException,
        Timeout,
        RespawnProduserError,
        NoClientBySending
    }
}