using System;

namespace AbstractProduser.Helpers
{

    public class ErrorWrapper
    {
        public readonly ResultError DequeueResultError;
        public readonly string _errorStr;
        public readonly Exception _exception;



        #region ctor

        public ErrorWrapper(ResultError dequeueResultError)
        {
            DequeueResultError = dequeueResultError;
        }

        public ErrorWrapper(ResultError dequeueResultError, Exception exception) : this(dequeueResultError)
        {
            _exception = exception;
        }

        public ErrorWrapper(ResultError dequeueResultError, string errorStr) : this(dequeueResultError)
        {
            _errorStr = errorStr;
        }

        #endregion
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