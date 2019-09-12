﻿using System.Collections.Generic;
using System.Linq;
using DAL.Abstract.Entities.Options.Exchange.ProvidersOption;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Serilog;

namespace Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules
{
    public class Rule<TIn>
    {
        #region fields

        protected RuleOption Option;
        private readonly ILogger _logger;
        private readonly List<ViewRule<TIn>> _viewRules;

        #endregion


        #region prop

        public IEnumerable<ViewRule<TIn>> GetViewRules => _viewRules.ToList();

        #endregion



        #region ctor

        public Rule(RuleOption option, IIndependentInsertsService independentInsertsService, ILogger logger)
        {
            Option = option;
            _logger = logger;
            _viewRules= option.ViewRules.Select(opt=> new ViewRule<TIn>(Option.AddressDevice, opt, independentInsertsService, _logger)).ToList();
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

        #endregion
    }
}