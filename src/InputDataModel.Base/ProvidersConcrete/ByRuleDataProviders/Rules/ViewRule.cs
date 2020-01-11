using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InseartServices.DependentInsearts;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Factory;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Handlers;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.ProvidersOption;
using Serilog;
using Shared.Extensions;
using Shared.Helpers;
using Shared.Services.StringInseartService;
using Shared.Services.StringInseartService.DependentInseart;
using Shared.Services.StringInseartService.IndependentInseart;
using Shared.Types;

namespace Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules
{
    /// <summary>
    /// Правило отображения порции даных
    /// STX - \u0002
    /// RTX - \u0003
    /// </summary>
    public class ViewRule<TIn>
    {
        #region fields
        public const string Pattern = @"\{([^:{]+)(:[^{}]+)?\}";

        private readonly ILogger _logger;
        private readonly StringBuilder _headerExecuteInseartsResult;                         //Строка Header после IndependentInserts
        private readonly IndependentInsertsService _requestBodyParserModel;                  //модель вставки IndependentInserts в ТЕЛО ЗАПРОСА
        private readonly StringBuilder _footerExecuteInseartsResult;                         //Строка Footer после IndependentInserts
        private readonly DependentInseartsService _requestDependentInseartsService;          //Сервис вставки зависимых данных в общий ЗАПРОС (header+body+footer)

        private readonly ResponseTransfer _responseTransfer;                                 //Ответ после всех вставок
        #endregion



        #region ctor
        private ViewRule(ViewRuleOption option,
            StringBuilder headerExecuteInseartsResult,
            IndependentInsertsService requestBodyParserModel,
            StringBuilder footerExecuteInseartsResult,
            DependentInseartsService requestDependentInseartsService,
            ResponseTransfer responseTransfer,
            ILogger logger)
        {
   
            GetCurrentOption = option;
            _headerExecuteInseartsResult = headerExecuteInseartsResult;
            _requestBodyParserModel = requestBodyParserModel;
            _footerExecuteInseartsResult = footerExecuteInseartsResult;
            _requestDependentInseartsService = requestDependentInseartsService;
            _responseTransfer = responseTransfer;
            _logger = logger;
        }
        #endregion



        public static ViewRule<TIn> Create(string addressDevice, ViewRuleOption option, IIndependentInseartsHandlersFactory inputTypeInseartsHandlersFactory, ILogger logger)
        {
            var header = option.RequestOption.Header;
            var body = option.RequestOption.Body;
            var footer = option.RequestOption.Footer;

            //список фабрик, создающих нужные обработчики
            var handlerFactorys= new List<Func<StringInsertModel, IIndependentInsertsHandler>>
            {
                new BaseIndependentInseartsHandlersFactory().Create,
                inputTypeInseartsHandlersFactory.Create
            };
            var hIndependentInsertsService = IndependentInsertsServiceFactory.CreateIndependentInsertsService(header, Pattern, handlerFactorys, logger);
            var bIndependentInsertsService = IndependentInsertsServiceFactory.CreateIndependentInsertsService(body, Pattern, handlerFactorys, logger);
            var fIndependentInsertsService = IndependentInsertsServiceFactory.CreateIndependentInsertsService(footer, Pattern, handlerFactorys, logger);

            var headerExecuteInseartsResult = hIndependentInsertsService.ExecuteInsearts(new Dictionary<string, string> { { "AddressDevice", addressDevice } }).result;
            var footerExecuteInseartsResult = fIndependentInsertsService.ExecuteInsearts(null).result;
        
            var requestDependentInseartsService = DependentInseartsServiceFactory.Create(header + body + footer);

            var (_, isFailure, responseTransfer, error) = CreateResponseTransfer(option.ResponseOption, addressDevice, logger);
            if(isFailure) throw new ArgumentException(error); //???

            var viewRule= new ViewRule<TIn>(option, headerExecuteInseartsResult, bIndependentInsertsService, footerExecuteInseartsResult, requestDependentInseartsService, responseTransfer, logger);
            return viewRule;
        }



