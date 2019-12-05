using System.Collections.Generic;
using Domain.Device.Repository.Entities;
using Domain.Exchange.Repository.Entities;

namespace App.Services.Agregators
{
    public class OptionAgregator
    {
        public List<DeviceOption> DeviceOptions { get; set; }
        public List<ExchangeOption> ExchangeOptions { get; set; }
        public TransportOption TransportOptions { get; set; }
    }
}