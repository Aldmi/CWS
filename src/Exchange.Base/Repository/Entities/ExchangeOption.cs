using Domain.InputDataModel.Base.ProvidersOption;
using Infrastructure.Dal.Abstract;
using Shared.Collections;
using Shared.Types;

namespace Domain.Exchange.Repository.Entities
{
    public class ExchangeOption : EntityBase
    {
        public string Key { get; set; }
        public KeyTransport KeyTransport { get; set; }
        public ProviderOption Provider { get; set; }

        public int NumberErrorTrying { get; set; }                // Кол-во ошибочных запросов до переоткрытия соединения. IsConnect=false. ReOpenTransport()
        public int NumberTimeoutTrying { get; set; }              // Кол-во запросов не получивщих ответ за заданное время. IsConnect=false.

        /// <summary>
        /// Добавление функции циклического обмена на бекгроунд
        /// Флаг учитывается, только при старте сервис.
        /// </summary>
        public CycleFuncOption CycleFuncOption { get; set; }
    }


    public class CycleFuncOption
    {
        /// <summary>
        /// Добавление функции циклического обмена на бекгроунд
        /// Флаг учитывается, только при старте сервис.
        /// </summary>
        public bool AutoStartCycleFunc { get; set; }

        /// <summary>
        /// Интервал в течении которого пропускаем вызов цикл. функции на обмене
        /// </summary>
        public int SkipInterval { get; set; }

        /// <summary>
        /// Допустимое время обновления цикл. данных
        /// </summary>
        public int NormalIntervalCycleDataEntry { get; set; }

        /// <summary>
        /// Опции очереди данных для цикл. обмена
        /// </summary>
        public QueueMode CycleQueueMode { get; set; }
    }
}