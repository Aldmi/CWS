﻿using System.ComponentModel.DataAnnotations;
using Domain.Exchange.Repository.Entities;
using WebApiSwc.DTO.JSON.OptionsDto.ExchangeOption.ProvidersOption;

namespace WebApiSwc.DTO.JSON.OptionsDto.ExchangeOption
{
    public class ExchangeOptionDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Key для Exchange не может быть NULL")]
        public string Key { get; set; }

        [Required(ErrorMessage = "KeyTransport для Exchange не может быть NULL")]
        public KeyTransportDto KeyTransport { get; set; }

        [Required(ErrorMessage = "Provider для Exchange не может быть NULL")]
        public ProviderOptionDto Provider { get; set; }

        public int NumberErrorTrying { get; set; }
        public int NumberTimeoutTrying { get; set; }

        public CycleFuncOption CycleFuncOption { get; set; }
    }
}