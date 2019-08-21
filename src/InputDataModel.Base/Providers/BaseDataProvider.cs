using InputDataModel.Base.InData;
using InputDataModel.Base.Response;
using Serilog;
using Shared.Types;

namespace InputDataModel.Base.Providers
{
    //TODO: после созданния нескольких провайдеров, вынести обший функционал в этот класс
    public class BaseDataProvider<TInput>
    {
        protected readonly IStronglyTypedResponseFactory StronglyTypedResponseFactory;  //TODO: вынести обработку из SetDataByte потомков в базовый класс
        private readonly ILogger _logger;



        #region ctor

        public BaseDataProvider(IStronglyTypedResponseFactory stronglyTypedResponseFactory, ILogger logger)
        {
            StronglyTypedResponseFactory = stronglyTypedResponseFactory;
            _logger = logger;
        }

        #endregion


        /// <summary>
        /// Определяет обработчик входных данных.
        /// Команда или Данные.
        /// </summary>
        /// <param name="inData">Обертка над входными данными</param>
        /// <param name="handlerName">Имя обработчика входных данных</param>
        /// <returns></returns>
        public RuleSwitcher4InData SwitchInDataHandler(InDataWrapper<TInput> inData, string handlerName)
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