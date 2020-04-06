namespace Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.StringConvertersOption
{
    /// <summary>
    /// Обрезка строки, если превышает Limit
    /// </summary>
    public class LimitStringConverterOption
    {
        public int Limit { get; set; }
    }
}