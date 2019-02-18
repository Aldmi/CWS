﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using Exchange.Base.Model;
using InputDataModel.Autodictor.Model;
using InputDataModel.Base;
using Newtonsoft.Json;
using Serilog;

namespace InputDataModel.Autodictor.DataProviders
{
    public class BaseDataProvider
    {
        private readonly ILogger _logger;



        #region ctor

        public BaseDataProvider(ILogger logger)
        {
            _logger = logger;
        }

        #endregion



        ///// <summary>
        ///// Определяет обработчик входных данных.
        ///// Команда или Данные.
        ///// </summary>
        ///// <param name="command">Идентификатор команды в входных данных</param>
        ///// <param name="handlerName">Имя обработчика входных данных</param>
        ///// <returns></returns>
        //public RuleSwitcher4InData SwitchInDataHandler(Command4Device command, string handlerName)
        //{
        //    var commandPrefix = "Command_";
        //    var commandName = $"{commandPrefix}{command.ToString()}";  //Command_On, Command_Off, Command_Restart, Command_Clear
        //    if(commandName.Equals(handlerName))
        //        return RuleSwitcher4InData.CommandHanler;

        //    if((command == Command4Device.None) && (!handlerName.Contains(commandPrefix)))
        //        return RuleSwitcher4InData.InDataHandler;

        //    return RuleSwitcher4InData.None;
        //}


        /// <summary>
        /// Определяет обработчик входных данных.
        /// Команда или Данные.
        /// </summary>
        /// <param name="inData">Обертка над входными данными</param>
        /// <param name="handlerName">Имя обработчика входных данных</param>
        /// <returns></returns>
        public RuleSwitcher4InData SwitchInDataHandler(InDataWrapper<AdInputType> inData, string handlerName)
        {
            var command = inData.Command;
            var directHandlerName = inData.DirectHandlerName ?? string.Empty;
            var commandPrefix = "Command_";
            var commandName = $"{commandPrefix}{command.ToString()}";  //Command_On, Command_Off, Command_Restart, Command_Clear

            if (handlerName.Equals(commandName))
                return RuleSwitcher4InData.CommandHanler;

            switch (command)
            {
                case Command4Device.None when (handlerName.Equals(directHandlerName)) || (directHandlerName.Equals("DefaultHandler") && (!handlerName.Contains(commandPrefix))):
                    return RuleSwitcher4InData.InDataDirectHandler;

                case Command4Device.None when (string.IsNullOrEmpty(directHandlerName)) && (!handlerName.Contains(commandPrefix)):
                    return RuleSwitcher4InData.InDataHandler;

                default:
                    return RuleSwitcher4InData.None;
            }
        }


  



        ///// <summary>
        ///// Фильтровать элементы по Contrains этого правила.
        ///// </summary>
        ///// <param name="inData"></param>
        ///// <param name="whereFilter">фильтрация данных</param>
        ///// <param name="orderBy">упорядочевание данных по имени св-ва</param>
        ///// <param name="takeItems">кол-во элементов которые нужно взять из коллекции или дополнить до этого кол-ва</param>
        ///// <param name="defaultItemJson">дефолтное значение AdInputType, дополняется до TakeIteme этим значением</param>
        ///// <returns></returns>
        //public IEnumerable<AdInputType> FilteredAndOrderedAndTakesItems(IEnumerable<AdInputType> inData, string whereFilter, string orderBy, int takeItems, string defaultItemJson)
        //{
        //    if(inData == null)
        //        return new List<AdInputType>();

        //    var now = DateTime.Now;
        //    try
        //    {
        //        //ЗАМЕНА  DateTime.Now.AddMinute(...)---------------------------
        //        var pattern = @"DateTime\.Now\.AddMinute\(([^()]*)\)";
        //        var where = whereFilter;
        //        where = Regex.Replace(where, pattern, x =>
        //        {
        //            var val = x.Groups[1].Value;
        //            if (int.TryParse(val, out var min))
        //            {
        //                var date = now.AddMinutes(min);
        //                return $"DateTime({date.Year}, {date.Month}, {date.Day}, {date.Hour}, {date.Minute}, 0)";
        //            }
        //            return x.Value;
        //        });
        //        //ЗАМЕНА  DateTime.Now----------------------------------------
        //        pattern = @"DateTime.Now";
        //        where = Regex.Replace(where, pattern, x =>
        //        {
        //            var date = now;
        //            return $"DateTime({date.Year}, {date.Month}, {date.Day}, {date.Hour}, {date.Minute}, 0)";
        //        });
        //        //ПРИМЕНИТЬ ФИЛЬТР И УПОРЯДОЧЕВАНИЕ
        //        var filtred = inData.AsQueryable().Where(where).OrderBy(orderBy).ToList();
        //        //ВЗЯТЬ TakeItems ИЛИ ДОПОЛНИТЬ ДО TakeItems.

        //        var defaultItem= GetDefaultAdInputType(defaultItemJson);
        //        var takedItems= Enumerable.Repeat(defaultItem, takeItems).ToArray();
        //        var endPosition= (takeItems < filtred.Count) ? takeItems : filtred.Count;
        //        filtred.CopyTo(0, takedItems, 0, endPosition);
        //        return takedItems;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Warning($"FilteredAndOrderedAndTakesItems Exception: {ex}");            
        //        return null;
        //    }
        //}


        //private AdInputType GetDefaultAdInputType(string defaultItemJson)
        //{
        //    var adInputType = JsonConvert.DeserializeObject<AdInputType>(defaultItemJson);
        //    return adInputType;
        //}
    }

    /// <summary>
    /// Выбор Обработчика входных данных.
    /// </summary>
    public enum RuleSwitcher4InData
    {
        None,
        CommandHanler,
        InDataHandler,
        InDataDirectHandler
    }
}