namespace Domain.InputDataModel.Base.ProvidersOption
{
    /// <summary>
    /// Конкретный провайдер обмена, захардкоженный (указать имя)
    /// </summary>
    public class ManualProviderOption
    {
        public string Address { get; set; }
        public int TimeRespone { get; set; }
    }
}