using System;
using System.Collections.Generic;
using System.Linq;
using Domain.InputDataModel.Base.ProvidersOption;
using Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers;
using Domain.InputDataModel.Shared.StringInseartService.InlineInseart;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Infrastructure.Dal.EfCore.Entities.Exchange.ProvidersOption;
using Serilog;

namespace Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules
{
    public class Rule<TIn> : IEquatable<Rule<TIn>>
    {
        #region fields
        protected RuleOption Option;
        private readonly List<ViewRule<TIn>> _viewRules;
        private readonly ILogger _logger;
        #endregion


        #region prop
        /// <summary>
        /// Выдать разрешенные к работе ViewRules
        /// </summary>
        public IEnumerable<ViewRule<TIn>> GetViewRules => _viewRules.Where(vr =>vr.CurrentMode != ViewRuleMode.Deprecated).ToList();
        #endregion



        #region ctor
        public Rule(RuleOption option,
            IIndependentInseartsHandlersFactory inputTypeInseartsHandlersFactory,
            IReadOnlyDictionary<string, StringInsertModelExt> stringInsertModelExtDict,
            InlineInseartService inlineInseartService,
            ILogger logger)
        {
            Option = option;
            _logger = logger;
            _viewRules= option.ViewRules.Select(viewRuleOption=> ViewRule<TIn>.Create(
                viewRuleOption,
                Option.AddressDevice,
                inputTypeInseartsHandlersFactory,
                stringInsertModelExtDict,
                inlineInseartService,
                _logger)
            ).ToList();
        }
        #endregion



        #region Methods
        public RuleOption GetCurrentOption()
        {
            var ruleOption = Option;
            var currentViewRuleOptions= _viewRules.Select(vr => vr.GetCurrentOption);
            ruleOption.ViewRules = new List<ViewRuleOption>(currentViewRuleOptions);
            return ruleOption;
        }

        /// <summary>
        /// Сбросить состояние ВСЕХ ViewRules на Дефолтное
        /// </summary>
        public void ResetViewRulesMode2Default()
        {
            _viewRules.ForEach(vr=> vr.ResetMode2Default());
        }
        #endregion


        #region IEquatable

        public static bool operator ==(Rule<TIn> obj1, Rule<TIn> obj2)
        {
            if (ReferenceEquals(obj1, obj2)) return true;
            if (ReferenceEquals(obj1, null)) return false;
            if (ReferenceEquals(obj2, null)) return false;
            
            return obj1.Equals(obj2);
        }

        public static bool operator !=(Rule<TIn> obj1, Rule<TIn> obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals(Rule<TIn> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Option, other.Option);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Rule<TIn>) obj);
        }

        public override int GetHashCode()
        {
            return (Option != null ? Option.GetHashCode() : 0);
        }
        #endregion
    }
}