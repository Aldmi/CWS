using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Factory;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Handlers;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using Domain.InputDataModel.Base.ProvidersOption;
using Domain.InputDataModel.Base.Response;
using Serilog;
using Shared.Extensions;
using Shared.Services.StringInseartService;
using Shared.Services.StringInseartService.IndependentInseart;


namespace Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders
{
    /// <summary>
    /// Immutable.
    /// Задает провайдер конфигурируемый правилами.
    /// </summary>
    /// <typeparam name="TIn">тип входных данных для провайдера.</typeparam>
    public class ByRulesDataProvider<TIn> : BaseDataProvider<TIn>, IDataProvider<TIn, ResponseInfo> where TIn : InputTypeBase
    {
        #region field
        private readonly List<Rule<TIn>> _rules;        // Набор правил, для обработки данных.
        private readonly ILogger _logger;
        private readonly ByRulesProviderOption _option;
        #endregion

        

        #region ctor
        public ByRulesDataProvider(Func<ProviderTransfer<TIn>, IDictionary<string, string>,
            ProviderResult<TIn>> providerResultFactory,
            ProviderOption providerOption,
            IIndependentInseartsHandlersFactory inputTypeInseartsHandlersFactory,
            ILogger logger) : base(providerResultFactory, logger)
        {
            _option = providerOption.ByRulesProviderOption;
            if (_option == null)
                throw new ArgumentNullException(providerOption.Name); //TODO: выбросы исключений пометсить в Shared ThrowIfNull(object obj)

            ProviderName = providerOption.Name;
            _rules = _option.Rules.Select(opt => new Rule<TIn>(opt, inputTypeInseartsHandlersFactory, logger)).ToList();
            RuleName4DefaultHandle = string.IsNullOrEmpty(_option.RuleName4DefaultHandle)
                ? "DefaultHandler"
                : _option.RuleName4DefaultHandle;
            _logger = logger;

            //var providerCore = ProviderResultFactory(null, StatusDict);//DEBUG
        }
        #endregion



        #region prop
        public string ProviderName { get; }
        private IEnumerable<Rule<TIn>> GetRules => _rules.ToList();                     //Копия списка Rules, чтобы  избежать Exception при перечислении (т.к. Rules - мутабельна).
        public string RuleName4DefaultHandle { get; }
        public Dictionary<string, string> StatusDict { get; } = new Dictionary<string, string>();
        #endregion



        #region RxEvent

        public Subject<ProviderResult<TIn>> RaiseSendDataRx { get; } = new Subject<ProviderResult<TIn>>();

        #endregion



        #region RtOptionsMethods

        /// <summary>
        /// Вернуть опции провайдера
        /// </summary>
        public ProviderOption GetCurrentOption()
        {
            var provideroption = new ProviderOption
            {
                //Name = ProviderName,
                //ByRulesProviderOption = new ByRulesProviderOption
                //{
                //    RuleName4DefaultHandle = RuleName4DefaultHandle,
                //    Rules = GetRules.Select(r => r.GetCurrentOption()).ToList()
                //}
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
                    DirectHandlerName = RuleName4DefaultHandle
                };
            }
            foreach (var rule in GetRules)
            { 
               ct.ThrowIfCancellationRequested();
               StatusDict.Clear();
                var ruleOption = rule.GetCurrentOption();
                switch (SwitchInDataHandler(inData, ruleOption.Name))
                {
                    //КОМАНДА-------------------------------------------------------------
                    case RuleSwitcher4InData.CommandHanler:
                        SendCommand(rule, inData.Command);
                        continue;

                    //ДАННЫЕ ДЛЯ УКАЗАНОГО RULE--------------------------------------------
                    case RuleSwitcher4InData.InDataDirectHandler:
                        var takesItems = inData.Datas
                            ?.Order(ruleOption.OrderBy, _logger)
                            ?.TakeItems(ruleOption.TakeItems, ruleOption.DefaultItemJson, _logger)
                            ?.ToList();
                        await SendDataAsync(rule, takesItems, ct);
                        continue;

                    //ДАННЫЕ--------------------------------------------------------------  
                    case RuleSwitcher4InData.InDataHandler:
                        var filtredItems = inData.Datas?.Filter(ruleOption.WhereFilter, _logger);
                        if (filtredItems == null || !filtredItems.Any())
                            continue;

                        takesItems = filtredItems.Order(ruleOption.OrderBy, _logger)
                            .TakeItems(ruleOption.TakeItems, ruleOption.DefaultItemJson, _logger)
                            .ToList();
                        await SendDataAsync(rule, takesItems, ct);
                        continue;

                    default:
                        continue;
                }
            }
            //Конвеер обработки входных данных завершен    
            StatusDict.Clear();
            await Task.CompletedTask;
        }


        /// <summary>
        /// Отобразить данные через коллекцию ViewRules у правила.
        /// </summary>
        private async Task SendDataAsync(Rule<TIn> rule, List<TIn> takesItems, CancellationToken ct)
        {
            if (takesItems != null && takesItems.Any())
            {
                StatusDict["RuleName"] = $"{rule.GetCurrentOption().Name}";
                foreach (var viewRule in rule.GetViewRules)
                {
                    foreach (var providerTransfer in viewRule.CreateProviderTransfer4Data(takesItems))
                    {
                        ct.ThrowIfCancellationRequested();
                        if (providerTransfer == null) //правило отображения не подходит под ДАННЫЕ
                            continue;

                        StatusDict["viewRule.Id"] = $"{viewRule.GetCurrentOption.Id}";
                        var providerResult = ProviderResultFactory(providerTransfer, StatusDict);
                        RaiseSendDataRx.OnNext(providerResult);
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
            var providerTransfer = commandViewRule?.CreateProviderTransfer4Command(command);
            if(providerTransfer == null)
                return;

            StatusDict["Command"] = $"{command}";
            StatusDict["RuleName"] = $"{rule.GetCurrentOption().Name}";
            StatusDict["viewRule.Id"] = $"{commandViewRule.GetCurrentOption.Id}";
            var providerResult = ProviderResultFactory(providerTransfer, StatusDict);
            RaiseSendDataRx.OnNext(providerResult);
        }
       #endregion




       public override void Dispose()
       {
           base.Dispose();
           RaiseSendDataRx?.Dispose();
       }
    }
}