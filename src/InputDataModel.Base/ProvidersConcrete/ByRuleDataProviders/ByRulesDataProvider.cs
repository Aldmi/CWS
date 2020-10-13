using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using Domain.InputDataModel.Base.ProvidersOption;
using Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers;
using Domain.InputDataModel.Shared.StringInseartService.InlineInseart;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Serilog;
using Shared.Extensions;


namespace Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders
{
    /// <summary>
    /// Immutable.
    /// Задает провайдер конфигурируемый правилами.
    /// </summary>
    /// <typeparam name="TIn">тип входных данных для провайдера.</typeparam>
    public class ByRulesDataProvider<TIn> : BaseDataProvider<TIn>, IDataProvider<TIn> where TIn : InputTypeBase
    {
        #region field
        private readonly List<Rule<TIn>> _rules;        // Набор правил, для обработки данных.
        private readonly ILogger _logger;
        private readonly ByRulesProviderOption _option;
        #endregion

        

        #region ctor
        public ByRulesDataProvider(
            Func<ProviderTransfer<TIn>, ProviderStatus.Builder, ProviderResult<TIn>> providerResultFactory,
            ProviderOption providerOption,
            IIndependentInseartsHandlersFactory inputTypeInseartsHandlersFactory,
            StringInsertModelExtStorage stringInsertModelExtStorage,
            InlineInseartService inlineInseartService,
            ILogger logger) : base(providerOption.Name, providerResultFactory, logger)
        {
            _option = providerOption.ByRulesProviderOption;
            if (_option == null)
                throw new ArgumentNullException(providerOption.Name); //TODO: выбросы исключений пометсить в Shared ThrowIfNull(object obj)

            _rules = _option.Rules.Select(opt => new Rule<TIn>(opt, inputTypeInseartsHandlersFactory, stringInsertModelExtStorage, inlineInseartService, logger)).ToList();
            RuleName4DefaultHandle = string.IsNullOrEmpty(_option.RuleName4DefaultHandle)
                ? "DefaultHandler"
                : _option.RuleName4DefaultHandle;
            _logger = logger;

            //var providerCore = ProviderResultFactory(null, StatusDict);//DEBUG
        }
        #endregion



        #region prop
        private IEnumerable<Rule<TIn>> GetRules => _rules.ToList();                     //Копия списка Rules, чтобы  избежать Exception при перечислении (т.к. Rules - мутабельна).
        public string RuleName4DefaultHandle { get; }
        #endregion



        #region RtOptionsMethods
        /// <summary>
        /// Вернуть опции провайдера
        /// </summary>
        public ProviderOption GetCurrentOption()
        {
            var provideroption = new ProviderOption
            {
                Name = ProviderName,
                ByRulesProviderOption = _option
            };
            return provideroption;
        }
        #endregion



        #region Methode

        /// <summary>
        /// Запуск конвеера обработки запрос-ответ для обмена
        /// </summary>
        public async Task StartExchangePipelineAsync(InDataWrapper<TIn> inData, CancellationToken ct)
        {
            //ЕСЛИ ДАННЫХ ДЛЯ ОТПРАВКИ НЕТ (например для Цикл. обмена при старте)
            if (inData == null)
            {
                inData = new InDataWrapper<TIn>
                {
                    Datas = new List<TIn>(),
                    Command = Command4Device.None,
                    //DirectHandlerName = RuleName4DefaultHandle
                };
            }
            foreach (var rule in GetRules)
            { 
               ct.ThrowIfCancellationRequested();
               var ruleOption = rule.GetCurrentOption();
                switch (SwitchInDataHandler(inData, ruleOption.Name))
                {
                    //КОМАНДА-------------------------------------------------------------
                    case RuleSwitcher4InData.CommandHanler:
                        SendCommand(rule, inData.Command);
                        continue;

                    //ДАННЫЕ ДЛЯ УКАЗАНОГО RULE--------------------------------------------
                    case RuleSwitcher4InData.InDataDirectHandler:
                        var takesItems = inData.Datas.TakeItemIfEmpty(ruleOption.AgregateFilter, ruleOption.DefaultItemJson, _logger);
                        var resultItems = takesItems.ToList();
                        await SendDataAsync(rule, resultItems, ct);
                        continue;

                    //ДАННЫЕ--------------------------------------------------------------  
                    case RuleSwitcher4InData.InDataHandler:
                        takesItems = inData.Datas.TakeItemIfEmpty(ruleOption.AgregateFilter, ruleOption.DefaultItemJson, _logger);           //Позволяет применять Filter для Default данных (inData.Datas пустой)
                        var filtredItems = takesItems.Filter(ruleOption.AgregateFilter, ruleOption.DefaultItemJson, _logger);
                        if (filtredItems == null || !filtredItems.Any())
                            continue;

                        resultItems = filtredItems.ToList();
                        await SendDataAsync(rule, resultItems, ct);
                        continue;

                    default:
                        continue;
                }
            }
            //Конвеер обработки входных данных завершен    
            await Task.CompletedTask;
        }


        /// <summary>
        /// Отобразить данные через коллекцию ViewRules у правила.
        /// </summary>
        private async Task SendDataAsync(Rule<TIn> rule, List<TIn> takesItems, CancellationToken ct)
        {
            if (takesItems != null && takesItems.Any())
            {
                var ruleName = $"{rule.GetCurrentOption().Name}";
                foreach (var viewRule in rule.GetViewRules)
                {
                    await foreach (var (_, isFailure, transfer, error) in viewRule.CreateProviderTransfer4Data(takesItems, ct).WithCancellation(ct))
                    {
                        ct.ThrowIfCancellationRequested();
                        if (isFailure)
                        {
                            _logger.Warning(error);
                            await Task.Delay(1000, ct); //Задержка на отображение ошибки
                            continue;
                        }
                        var sendingUnitName = $"RuleName= '{ruleName}' viewRule.Id= '{viewRule.GetCurrentOption.Id}'";
                        var providerStatusBuilder = transfer.CreateProviderStatusBuilder(sendingUnitName);
                        var providerResult = ProviderResultFactory(transfer, providerStatusBuilder);
                        RaiseProviderResultRx.OnNext(providerResult);
                    }
                }
            }
            await Task.CompletedTask;
        }


        /// <summary>
        /// Отправить команду через первое ViewRule.
        /// </summary>
        private void SendCommand(Rule<TIn> rule, Command4Device command)
        {
            var commandViewRule = rule.GetViewRules.FirstOrDefault();
            var transfer = commandViewRule?.CreateProviderTransfer4Command(command);
            if(transfer == null)
                return;

            var sendingUnitName = $"RuleName= '{rule.GetCurrentOption().Name}' viewRule.Id= '{commandViewRule.GetCurrentOption.Id}'";
            var providerStatusBuilder = transfer.CreateProviderStatusBuilder(sendingUnitName);
            providerStatusBuilder.SetCommand(command);
            var providerResult = ProviderResultFactory(transfer, providerStatusBuilder);

            RaiseProviderResultRx.OnNext(providerResult);
        }
        #endregion



        #region Disposable
        public override void Dispose()
        {
           base.Dispose();
           RaiseProviderResultRx?.Dispose();
        }
        #endregion
    }
}