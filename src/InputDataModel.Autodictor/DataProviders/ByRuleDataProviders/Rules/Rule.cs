﻿using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Abstract.Entities.Options.Exchange.ProvidersOption;
using Serilog;

namespace InputDataModel.Autodictor.DataProviders.ByRuleDataProviders.Rules
{
    public class Rule
    {
        #region fields

        protected RuleOption Option;
        private readonly ILogger _logger;
        private readonly List<ViewRule> _viewRules;

        #endregion


        #region prop

        public IEnumerable<ViewRule> GetViewRules => _viewRules.ToList();

        #endregion



        #region ctor

        public Rule(RuleOption option, ILogger logger)
        {
            Option = option;
            _logger = logger;
            _viewRules= option.ViewRules.Select(opt=> new ViewRule(Option.AddressDevice, opt, _logger)).ToList();
        }

        #endregion




        #region Methods

        public RuleOption GetCurrentOption()
        {
            var ruleOption = Option;
            var currentViewRuleOptions= _viewRules.Select(vr => vr.GetCurrentOption());
            ruleOption.ViewRules = new List<ViewRuleOption>(currentViewRuleOptions);
            return ruleOption;
        }

        #endregion

    }
}