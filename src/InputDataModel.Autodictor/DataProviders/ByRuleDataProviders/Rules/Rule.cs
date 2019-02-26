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

        #endregion


        #region prop

        public IEnumerable<ViewRule> ViewRules { get; set; }

        #endregion



        #region ctor

        public Rule(RuleOption option, ILogger logger)
        {
            Option = option;
            _logger = logger;
            ViewRules= option.ViewRules.Select(opt=> new ViewRule(Option.AddressDevice, opt, _logger)).ToList();
        }

        #endregion




        #region MutabeleOptions

        public RuleOption GetCurrentOption()
        {
            var ruleOption = Option;
            var currentViewRuleOptions= ViewRules.Select(vr => vr.GetCurrentOption());
            ruleOption.ViewRules = new List<ViewRuleOption>(currentViewRuleOptions);
            return ruleOption;
        }

         
        public void SetCurrentOption(RuleOption ruleOption)
        {
            Option = ruleOption;
            foreach (var viewRuleOption in Option.ViewRules)
            {
                var viewRule = ViewRules.FirstOrDefault(vr => vr.GetCurrentOption().Id == viewRuleOption.Id);
                if (viewRule == null)
                {
                    //TODO: Копить в коллекцию ошибок
                    continue;
                }

                viewRule.SetCurrentOption(viewRuleOption);      
            }
        }

        #endregion

    }
}