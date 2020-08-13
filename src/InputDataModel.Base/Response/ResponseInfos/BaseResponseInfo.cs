using Shared.Helpers;

namespace Domain.InputDataModel.Base.Response.ResponseInfos
{
    /// <summary>
    /// Вся информация про ответ
    /// </summary>
    public abstract class BaseResponseInfo
    {
        public bool IsOutDataValid { get; protected set; }                       //Флаг валидности ответа

        public override string ToString()
        {
            return $"Valid: {IsOutDataValid}  Type: {GetType().Name}";
        }

        public object GetResponseData()
        {
            var envelope = new
            {
                IsOutDataValid,
                data= GetData()
            };
            return envelope;
        }

        protected virtual object GetData()
        {
            return null;
        }
    }
}