        #region prop
        public ViewRuleOption GetCurrentOption { get; }
        #endregion



        #region Methode
        /// <summary>
        /// Создать строку запроса ПОД ДАННЫЕ, подставив в форматную строку запроса значения переменных из списка items.
        /// </summary>
        /// <param name="items">элементы прошедшие фильтрацию для правила</param>
        /// <returns>строку запроса и батч данных в обертке </returns>
        public IEnumerable<ProviderTransfer<TIn>> CreateProviderTransfer4Data(List<TIn> items)
        {
            var viewedItems = GetViewedItems(items);
            if (viewedItems == null)
            {
                yield return null;
            }
            else
            {
                int numberOfBatch = 0;
                foreach (var batch in viewedItems.Batch(GetCurrentOption.BatchSize))
                {
                    var startItemIndex = GetCurrentOption.StartPosition + (numberOfBatch++ * GetCurrentOption.BatchSize);

                    #region РЕАЛИЗАЦИЯ СПИСКА ЗАПРОС/ОТВЕТ ДЛЯ КАЖДОГО ViewRule (SendingUnitList)

                    //1. requestOption и responseOption обернуть в тип SendingUnit
                    //2. ViewRule хранит List<SendingUnit> SendingUnitList
                    //foreach (var sendingUnit in _option.SendingUnitList)
                    //{
                    //    var requestOption = sendingUnit.RequestOption;
                    //    var responseOption = sendingUnit.ResponseOption;

                    //    var stringRequest = CreateRequestTransfer4Data(batch, requestOption, startItemIndex); //requestOption передаем
                    //    var stringResponse = CreateResponseTransfer(responseOption);//responseOption передаем
                    //    if (stringRequest == null)
                    //        continue;

                    //    yield return new ViewRuleRequestModelWrapper
                    //    {
                    //        StartItemIndex = startItemIndex,
                    //        BatchSize = _option.BatchSize,
                    //        BatchedData = batch,
                    //        StringRequest = stringRequest,
                    //        StringResponse = stringResponse,
                    //        RequestOption = _option.RequestOption,
                    //        ResponseOption = _option.ResponseOption
                    //    };
                    //}
                    #endregion

                    RequestTransfer<TIn> request;
                    try
                    {
                        var (_, isFailure, value, error) = CreateRequestTransfer4Data(batch, startItemIndex);
                        if (isFailure)
                        {
                            _logger.Warning(error);
                            continue;
                        }
                        request = value;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"neizvestnaya Ошибка формирования запроса или ответа ViewRuleId= {GetCurrentOption.Id}   {ex}"); //????
                        continue;
                    }

                    yield return new ProviderTransfer<TIn>
                    {
                        Request = request,
                        Response = _responseTransfer,
                        Command = Command4Device.None
                    };
                }
            }
        }


        /// <summary>
        /// Создать строку запроса ПОД КОМАНДУ.
        /// Body содержит готовый запрос для команды.
        /// </summary>
        /// <returns></returns>
        public ProviderTransfer<TIn> CreateProviderTransfer4Command(Command4Device command)  //TODO: Когда будет отдельный список команд, нужно формировать Dictionary<Command,ProviderTransfer<TIn>>.
        {
            //ФОРМИРОВАНИЕ ЗАПРОСА--------------------------------------------------------------------------------------
            var (_, isFailureReq, request, error) = CreateRequestTransfer4Command();
            if (isFailureReq)
            {
                _logger.Error($"CreateProviderTransfer4Command ERROR= {error}");
                return null;
            }

            return new ProviderTransfer<TIn>
            {
                Request = request,
                Response = _responseTransfer,
                Command = command
            };
        }


