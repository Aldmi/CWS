using System.Collections.Generic;

namespace DAL.EFCore.Entities.MiddleWare.Converters.StringConvertersOption
{
    /// <summary>
    /// Вставка подстрок по указаныым индексам в строку.
    /// </summary>
    public class EfInseartStringConverterOption
    {
        public Dictionary<int, string> InseartDict { get; set; }
    }
}