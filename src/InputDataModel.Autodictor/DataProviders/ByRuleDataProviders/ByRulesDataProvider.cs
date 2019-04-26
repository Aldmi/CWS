﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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



        #region prop

        private IEnumerable<Rule> GetRules => _rules.ToList();                     //Копия списка Rules, чтобы  избежать Exception при перечислении (т.к. Rules - мутабельна).
        public string RuleName4DefaultHandle { get; }
        public string ProviderName { get; }
        public Dictionary<string, string> StatusDict { get; } = new Dictionary<string, string>();
        public InDataWrapper<AdInputType> InputData { get; set; }
        public ResponseDataItem<AdInputType> OutputData { get; set; }
        public bool IsOutDataValid { get; set; }
        public int TimeRespone => _currentRequest.ResponseOption.TimeRespone;        //Время на ответ
        public int CountSetDataByte => _currentRequest.ResponseOption.Lenght;        //Кол-во принимаемых байт в ответе

        #endregion



        #region RxEvent

        public Subject<IExchangeDataProvider<AdInputType, ResponseDataItem<AdInputType>>> RaiseSendDataRx { get; } = new Subject<IExchangeDataProvider<AdInputType, ResponseDataItem<AdInputType>>>();

        #endregion



        #region IExchangeDataProviderImplementation

        /// <summary>
        /// Сформировать буффер ЗАПРОСА
        /// </summary>
        /// <returns></returns>
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
            var format = _currentRequest.ResponseOption.GetCurrentFormat();
            var stringResponseRef = _currentRequest.StringResponse;
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
            IsOutDataValid = (stringResponse == stringResponseRef); //TODO: как лутчше сравнивать строки
            OutputData = new ResponseDataItem<AdInputType>
            {
                ResponseData = stringResponse,
                Encoding = format,
                IsOutDataValid = IsOutDataValid
            };
            var diffResp = (!IsOutDataValid) ? $"ПринятоБайт/ОжидаемБайт= {data.Length}/{_currentRequest.ResponseOption.Lenght}" : string.Empty;
            StatusDict["SetDataByte.StringResponse"] = $"{stringResponseRef} ?? {stringResponse}   diffResp=  {diffResp}";
            return IsOutDataValid;
        }

        #endregion



        #region RtOptionsMethods

        /// <summary>
        /// Вернуть опции провайдера
        /// </summary>
        public ProviderOption GetCurrentOptionRt()
        {
            var provideroption = new ProviderOption
            {
                Name = ProviderName,
                ByRulesProviderOption = new ByRulesProviderOption
                {
                    RuleName4DefaultHandle = RuleName4DefaultHandle,
                    Rules = GetRules.Select(r => r.GetCurrentOption()).ToList()
                }
            };
            return provideroption;
        }


        /// <summary>
        /// Меняются все опции провайдера на optionNew.
        /// </summary>
        public bool SetCurrentOptionRt(ProviderOption optionNew)
        {
            var option = optionNew.ByRulesProviderOption;
            if (option == null)
                throw new ArgumentNullException(optionNew.Name);

            _rules.Clear();
            var newRules = option.Rules.Select(opt => new Rule(opt, _logger)).ToList();//TODO: валидация проводить в самом dto объекте Rule и ViewRule
            _rules.AddRange(newRules);

            return true;
        }

        #endregion




        #region Methode

        /// <summary>
        /// Запуск конвеера обработки запрос-ответ для обмена
        /// </summary>
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

            foreach (var rule in GetRules)
            {
                StatusDict.Clear();
                var ruleOption = rule.GetCurrentOption();
                switch (SwitchInDataHandler(inData, ruleOption.Name))
                {
                    //КОМАНДА-------------------------------------------------------------
                    case RuleSwitcher4InData.CommandHanler:
                        ViewRuleSendCommand(rule, inData.Command);
                        continue;

                    //ДАННЫЕ ДЛЯ УКАЗАНОГО RULE--------------------------------------------
                    case RuleSwitcher4InData.InDataDirectHandler:
                        var takesItems = inData.Datas?.Order(ruleOption.OrderBy, _logger)
                            ?.TakeItems(ruleOption.TakeItems, ruleOption.DefaultItemJson, _logger)
                            ?.ToList();
                        ViewRuleSendData(rule, takesItems);
                        continue;

                    //ДАННЫЕ--------------------------------------------------------------  
                    case RuleSwitcher4InData.InDataHandler:
                        var filtredItems = inData.Datas?.Filter(ruleOption.WhereFilter, _logger);
                        if (filtredItems == null || !filtredItems.Any())
                            continue;

                        takesItems = filtredItems.Order(ruleOption.OrderBy, _logger)
                            .TakeItems(ruleOption.TakeItems, ruleOption.DefaultItemJson, _logger)
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
                StatusDict["RuleName"] = $"{rule.GetCurrentOption().Name}";
                //_logger.Information($"Отправка ДАННЫХ через {rule.Option.Name}. Кол-во данных:{takesItems.Count}");


                foreach (var viewRule in rule.GetViewRules)
                {
                    foreach (var request in viewRule.GetDataRequestString(takesItems))
                    {
                        if (request == null) //правило отображения не подходит под ДАННЫЕ
                            continue;

                        _currentRequest = request;
                        InputData = new InDataWrapper<AdInputType> { Datas = _currentRequest.BatchedData.ToList() };
                        StatusDict["viewRule.Id"] = $"{viewRule.GetCurrentOption().Id}";
                        StatusDict["BodyLenght"] = $"{_currentRequest.BodyLenght}";
                        RaiseSendDataRx.OnNext(this);
                    }
                }
            }
        }


        /// <summary>
        /// Отправить команду через первое ViewRule.
        /// </summary>
        private void ViewRuleSendCommand(Rule rule, Command4Device command)
        {
            var commandViewRule = rule.GetViewRules.FirstOrDefault();
            _currentRequest = commandViewRule?.GetCommandRequestString();
            InputData = new InDataWrapper<AdInputType> { Command = command };
            StatusDict["Command"] = $"{command}";
            StatusDict["RuleName"] = $"{rule.GetCurrentOption().Name}";
            StatusDict["viewRule.Id"] = $"{commandViewRule.GetCurrentOption().Id}";
            RaiseSendDataRx.OnNext(this);
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