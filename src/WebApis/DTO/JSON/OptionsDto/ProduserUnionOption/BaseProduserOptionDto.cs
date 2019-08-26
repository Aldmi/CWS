using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.ProduserUnionOption
{
    public class BaseProduserOptionDto
    {
        [Required(ErrorMessage = "Укажите ключ провайдера")]
        public string Key { get; set; }
        public TimeSpan TimeRequest { get; set; }

        [Range(1, 1000, ErrorMessage = "TrottlingQuantity не попал в диапазон 1...1000")]
        public int TrottlingQuantity { get; set; }
    }
}