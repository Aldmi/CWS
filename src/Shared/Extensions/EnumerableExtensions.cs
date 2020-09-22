using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Serilog;
using Shared.Types;

namespace Shared.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Выполняет последовательно над коллекцией операции указанные в AgregateFilter.
        ///  Where->OredBy->Take с дополнением
        /// Полученные коллекции объединяются в один список.
        /// Если inData пустой список, то  Where->OredBy не РАБОТАЕТ, только дополнение Take.
        /// </summary>
        /// <returns>Возвращает объединенный список всех, после всех фильтров</returns>
        public static IEnumerable<TInput> Filter<TInput>(this IEnumerable<TInput> inData, AgregateFilter filter, string defaultItemJson, ILogger logger = null)
        {
            if (inData == null)
                return new List<TInput>();

            var items = inData;
            var agregateList = new List<TInput>();
            foreach (var p in filter.Filters)
            {
                //1. Where-------------------------------------
                var where = p.Where;
                var whereLenghtConditionFunc = p.WhereLenght.HasValue
                    ? new Func<bool>(() => items.Count() == p.WhereLenght.Value)
                    : () => true;
                items = inData.Filter(where, logger);
                var filterConditions = items.Any() && whereLenghtConditionFunc();
                if (!filterConditions)
                    continue;
                //2. OredBy------------------------------------
                var oredBy = p.OrderBy;
                if (!string.IsNullOrEmpty(oredBy))
                {
                    items = items.Order(oredBy, logger);
                }
                //3. Take с дополнением-----------------------------
                var take = p.Take ?? 1000;
                items = items.TakeItems(take, defaultItemJson, logger);
                agregateList.AddRange(items);
            }
            return agregateList;
        }


        /// <summary>
        /// Если inData пустой список, то Take в каждом фильтре формируеут список из defaultItemJson элементов.
        /// Фильтрация и Упорядочевание не применяются.
        /// </summary>
        /// <returns>Созданный список</returns>
        public static IEnumerable<TInput> TakeItemIfEmpty<TInput>(this IEnumerable<TInput> inData, AgregateFilter filter, string defaultItemJson, ILogger logger = null)
        {
            if (inData.Any())
                return inData;

            var agregateList = new List<TInput>();
            foreach (var p in filter.Filters)
            {
                //1. Take с дополнением-----------------------------
                var take = p.Take ?? 1000;
                var items = inData.TakeItems(take, defaultItemJson, logger);
                agregateList.AddRange(items);
            }
            return agregateList;
        }


        /// <summary>
        /// ФИЛЬТРАЦИЯ элементов.
        /// </summary>
        /// <param name="inData">данные</param>
        /// <param name="whereFilter">вражения DynamicLinq для фильтрации</param>
        /// <param name="logger">ОПЦИОНАЛЬНО logger</param>
        /// <returns></returns>
        public static IEnumerable<TInput> Filter<TInput>(this IEnumerable<TInput> inData, string whereFilter, ILogger logger = null)
        {
            if (inData == null)
                return new List<TInput>();

            try
            {
                //ПРИМЕНИТЬ ФИЛЬТР И УПОРЯДОЧЕВАНИЕ
                var filtred = inData.AsQueryable().Where(whereFilter).ToList();
                return filtred;
            }
            catch (Exception ex)
            {
                logger?.Warning($"Filter In Data Exception: {ex}");
                return null;
            }
        }


        /// <summary>
        /// УПОРЯДОЧИТЬ элементы.
        /// </summary>
        /// <param name="inData">данные</param>
        /// <param name="orderBy">поле для упорядочевания</param>
        /// <param name="logger">ОПЦИОНАЛЬНО logger</param>
        /// <returns></returns>
        public static IEnumerable<TInput> Order<TInput>(this IEnumerable<TInput> inData, string orderBy, ILogger logger = null)
        {
            if (inData == null)
                return new List<TInput>();

            try
            {
                return inData.AsQueryable().OrderBy(orderBy).ToList();
            }
            catch (Exception ex)
            {
                logger?.Warning($"Filter In Data Exception: {ex}");
                return null;
            }
        }


        /// <summary>
        /// ВЗЯТЬ TakeItems ИЛИ ДОПОЛНИТЬ ДО TakeItems.
        /// Если 
        /// </summary>
        /// <param name="inData">данные</param>
        /// <param name="takeItems">кол-во первых элементов</param>
        /// <param name="defaultItemJson">тип по умолчанию в формате JSON</param>
        /// <param name="logger">ОПЦИОНАЛЬНО logger</param>
        /// <returns></returns>
        public static IEnumerable<TInput> TakeItems<TInput>(this IEnumerable<TInput> inData, int takeItems, string defaultItemJson, ILogger logger = null)
        {
            var inDataList = inData == null ? new List<TInput>() : inData.ToList();
            try
            {
                var defaultItem = JsonConvert.DeserializeObject<TInput>(defaultItemJson);
                var takedItems = Enumerable.Repeat(defaultItem, takeItems).ToArray();
                var endPosition = (takeItems < inDataList.Count) ? takeItems : inDataList.Count;
                inDataList.CopyTo(0, takedItems, 0, endPosition);
                return takedItems;
            }
            catch (Exception ex)
            {
                logger?.Warning($"Filter In Data Exception: {ex}");
                return null;
            }
        }


        /// <summary>
        /// Разбивает коллекцию на порции (батчи)
        /// </summary>
        /// <param name="source">коллекция</param>
        /// <param name="size">размер батча</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int size)
        {
            using (var iter = source.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    var chunk = new T[size];
                    var count = 1;
                    chunk[0] = iter.Current;
                    for (int i = 1; i < size && iter.MoveNext(); i++)
                    {
                        chunk[i] = iter.Current;
                        count++;
                    }
                    if (count < size)
                    {
                        Array.Resize(ref chunk, count);
                    }
                    yield return chunk;
                }
            }
        }

        /// <summary>
        /// Возвращает следующий за item элемент коллекции items.
        /// Если item  - последний элемент коллекции, то вернуть первый элемент.
        /// </summary>
        /// <param name="items">Коллекция</param>
        /// <param name="item">Элемент</param>
        /// <returns>Следующий элемент коллекции</returns>
        public static T GetSecondItemInCycle<T>(this ReadOnlyCollection<T> items, T item)
        {
            var i = items.IndexOf(item);
            if (++i >= items.Count)
            {
                return items[0];
            }
            return items[i];
        }
    }
}