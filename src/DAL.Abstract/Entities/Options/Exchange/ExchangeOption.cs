using DAL.Abstract.Entities.Options.Exchange.ProvidersOption;
using Shared.Collections;
using Shared.Types;

namespace DAL.Abstract.Entities.Options.Exchange
{
    public class ExchangeOption : EntityBase
    {
        public string Key { get; set; }
        public KeyTransport KeyTransport { get; set; }
        public ProviderOption Provider { get; set; }

        public int NumberErrorTrying { get; set; }         // Кол-во ошибочных запросов до переоткрытия соединения. IsConnect=false. ReOpenTransport()
        public int NumberTimeoutTrying { get; set; }       // Кол-во запросов не получивщих ответ за заданное время. IsConnect=false.

        /// <summary>
        /// Добавление функции циклического обмена на бекгроунд
        /// Флаг учитывается, только при старте сервис.
        /// </summary>
        public bool AutoStartCycleFunc { get; set; }

        /// <summary>
        /// Опции очереди данных для цикл. обмена
        /// </summary>
        public QueueOption CycleQueueOption { get; set; }
    }
}