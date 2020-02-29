using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Serilog;

namespace Shared.Extensions
{

    //TODO: Вынести в отдельный файл
    public class AgregateFilter
    {
        public List<Predicate> Filters { get; set; }
    }

    public class Predicate
    {
        public string Where { get; set; }             //Булевое выражение для фильтрации (например "(ArrivalTime > DateTime.Now.AddMinute(-100) && ArrivalTime < DateTime.Now.AddMinute(100)) || ((EventTrain == \"Transit\") && (ArrivalTime > DateTime.Now))")
        public int? Take { get; set; }
    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<TInput> Filter<TInput>(this IEnumerable<TInput> inData, AgregateFilter whereFilter, ILogger logger = null)
        {
            if (inData == null)
                return new List<TInput>();

            var agregateList = new List<TInput>();
            foreach (var filter in whereFilter.Filters)
            {
                var where = filter.Where;
                var take = filter.Take ?? 1000;
                var items= inData.Filter(filter.Where, logger).Take(take);
                agregateList.AddRange(items);
            }
            return agregateList;//DEBUG
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

            var now = DateTime.Now;
            try
            {
                //ЗАМЕНА  DateTime.Now.AddMinute(...)---------------------------
                var pattern = @"DateTime\.Now\.AddMinute\(([^()]*)\)";
                var where = whereFilter;
                where = Regex.Replace(where, pattern, x =>
                {
                    var val = x.Groups[1].Value;
                    if (int.TryParse(val, out var min))
                    {
                        var date = now.AddMinutes(min);
                        return $"DateTime({date.Year}, {date.Month}, {date.Day}, {date.Hour}, {date.Minute}, 0)";
                    }
                    return x.Value;
                });
                //ЗАМЕНА  DateTime.Now----------------------------------------
                pattern = @"DateTime.Now";
                where = Regex.Replace(where, pattern, x =>
                {
                    var date = now;
                    return $"DateTime({date.Year}, {date.Month}, {date.Day}, {date.Hour}, {date.Minute}, 0)";
                });
                //ПРИМЕНИТЬ ФИЛЬТР И УПОРЯДОЧЕВАНИЕ
                var filtred = inData.AsQueryable().Where(where).ToList();
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
    }
}