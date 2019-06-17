namespace DAL.EFCore.Entities.MiddleWare.Converters.StringConvertersOption
{
    /// <summary>
    /// Обрезка строки, если превышает Limit
    /// </summary>
    public class EfLimitStringConverterOption
    {
        public int Limit { get; set; }
    }
}