namespace DAL.Abstract.Entities.Options.MiddleWare.Hadlers.StringConvertersOption
{
    /// <summary>
    /// Обрезка строки, если превышает Limit
    /// </summary>
    public class LimitStringConverterOption
    {
        public int Limit { get; set; }
    }
}