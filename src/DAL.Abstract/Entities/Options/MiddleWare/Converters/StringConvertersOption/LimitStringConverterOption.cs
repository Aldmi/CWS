namespace DAL.Abstract.Entities.Options.MiddleWare.Converters.StringConvertersOption
{
    /// <summary>
    /// Обрезка строки, если превышает Limit
    /// </summary>
    public class LimitStringConverterOption
    {
        public int Limit { get; set; }
    }
}