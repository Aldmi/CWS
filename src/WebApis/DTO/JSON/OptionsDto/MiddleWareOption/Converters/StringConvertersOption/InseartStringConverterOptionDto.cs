using System.Collections.Generic;
using DAL.Abstract.Entities.Options.MiddleWare.Converters;

namespace WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption.Converters.StringConvertersOption
{
    /// <summary>
    /// Вставка подстрок по указаныым индексам в строку.
    /// </summary>
    public class InseartStringConverterOptionDto : BaseConverterOptionDto
    {
        public Dictionary<int, string> InseartDict { get; set; }
    }
}