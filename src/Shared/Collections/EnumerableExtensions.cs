using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Serilog;

namespace Shared.Collections
{
    public static class EnumerableExtensions
    {
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
    }
}