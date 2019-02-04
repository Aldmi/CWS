using System.Collections.Generic;
using WebApiSwc.DTO.JSON.OptionsDto.DeviceOption;
using WebApiSwc.DTO.JSON.OptionsDto.ExchangeOption;
using WebApiSwc.DTO.JSON.OptionsDto.TransportOption;

namespace WebApiSwc.DTO.JSON.OptionsDto
{
    public class OptionAgregatorDto
    {
        public List<DeviceOptionDto> DeviceOptions { get; set; }   
        public List<ExchangeOptionDto> ExchangeOptions { get; set; }   
        public TransportOptionsDto TransportOptions { get; set; }   
    }
}