        /// <summary>
        /// Вернуть элементы из диапазона укзанного в правиле отображения
        /// Если границы диапазона не прпавильны вернуть null
        /// </summary>
        private IEnumerable<TIn> GetViewedItems(List<TIn> items)
        {
            try
            {
                return items.GetRange(GetCurrentOption.StartPosition, GetCurrentOption.Count);
            }
            catch (Exception)
            {
                return null;
            }
        }    //TODO: вставить в метод как Func


        /// <summary>
        /// Создать Запрос (используя форматную строку RequestOption) из одного батча данных.
        /// </summary>
        private Result<RequestTransfer<TIn>> CreateRequestTransfer4Data(IEnumerable<TIn> batch, int startItemIndex)
        {
            var items = batch.ToList();
            var format = GetCurrentOption.RequestOption.Format;
            var maxBodyLenght = GetCurrentOption.RequestOption.MaxBodyLenght;

            //INDEPENDENT insearts-------------------------------------------------------------------------------
            var processedItems = new List<ProcessedItem<TIn>>();
            var sbBodyResult = new StringBuilder();
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var currentRow = startItemIndex + i + 1;
                var (result, inseartedDict) = _requestBodyParserModel.ExecuteInsearts(item, new Dictionary<string, string> { { "rowNumber", currentRow.ToString() } });
                processedItems.Add(new ProcessedItem<TIn>(item, inseartedDict));
                sbBodyResult.Append(result);
            }
            var sbAppendResult = new StringBuilder().Append(_headerExecuteInseartsResult).Append(sbBodyResult).Append(_footerExecuteInseartsResult);
            var appendResultStr = sbAppendResult.ToString(); //TODO: Переход к старому коду зависимой вставки. Нужно его переделать на работу с StringBuilder

            //DEPENDENT insearts------------------------------------------------------------------------------------------------------
            string str = appendResultStr;
            if (_requestDependentInseartsService != null)
            {
                var (_, isFailure, value, error) = _requestDependentInseartsService.ExecuteInseart(appendResultStr, format);
                if (isFailure)
                {
                    return Result.Failure<RequestTransfer<TIn>>(error);
                }
                str = value;
            }

            //CHECK LIMIT---------------------------------------------------------------------------------------------------------------
            var (res, outOfLimit) = str.CheckLimitLenght(maxBodyLenght);
            if (res)
            {
                return Result.Failure<RequestTransfer<TIn>>($"Строка тела запроса СЛИШКОМ БОЛЬШАЯ. Превышение на {outOfLimit}");
            }

            ////CHECK RESULT STRING--------------------------------------------------------------------------------
            //var (_, f, _, e) = CheckResultString(str);
            //if(f)
            //    return Result.Failure<RequestTransfer<TIn>>(e);

            //FORMAT SWITCHER------------------------------------------------------------------------------------------------------------
            var (newStr, newFormat) = HelperFormatSwitcher.CheckSwitch2Hex(str, format);

