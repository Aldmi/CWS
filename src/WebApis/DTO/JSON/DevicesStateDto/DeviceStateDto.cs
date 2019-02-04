using System.Collections.Generic;
using DAL.Abstract.Entities.Options.Device;

namespace WebApiSwc.DTO.JSON.DevicesStateDto
{
    public class DeviceStateDto
    {
        //Статичиские настройки
        public DeviceOption Option { get; set; }
        public List<ExchangeStateDto> Exchanges { get; set; }

        //Динамические настройки
        //TODO: добавить ответы от устройства (kafka, log, mail, ...)
    }

}