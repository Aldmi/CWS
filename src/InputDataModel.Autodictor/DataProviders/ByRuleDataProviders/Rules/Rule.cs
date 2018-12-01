using System.Collections.Generic;
using System.Linq;
using DAL.Abstract.Entities.Options.Exchange.ProvidersOption;
using Serilog;

namespace InputDataModel.Autodictor.DataProviders.ByRuleDataProviders.Rules
{
    public class Rule
    {
        #region fields

        public readonly RuleOption Option;
        private readonly ILogger _logger;

        #endregion




        #region ctor

        public Rule(RuleOption option, ILogger logger)
        {
            Option = option;
            _logger = logger;
            ViewRules= option.ViewRules.Select(opt=> new ViewRule(Option.AddressDevice, opt, _logger)).ToList();
        }

        #endregion




        #region prop

        public IEnumerable<ViewRule> ViewRules { get; set; }

        #endregion
    }
}