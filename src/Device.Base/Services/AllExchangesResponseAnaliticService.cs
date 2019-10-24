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
        public void SetResponseResult(string key, bool respResult)
        {
            if (_dictionary.ContainsKey(key))
            {
                _dictionary[key] = respResult;
                DoAnalitic();
            }
        }

        /// <summary>
        /// Аналитика результатов обмена.
        /// Васе обмены должны закончится (получить результат)
        /// И хотя бы 1 обмен должен завершится успешно.
        /// Тогда срабатывает событие AnalyticsDoneRx.
        /// </summary>
        private void DoAnalitic()
        {
            var allResult = _dictionary.Select(pair => pair.Value).ToList();
            var allResultSet = allResult.All(flag => flag.HasValue);            //Все обмены получили результат.
            if (allResultSet)
            {
                var allResultSucsses = allResult.All(flag => flag ?? false);    //Все обмены завершились успешно.
                var anyResultSucsses = allResult.Any(flag => flag ?? false);    //Хотя бы один обмен завершился успешно.
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