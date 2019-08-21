using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using Exchange.Base.Model;
using InputDataModel.Autodictor.Model;
using InputDataModel.Base;
using Newtonsoft.Json;
using Serilog;
using Shared.Types;

namespace InputDataModel.Autodictor.DataProviders
{
    public class BaseDataProvider
    {
        protected readonly IStronglyTypedResponseFactory StronglyTypedResponseFactory;
        private readonly ILogger _logger;



        #region ctor

        public BaseDataProvider(IStronglyTypedResponseFactory stronglyTypedResponseFactory, ILogger logger)
        {
            StronglyTypedResponseFactory = stronglyTypedResponseFactory;
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