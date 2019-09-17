using System.Collections.Generic;

namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.StringConvertersOption
{
    /// <summary>
    /// Вставка подстрок по указаныым индексам в строку.
    /// </summary>
    public class EfInseartStringConverterOption : EfBaseConverterOption
    {
        public Dictionary<int, string> InseartDict { get; set; }
    }
}