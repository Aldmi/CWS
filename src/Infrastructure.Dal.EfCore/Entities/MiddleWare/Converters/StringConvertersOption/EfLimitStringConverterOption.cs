namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.StringConvertersOption
{
    /// <summary>
    /// Обрезка строки, если превышает Limit
    /// </summary>
    public class EfLimitStringConverterOption : EfBaseConverterOption
    {
        public int Limit { get; set; }
    }
}