            //ФОРМИРОВАНИЕ ОБЪЕКТА ЗАПРОСА.------------------------------------------------------------------------------------------------
            var request = new RequestTransfer<TIn>(GetCurrentOption.RequestOption)
            {
                StrRepresentBase = new StringRepresentation(str, format),
                StrRepresent = new StringRepresentation(newStr, newFormat),
                ProcessedItemsInBatch = new ProcessedItemsInBatch<TIn>(startItemIndex, items.Count, processedItems)
            };
            return Result.Ok(request);
        }


        /// <summary>
        /// Создать запрос для комманды.
        /// </summary>
        /// <returns></returns>
        private Result<RequestTransfer<TIn>>  CreateRequestTransfer4Command()
        {
            var format = GetCurrentOption.RequestOption.Format;

            //INDEPENDENT insearts-------------------------------------------------------------------------------
            var (sbBodyResult, _) = _requestBodyParserModel.ExecuteInsearts(null);
            var sbAppendResult = new StringBuilder().Append(_headerExecuteInseartsResult).Append(sbBodyResult).Append(_footerExecuteInseartsResult);
            var appendResultStr = sbAppendResult.ToString(); //TODO: Переход к старому коду зависимой вставки. Нужно его переделать на работу с StringBuilder

            //DEPENDENT insearts----------------------------------------------------------------------------------
            string str = appendResultStr;
            if (_requestDependentInseartsService != null)
            {
                var (_, isFailure, value, error) = _requestDependentInseartsService.ExecuteInseart(appendResultStr, format);
                if (isFailure)
                {
                    return Result.Failure<RequestTransfer<TIn>>(error);
                }
                str = value;
            }

            //CHECK RESULT STRING--------------------------------------------------------------------------------
            var (_, f, _, e) = CheckResultString(str);
            if(f)
                return Result.Failure<RequestTransfer<TIn>>(e);

            //FORMAT SWITCHER-------------------------------------------------------------------------------------
            var (newStr, newFormat) = HelperFormatSwitcher.CheckSwitch2Hex(str, format);

            //ФОРМИРОВАНИЕ ОБЪЕКТА ЗАПРОСА.-----------------------------------------------------------------------
            var request = new RequestTransfer<TIn>(GetCurrentOption.RequestOption)
            {
                StrRepresentBase = new StringRepresentation(str, format),
                StrRepresent = new StringRepresentation(newStr, newFormat)
            };
            return Result.Ok(request);
        }


        /// <summary>
        /// Создать строку Ответа (используя форматную строку ResponseOption).
        /// </summary>
        private static Result<ResponseTransfer> CreateResponseTransfer(ResponseOption responseOption, string addressDevice, ILogger logger)
        {
            var format = responseOption.Format;
            var body = responseOption.Body;
            var handlerFactorys= new List<Func<StringInsertModel, IIndependentInsertsHandler>>
            {
                new BaseIndependentInseartsHandlersFactory().Create,
            };
            var indInsServ = IndependentInsertsServiceFactory.CreateIndependentInsertsService(body, Pattern, handlerFactorys, logger);

            //INDEPENDENT insearts---------------------------------------------------------------------------------------------------------
            var (sbBodyResult, _) = indInsServ.ExecuteInsearts(new Dictionary<string, string> { { "AddressDevice", addressDevice } });
            var appendResultStr = sbBodyResult.ToString();

            //DEPENDENT insearts-----------------------------------------------------------------------------------------------------------
            string str = appendResultStr;
            var depInsServ = DependentInseartsServiceFactory.Create(body);
            if (depInsServ != null)
            {
                var (_, isFailure, value, error) = depInsServ.ExecuteInseart(appendResultStr, format);
                if (isFailure)
                {
                    return Result.Failure<ResponseTransfer>(error);
                }
                str = value;
            }

            ////CHECK RESULT STRING---------------------------------------------------------------------------------------------------------
            //var (_, f, _, e) = CheckResultString(str);
            //if (f)
            //    return Result.Failure<ResponseTransfer>(e);

            //FORMAT SWITCHER--------------------------------------------------------------------------------------------------------------
            var (newStr, newFormat) = HelperFormatSwitcher.CheckSwitch2Hex(str, format);

            //ФОРМИРОВАНИЕ ОБЪЕКТА ОТВЕТА.--------------------------------------------------------------------------------------------------
            var response = new ResponseTransfer(responseOption)
            {
                StrRepresentBase = new StringRepresentation(str, format),
                StrRepresent = new StringRepresentation(newStr, newFormat)
            };
            return Result.Ok(response);
        }


        /// <summary>
        /// 
        /// </summary>
        private Result<string> CheckResultString(string str)
        {
            if(str.Contains("{") || str.Contains("}"))
            {
                return Result.Failure<string>(@"str contains {  or  }!!!");
            }
            return Result.Ok(str);
        }

        #endregion
    }
}