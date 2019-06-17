using System.Collections.Generic;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption
{
    /// <summary>
    /// Вставка подстрок по указаныым индексам в строку.
    /// </summary>
    public class InseartStringConverterOptionDto
    {
        public Dictionary<int, string> InseartDict { get; set; }
    }
}