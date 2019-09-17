using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using Domain.InputDataModel.Base.ProvidersOption;
using Domain.InputDataModel.Base.Response;
using Domain.InputDataModel.Base.Services;
using Serilog;
using Shared.Extensions;
using Shared.Helpers;

namespace Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders
{
    public class ByRulesDataProvider<TIn> : BaseDataProvider<TIn>, IDataProvider<TIn, ResponseInfo> where TIn : InputTypeBase
    {
        #region field

        private readonly List<Rule<TIn>> _rules;        // Набор правил, для обработки данных.
        private ViewRuleTransferWrapper<TIn> _current;  // Созданный запрос, после подготовки данных. 
        private readonly ILogger _logger;

        #endregion



        #region ctor

        public ByRulesDataProvider(IStronglyTypedResponseFactory stronglyTypedResponseFactory, ProviderOption providerOption, IIndependentInsertsService independentInsertsService, ILogger logger) : base(stronglyTypedResponseFactory, logger)
        {
            var option = providerOption.ByRulesProviderOption;
            if (option == null)
                throw new ArgumentNullException(providerOption.Name);

            ProviderName = providerOption.Name;
            _rules = option.Rules.Select(opt => new Rule<TIn>(opt, independentInsertsService, logger)).ToList();
            RuleName4DefaultHandle = string.IsNullOrEmpty(option.RuleName4DefaultHandle)
                ? "DefaultHandler"//_rules.First().Option.Name
                : option.RuleName4DefaultHandle;
            _logger = logger;
        }

        #endregion



        #region prop

        private IEnumerable<Rule<TIn>> GetRules => _rules.ToList();                     //Копия списка Rules, чтобы  избежать Exception при перечислении (т.к. Rules - мутабельна).
        public string RuleName4DefaultHandle { get; }
        public string ProviderName { get; }
        public Dictionary<string, string> StatusDict { get; } = new Dictionary<string, string>();
        public InDataWrapper<TIn> InputData { get; set; }
        public ResponseInfo OutputData { get; set; }
        public bool IsOutDataValid { get; set; }
        public int TimeRespone => _current.Response.Option.TimeRespone;        //Время на ответ
        public int CountSetDataByte => _current.Response.Option.Lenght;        //Кол-во принимаемых байт в ответе

        #endregion



        #region RxEvent

        public Subject<IDataProvider<TIn, ResponseInfo>> RaiseSendDataRx { get; } = new Subject<IDataProvider<TIn, ResponseInfo>>();

        #endregion



        #region IExchangeDataProviderImplementation

        /// <summary>
        /// Сформировать буфер ЗАПРОСА
        /// </summary>
        /// <returns></returns>
        public byte[] GetDataByte()
        {
            var stringRequset = _current.Request.StrRepresent.Str; 
            var format = _current.Request.StrRepresent.Format; 
            StatusDict["GetDataByte.Request"] = $"[{stringRequset}] Lenght= {stringRequset.Length}  Format={format}";
            StatusDict["GetDataByte.RequestBase"] = _current.Request.EqualStrRepresent ? null : $"[{_current.Request.StrRepresentBase.Str}]  Lenght= {_current.Request.StrRepresentBase.Str.Length}   Format= {_current.Request.StrRepresentBase.Format}";
            //Преобразовываем КОНЕЧНУЮ строку в массив байт
            var resultBuffer = stringRequset.ConvertString2ByteArray(format);
            StatusDict["GetDataByte.ByteRequest"] = $"{ resultBuffer.ArrayByteToString("X2")} Lenght= {resultBuffer.Length}";
            StatusDict["TimeResponse"] = $"{TimeRespone}";
            return resultBuffer;
        }


