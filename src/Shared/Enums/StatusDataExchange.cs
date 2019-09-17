namespace Shared.Enums
{
    /// <summary>
    /// статус ПОСЛЕДНЕГО обмена данными
    /// </summary>
    public enum StatusDataExchange : byte
    {
        None,
        NotOpenTransport,
        Start,
        Process,
        End,
        EndWithTimeout,
        EndWithCanceled,
        EndWithErrorCannotSendData,
        EndWithErrorWrongAnswers,
        EndWithTimeoutCritical,
        EndWithErrorCritical
    }
}