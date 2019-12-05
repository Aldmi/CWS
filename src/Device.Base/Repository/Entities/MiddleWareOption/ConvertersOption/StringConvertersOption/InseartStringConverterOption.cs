using System.Collections.Generic;

namespace Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.StringConvertersOption
{
    /// <summary>
    /// Вставка подстрок по указаныым индексам в строку.
    /// </summary>
    public class InseartStringConverterOption : BaseConverterOption
    {
        public Dictionary<int, string> InseartDict { get; set; }
    }
}