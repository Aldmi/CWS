using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using DAL.Abstract.Entities.Options.Exchange.ProvidersOption;
using Exchange.Base.DataProviderAbstract;
using Exchange.Base.Model;
using InputDataModel.Autodictor.DataProviders.ByRuleDataProviders.Rules;
using InputDataModel.Autodictor.Extensions;
using InputDataModel.Autodictor.Model;
using InputDataModel.Base;
using Serilog;
using Shared.Extensions;
using Shared.Helpers;

namespace InputDataModel.Autodictor.DataProviders.ByRuleDataProviders
{
    public class ByRulesDataProvider : BaseDataProvider, IExchangeDataProvider<AdInputType, ResponseDataItem<AdInputType>>
    {
        #region field

        private readonly List<Rule> _rules;                   // Набор правил, для обработки данных.
        private ViewRuleRequestModelWrapper _currentRequest;  // Созданный запрос, после подготовки данных. 
        private readonly ILogger _logger;

        #endregion




        #region ctor

        public ByRulesDataProvider(ProviderOption providerOption, ILogger logger) : base(logger)
        {
            var option = providerOption.ByRulesProviderOption;
            if (option == null)
                throw new ArgumentNullException(providerOption.Name);
      
            ProviderName = providerOption.Name;
            _rules = option.Rules.Select(opt => new Rule(opt, logger)).ToList();
            RuleName4DefaultHandle = string.IsNullOrEmpty(option.RuleName4DefaultHandle)
                ? "DefaultHandler"//_rules.First().Option.Name
                : option.RuleName4DefaultHandle;
            _logger = logger;
        }

        #endregion


        //DEBUG----------------

        public ProviderOption GetCurrentOption()
        {
            var provideroption = new ProviderOption
            {
                Name = ProviderName,
                ByRulesProviderOption = new ByRulesProviderOption
                {
                    RuleName4DefaultHandle = RuleName4DefaultHandle,
                    Rules = _rules.Select(r => r.GetCurrentOption()).ToList()
                }
            };
            return provideroption;
        }


        #region MutableOptionsMethods

        /// <summary>
        /// Меняются все опции провайдера на optionNew.
        /// </summary>
        public bool SetCurrentOption(ProviderOption optionNew)
        {
            var option = optionNew.ByRulesProviderOption;
            if (option == null)
                throw new ArgumentNullException(optionNew.Name);

            //TODO: ВЫСТАВИТЬ ВРУЧНУЮ ВСЕ ОПЦИИ НА 


            return true;
        }


        public void ChangeWhereFilter(string key, string filter)
        {
            var rule = GetRuleByKey(key);
            rule.Option.WhereFilter = filter;
        }


        public void ChangeOrderBy(string key, string ordreby)
        {
            var rule = GetRuleByKey(key);
            rule.Option.OrderBy = ordreby;
        }





        private Rule GetRuleByKey(string key)
        {
            var rule = _rules.FirstOrDefault(r => r.Option.Name == key);
            HelpersException.ThrowIfNotFind(rule, $"объект не найден по Key {key}");
            return rule;
        }

        #endregion






        #region prop

        public string RuleName4DefaultHandle { get; }
        public string ProviderName { get; }
        public Dictionary<string, string> StatusDict { get; } = new Dictionary<string, string>();
        public InDataWrapper<AdInputType> InputData { get; set; }
        public ResponseDataItem<AdInputType> OutputData { get; set; }
        public bool IsOutDataValid { get; set; }

        public int TimeRespone => _currentRequest.ResponseOption.TimeRespone;        //Время на ответ
        public int CountSetDataByte => _currentRequest.ResponseOption.Lenght;

        #endregion




        #region RxEvent

        public Subject<IExchangeDataProvider<AdInputType, ResponseDataItem<AdInputType>>> RaiseSendDataRx { get; } = new Subject<IExchangeDataProvider<AdInputType, ResponseDataItem<AdInputType>>>();

        #endregion




        #region IExchangeDataProviderImplementation

        public byte[] GetDataByte()
        {
            var stringRequset = _currentRequest.StringRequest;
            var format = _currentRequest.RequestOption.GetCurrentFormat();
            
            StatusDict["GetDataByte.StringRequest"] = $"[{stringRequset}] Lenght= {stringRequset.Length}  Format={format}";
            //Преобразовываем КОНЕЧНУЮ строку в массив байт
            var resultBuffer = stringRequset.ConvertString2ByteArray(format);
            StatusDict["GetDataByte.ByteRequest"] = $"{ resultBuffer.ArrayByteToString("X2")} Lenght= {resultBuffer.Length}";
            StatusDict["TimeResponse"] = $"{ _currentRequest.ResponseOption.TimeRespone}";
            return resultBuffer;
        }

