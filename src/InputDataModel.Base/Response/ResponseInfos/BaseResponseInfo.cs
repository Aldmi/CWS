namespace Domain.InputDataModel.Base.Response.ResponseInfos
{
    /// <summary>
    /// Вся информация про ответ
    /// </summary>
    public abstract class BaseResponseInfo
    {
        public bool IsOutDataValid { get; protected set; }                       //Флаг валидности ответа
    }
}