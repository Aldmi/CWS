namespace Domain.InputDataModel.Base.Response
{
    /// <summary>
    /// Базовый тип для создания строго типизированных ответов из строкового представления ответа.
    /// </summary>
    public class StronglyTypedRespBase
    {
        public bool IsValid { get; set; }
        public string Status { get; set; }


        public override string ToString()
        {
            return $"IsValid = {IsValid}  Status= {Status}";
        }
    }
}