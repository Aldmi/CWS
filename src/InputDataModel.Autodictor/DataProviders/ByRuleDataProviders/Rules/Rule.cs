using System.Collections.Generic;
using System.Linq;
using DAL.Abstract.Entities.Options.Exchange.ProvidersOption;
using Domain.InputDataModel.Base;
using Serilog;

namespace Domain.InputDataModel.Autodictor.DataProviders.ByRuleDataProviders.Rules
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

        public Rule(RuleOption option, IIndependentInserts independentInserts, ILogger logger)
        {
            Option = option;
            _logger = logger;
            _viewRules= option.ViewRules.Select(opt=> new ViewRule<TIn>(Option.AddressDevice, opt, independentInserts, _logger)).ToList();
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