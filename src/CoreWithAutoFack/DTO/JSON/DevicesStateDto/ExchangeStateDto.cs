using DAL.Abstract.Entities.Options.Exchange;
using DAL.Abstract.Entities.Options.Exchange.ProvidersOption;
using Exchange.Base.Model;
using Shared.Types;

namespace WebServer.DTO.JSON.DevicesStateDto
{
    public class ExchangeStateDto
    {
        //Статичиские настройки
        public string Key { get; set; }
        public string KeyTransport { get; set; }
        public string ProviderName { get; set; }
        public int NumberErrorTrying { get; set; }                               // Кол-во ошибочных запросов до переоткрытия соединения. IsConnect=false. ReOpenTransport()
        public int NumberTimeoutTrying { get; set; }                             // Кол-во запросов не получивщих ответ за заданное время. IsConnect=false.
        public bool AutoStartCycleFunc { get; set; }

        //Динамические настройки
        public bool IsOpen { get; set; }                                         //Соединение открыто
        public bool IsCycleReopened { get; set; }                                //Соединение в процессе открытия
        public bool IsConnect { get; set; }                                      //Устройсвто на связи по открытому соединению (определяется по правильным ответам от ус-ва)
        public bool IsStartedTransportBg { get; set; }                           //Запущен бекграунд на транспорте
        public bool IsStartedCycleFunc { get; set; }                             //Флаг цикл. обмена
    }
}