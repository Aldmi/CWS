using System.Collections.Generic;
using Shared.Types;

namespace Infrastructure.Dal.EfCore.Entities.Exchange.ProvidersOption
{
    public class EfByRulesProviderOption
    {
        public string RuleName4DefaultHandle { get; set; }
        public List<EfRuleOption> Rules { get; set; }
    }


    public class EfRuleOption
    {
        public string Name { get; set; }
        public string AddressDevice { get; set; }
        public AgregateFilter AgregateFilter { get; set; }
        public string DefaultItemJson { get; set; } 
        public List<EfViewRuleOption> ViewRules { get; set; }
    }


    public class EfViewRuleOption
    {
        public int Id { get; set; }
        public int StartPosition { get; set; }               
        public int Count { get; set; }     
        public int BatchSize { get; set; }
        public ViewRuleMode Mode { get; set; }
        public List<EfUnitOfSendingOption> UnitOfSendings { get; set; }
    }


    public class EfUnitOfSendingOption
    {
        public string Name { get; set; }
        public EfRequestOption RequestOption { get; set; }     //Запрос
        public EfResponseOption ResponseOption { get; set; }   //Ответ
    }


    public class EfRequestOption
    {
        public string Format { get; set; } //TODO: убрать
        public int MaxBodyLenght { get; set; }
        public string Header { get; set; }                   // НАЧАЛО запроса (ТОЛЬКО ЗАВИСИМЫЕ ДАННЫЕ).
        public string Body { get; set; }                     // ТЕЛО запроса (ТОЛЬКО НЕЗАВИСИМЫЕ ДАННЫЕ). Каждый элемент батча подставляет свои данные в Body, затем все элементы Конкатенируются.
        public string Footer { get; set; }                   // КОНЕЦ ЗАПРОСА (ТОЛЬКО ЗАВИСИМЫЕ ДАННЫЕ).
    }


    public class EfResponseOption
    {
        public int TimeRespone { get; set; }                                         
        public string ValidatorName { get; set; }                              
        public EfLenghtResponseValidatorOption LenghtValidator { get; set; }         
        public EfEqualResponseValidatorOption EqualValidator { get; set; }
        public EfManualEkrimValidatorOption ManualEkrimValidator { get; set; }
    }

    public class EfLenghtResponseValidatorOption
    {
        public int ExpectedLenght { get; set; }
    }

    public class EfEqualResponseValidatorOption
    {
        public string Format { get; set; }
        public string Body { get; set; }
    }

    public class EfManualEkrimValidatorOption { }


    /// <summary>
    /// Режим работы ViewRule
    /// </summary>
    public enum ViewRuleMode  //TODO: в какой слой вынести?
    {
        Deprecated,
        Init,
        LongWork
    }
}