        /// <summary>
        /// Проверить ответ, Присвоить выходные данные.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool SetDataByte(byte[] data)
        {
            var format = _currentRequest.ResponseOption.Format;
            if (data == null)
            {
                IsOutDataValid = false;
                OutputData = new ResponseDataItem<AdInputType>
                {
                    ResponseData = null,
                    Encoding = format,
                    IsOutDataValid = IsOutDataValid
                };
                return false;
            }
            var stringResponse = data.ArrayByteToString(format);
            IsOutDataValid = (stringResponse == _currentRequest.ResponseOption.Body);
            OutputData = new ResponseDataItem<AdInputType>
            {
                ResponseData = stringResponse,
                Encoding = format,
                IsOutDataValid = IsOutDataValid
            };
            StatusDict["SetDataByte.StringResponse"] = $"{stringResponse} Length= {data.Length}";
         
           
            return IsOutDataValid;
        }

        #endregion



        #region Methode

        public async Task StartExchangePipeline(InDataWrapper<AdInputType> inData)
        {
            //ЕСЛИ ДАННЫХ ДЛЯ ОТПРАВКИ НЕТ (например для Цикл. обмена при старте)
            if (inData == null)
            {
                inData = new InDataWrapper<AdInputType>
                {
                    Datas = new List<AdInputType>(),
                    Command = Command4Device.None,
                    DirectHandlerName = RuleName4DefaultHandle
                };
            }
        

            foreach (var rule in _rules)
            {
                StatusDict.Clear();
                switch (SwitchInDataHandler(inData, rule.Option.Name))
                {
                    //КОМАНДА-------------------------------------------------------------
                    case RuleSwitcher4InData.CommandHanler:
                        StatusDict["Command"] = $"{inData.Command}";
                        //_logger.Information($"Отправка КОМАНДЫ: {inData.Command}");
                        var commandViewRule = rule.ViewRules.FirstOrDefault();
                        _currentRequest = commandViewRule?.GetCommandRequestString();
                        InputData = new InDataWrapper<AdInputType> { Command = inData.Command };
                        RaiseSendDataRx.OnNext(this);
                        continue;

                    //ДАННЫЕ ДЛЯ УКАЗАНОГО RULE--------------------------------------------------------------  
                    case RuleSwitcher4InData.InDataDirectHandler:
                        var takesItems = inData.Datas?.Order(rule.Option.OrderBy, _logger)
                                                     ?.TakeItems(rule.Option.TakeItems, rule.Option.DefaultItemJson, _logger)
                                                     ?.ToList();
                        ViewRuleSendData(rule, takesItems);
                        continue;

                    //ДАННЫЕ--------------------------------------------------------------  
                    case RuleSwitcher4InData.InDataHandler:
                        var filtredItems = inData.Datas?.Filter(rule.Option.WhereFilter, _logger);
                        if (filtredItems == null || !filtredItems.Any())
                            continue;

                        takesItems = filtredItems.Order(rule.Option.OrderBy, _logger)
                                                 .TakeItems(rule.Option.TakeItems, rule.Option.DefaultItemJson, _logger)
                                                 .ToList();
                        ViewRuleSendData(rule, takesItems);
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
        private void ViewRuleSendData(Rule rule, List<AdInputType> takesItems)
        {
            if (takesItems != null && takesItems.Any())
            {
                StatusDict["RuleName"] = $"{rule.Option.Name}";
                //_logger.Information($"Отправка ДАННЫХ через {rule.Option.Name}. Кол-во данных:{takesItems.Count}");
                foreach (var viewRule in rule.ViewRules)
                {
                    foreach (var request in viewRule.GetDataRequestString(takesItems))
                    {
                        if (request == null) //правило отображения не подходит под ДАННЫЕ
                            continue;

                        _currentRequest = request;
                        InputData = new InDataWrapper<AdInputType> { Datas = _currentRequest.BatchedData.ToList() };
                        StatusDict["viewRule.Id"] = $"{viewRule.Option.Id}";
                        RaiseSendDataRx.OnNext(this);
                    }
                }
            }
        }

        #endregion



        #region NotImplemented
        public Stream GetStream()
        {
            throw new NotImplementedException();
        }

        public bool SetStream(Stream stream)
        {
            throw new NotImplementedException();
        }

        public string GetString()
        {
            return _currentRequest.StringRequest;
        }

        public bool SetString(Stream stream)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}