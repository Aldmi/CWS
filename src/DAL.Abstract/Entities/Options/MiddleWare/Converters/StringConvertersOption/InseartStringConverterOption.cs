using System.Collections.Generic;

namespace DAL.Abstract.Entities.Options.MiddleWare.Converters.StringConvertersOption
{
    /// <summary>
    /// Вставка подстрок по указаныым индексам в строку.
    /// </summary>
    public class InseartStringConverterOption
    {
        public Dictionary<int, string> InseartDict { get; set; }
    }
}