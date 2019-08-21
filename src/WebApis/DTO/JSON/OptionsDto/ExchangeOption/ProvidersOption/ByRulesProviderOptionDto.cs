using System.Collections.Generic;

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
        public string WhereFilter { get; set; }            
        public string OrderBy { get; set; }                 
        public int TakeItems { get; set; }                  
        public string DefaultItemJson { get; set; }         
        public List<ViewRuleOptionDto> ViewRules { get; set; }  
    }


    public class ViewRuleOptionDto
    {
        public int Id { get; set; }
        public int StartPosition { get; set; }               
        public int Count { get; set; }                      
        public int BatchSize { get; set; }              
        public RequestOptionDto RequestOption { get; set; }     
        public ResponseOptionDto ResponseOption { get; set; }   
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
        public string Format { get; set; }
        public int Lenght { get; set; }                     
        public int TimeRespone { get; set; }
        public string Body { get; set; }
        public string StronglyTypedName { get; set; }
    }

}