        /// <summary>
        /// Проверить ответ, Присвоить выходные данные.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool SetDataByte(byte[] data)
        {
            var stringResponseRef = _current.Response.StrRepresent.Str;
            var format = _current.Response.StrRepresent.Format;           
            if (data == null)
            {
                IsOutDataValid = false;
                OutputData = new ResponseInfo
                {
                    ResponseData = null,
                    Encoding = format,
                    IsOutDataValid = IsOutDataValid
                };
                return false;
            }
            var stringResponse = data.ArrayByteToString(format);

            //Создать строго типизитрованный ответ на базе строки сырого ответа
            var stronglyTypedResponse = CreateStronglyTypedResponseByOption(_current.Response.Option.StronglyTypedName, stringResponse);
            IsOutDataValid = (stringResponse == stringResponseRef); //TODO: как лутчше сравнивать строки???
            OutputData = new ResponseInfo
            {
                ResponseData = stringResponse,
                Encoding = format,
                IsOutDataValid = IsOutDataValid,
                StronglyTypedResponse = stronglyTypedResponse
            };
            var diffResp = (!IsOutDataValid) ? $"ПринятоБайт/ОжидаемБайт= {data.Length}/{_current.Response.Option.Lenght}" : string.Empty;
            StatusDict["SetDataByte.StringResponse"] = $"{stringResponseRef} ?? {stringResponse}   diffResp=  {diffResp}";
            return IsOutDataValid;
        }

        /// <summary>
        ///Если указанно имя типа для ответоа, то мы его создаем через фабрику.
        /// </summary>
        private StronglyTypedRespBase CreateStronglyTypedResponseByOption(string stronglyTypedName, string stringResponse)
        {
            StronglyTypedRespBase stronglyTypedResponse = null;
            StatusDict["SetDataByte.StronglyTypedResponse"] = null;
            if (!string.IsNullOrEmpty(_current.Response.Option.StronglyTypedName))  
            {
                try
                {
                    stronglyTypedResponse = StronglyTypedResponseFactory.CreateStronglyTypedResponse(stronglyTypedName, stringResponse);
                    StatusDict["SetDataByte.StronglyTypedResponse"] = stronglyTypedResponse.ToString();
                }
                catch (NotSupportedException ex)
                {
                    StatusDict["SetDataByte.StronglyTypedResponse"] = $"ОШИБКА= {ex}";
                }
            }
            return stronglyTypedResponse;
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
            //TODO: лутчше весь ByRulesDataProvider сделать Immutable, чем перезаписывать список _rules  
            //var option = optionNew.ByRulesProviderOption;
            //if (option == null)
            //    throw new ArgumentNullException(optionNew.Name);

            //_rules.Clear();
            //var newRules = option.Rules.Select(opt => new Rule<TIn>(opt, _logger)).ToList();//TODO: валидация проводить в самом dto объекте Rule и ViewRule
            //_rules.AddRange(newRules);

            return true;
        }

        #endregion




        #region Methode

        /// <summary>
        /// Запуск конвеера обработки запрос-ответ для обмена
        /// </summary>
        public async Task StartExchangePipeline(InDataWrapper<TIn> inData)
        {
            //ЕСЛИ ДАННЫХ ДЛЯ ОТПРАВКИ НЕТ (например для Цикл. обмена при старте)
            if (inData == null)
            {
                inData = new InDataWrapper<TIn>
                {
                    Datas = new List<TIn>(),
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
                        var takesItems = inData.Datas
                            ?.Order(ruleOption.OrderBy, _logger)
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
        private void ViewRuleSendData(Rule<TIn> rule, List<TIn> takesItems)
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

                        _current = request;
                        InputData = new InDataWrapper<TIn> { Datas = _current.BatchedData.ToList() };
                        StatusDict["viewRule.Id"] = $"{viewRule.GetCurrentOption.Id}";
                        StatusDict["Request.BodyLenght"] = $"{_current.Request.BodyLenght}";
                        RaiseSendDataRx.OnNext(this);
                    }
                }
            }
        }


        /// <summary>
        /// Отправить команду через первое ViewRule.
        /// </summary>
        private void ViewRuleSendCommand(Rule<TIn> rule, Command4Device command)
        {
            var commandViewRule = rule.GetViewRules.FirstOrDefault();
            _current = commandViewRule?.GetCommandRequestString();
            InputData = new InDataWrapper<TIn> { Command = command };
            StatusDict["Command"] = $"{command}";
            StatusDict["RuleName"] = $"{rule.GetCurrentOption().Name}";
            StatusDict["viewRule.Id"] = $"{commandViewRule.GetCurrentOption.Id}";
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
            throw new NotImplementedException();
        }

        public bool SetString(Stream stream)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}