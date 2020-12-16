using System.Collections.Generic;
using Domain.InputDataModel.Base.ProvidersOption;
using Shared.Types;

namespace WebApiSwc.DTO.JSON.OptionsDto.ExchangeOption.ProvidersOption
{
    public class ByRulesProviderOptionDto
    {
        public string RuleName4DefaultHandle { get; set; }
        public List<RuleOptionDto> Rules { get; set; }
    }

    public class RuleOptionDto
    {
        public string Name { get; set; }                       
        public string AddressDevice { get; set; }
        public AgregateFilter AgregateFilter { get; set; }
        public string DefaultItemJson { get; set; }         
        public List<ViewRuleOptionDto> ViewRules { get; set; }  
    }

    public class ViewRuleOptionDto
    {
        public int Id { get; set; }
        public int StartPosition { get; set; }               
        public int Count { get; set; }                      
        public int BatchSize { get; set; }
        public List<UnitOfSendingOptionDto> UnitOfSendings { get; set; } 
    }

    public class UnitOfSendingOptionDto
    {
        public string Name { get; set; }
        public RequestOptionDto RequestOption { get; set; }     //Запрос
        public ResponseOptionDto ResponseOption { get; set; }   //Ответ
    }

    public class RequestOptionDto
    {
        public string Format { get; set; }
        public int MaxBodyLenght { get; set; }              
        public string Header { get; set; }                  
        public string Body { get; set; }                    
        public string Footer { get; set; }                  
    }

    public class ResponseOptionDto
    {
        public int TimeRespone { get; set; }                                         
        public string ValidatorName { get; set; }                         
        public LenghtResponseValidatorOption LenghtValidator { get; set; }            
        public EqualResponseValidatorOption EqualValidator { get; set; }
        public ManualEkrimValidatorOption ManualEkrimValidator { get; set; }
    }
}