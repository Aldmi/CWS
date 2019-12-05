using System;
using System.Collections.Generic;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.Response;
using Serilog;

namespace Domain.InputDataModel.Base.ProvidersAbstract
{
    //TODO: после созданния нескольких провайдеров, вынести обший функционал в этот класс
    public abstract class BaseDataProvider<TIn> : IDisposable where TIn : InputTypeBase
    {
        #region field
        protected readonly Func<ProviderTransfer<TIn>, IDictionary<string, string>, ProviderResult<TIn>> ProviderResultFactory;
        private readonly ILogger _logger;
        #endregion



        #region ctor
        protected BaseDataProvider(Func<ProviderTransfer<TIn>, IDictionary<string, string>, ProviderResult<TIn>> providerResultFactory, ILogger logger)
        {
            ProviderResultFactory = providerResultFactory;
            _logger = logger;
        }
        #endregion



        #region Methode

        /// <summary>
        /// Определяет обработчик входных данных.
        /// Команда или Данные.
        /// </summary>
        /// <param name="inData">Обертка над входными данными</param>
        /// <param name="handlerName">Имя обработчика входных данных</param>
        /// <returns></returns>
        public RuleSwitcher4InData SwitchInDataHandler(InDataWrapper<TIn> inData, string handlerName)
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

        #endregion



        #region Disposable
        public virtual void Dispose()
        {
        }
        #endregion
    }
}