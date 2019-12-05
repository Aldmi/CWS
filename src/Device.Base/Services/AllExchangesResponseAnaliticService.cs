using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;

namespace Domain.Device.Services
{
    /// <summary>
    /// Анализ ответов от всех обменов.
    /// </summary>
    public class AllExchangesResponseAnaliticService
    {
        #region fields
        private readonly ConcurrentDictionary<string, bool?> _dictionary = new ConcurrentDictionary<string, bool?>();
        #endregion


        #region ExchangeRx
        /// <summary>
        /// Событие. Все Обмены завершены.
        /// </summary>
        public ISubject<(bool, bool)> AllExchangeAnaliticDoneRx { get; } = new Subject<(bool, bool)>();  //(allResultSucsses, anyResultSucsses)
        #endregion


        #region ctor
        public AllExchangesResponseAnaliticService(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                _dictionary[key] = null;
            }
        }
        #endregion


        #region Methode

        /// <summary>
        /// Записать результат обмена (ответ).
        /// И выполнить аналитику всех ответов.
        /// </summary>
        /// <param name="key">ключ</param>
        /// <param name="respResult">ответ</param>
        /// <param name="exchangesInfoTuple">флаг isOpen всех обменов. На моменть передачи результата конкретным обменом</param>
        public void SetResponseResult(string key, bool respResult, List<(string key, bool isOpen)> exchangesInfoTuple)
        {
            if (_dictionary.ContainsKey(key))
            {
                _dictionary[key] = respResult;
                DoAnalitic(exchangesInfoTuple);
            }
        }

        /// <summary>
        /// Аналитика результатов обмена.
        /// Васе обмены должны закончится (получить результат) исключая обмены с ЗАКРЫТЫМ транспортом.
        /// И хотя бы 1 обмен должен завершится успешно.
        /// Тогда срабатывает событие AnalyticsDoneRx.
        /// </summary>
        private void DoAnalitic(List<(string key, bool isOpen)> exchangesInfoTuple)
        {
            var notOpenExchnageKey= exchangesInfoTuple.FirstOrDefault(tuple => !tuple.isOpen).key;
            var allResultWithoutNotOpen= _dictionary.Where(pair =>pair.Key != notOpenExchnageKey).Select(pair => pair.Value).ToList();
            var allResultSet= allResultWithoutNotOpen.All(flag => flag.HasValue);            //Все обмены получили результат.
            if (allResultSet)
            {
                var allResultSucsses = allResultWithoutNotOpen.All(flag => flag ?? false);    //Все обмены завершились успешно.
                var anyResultSucsses = allResultWithoutNotOpen.Any(flag => flag ?? false);    //Хотя бы один обмен завершился успешно.
                var tuple = (allResultSucsses, anyResultSucsses);
                ResetAllResult();
                AllExchangeAnaliticDoneRx.OnNext(tuple);
            }
        }

        /// <summary>
        /// Сбросить все значения в null.
        /// </summary>
        private void ResetAllResult()
        {
            foreach (var key in _dictionary.Keys.ToArray())
            {
                _dictionary[key] = null;
            }
        }
        #endregion
    }
}