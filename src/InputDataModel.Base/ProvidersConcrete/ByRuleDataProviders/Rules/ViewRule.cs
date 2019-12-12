using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InseartServices.DependentInsearts;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.IndependentInseartsHandlers;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.ProvidersOption;
using Serilog;
using Shared.Extensions;
using Shared.Helpers;
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
        private const string Pattern = @"\{(.*?)(:.+?)?\}";
        private readonly string _addressDevice;
        private readonly ViewRuleOption _option;
        private readonly ILogger _logger;

        private readonly StringBuilder _headerExecuteInseartsResult;                         //Строка Header после IndependentInserts
        private readonly IndependentInsertsService _requestBodyParserModel;                  //модель вставки IndependentInserts в ТЕЛО ЗАПРОСА
        private readonly StringBuilder _footerExecuteInseartsResult;                         //Строка Footer после IndependentInserts
        private readonly DependentInseartsService _requestDependentInseartsService;          //Сервис вставки зависимых данных в общий ЗАПРОС (header+body+footer)

        private readonly ResponseTransfer _responseTransfer;                                 //Ответ после всех вставок
        #endregion



        #region ctor
        public ViewRule(string addressDevice, ViewRuleOption option, IIndependentInsertsHandler inTypeIndependentInsertsHandler, ILogger logger)
        {
            _addressDevice = addressDevice;
            _option = option;
            _logger = logger;

            var requestHeaderParserModel = IndependentInsertsService.IndependentInsertsParserModelFactory(_option.RequestOption.Header, Pattern);
            _requestBodyParserModel = IndependentInsertsService.IndependentInsertsParserModelFactory(_option.RequestOption.Body, Pattern, inTypeIndependentInsertsHandler);
            var requesFooterParserModel = IndependentInsertsService.IndependentInsertsParserModelFactory(_option.RequestOption.Footer, Pattern);
            _headerExecuteInseartsResult = requestHeaderParserModel.ExecuteInsearts(new Dictionary<string, string> { { "AddressDevice", _addressDevice } }).result;
            _footerExecuteInseartsResult = requesFooterParserModel.ExecuteInsearts(null).result;
            _requestDependentInseartsService = DependentInseartsService.DependentInseartsServiceFactory(_option.RequestOption.Header + _option.RequestOption.Body + _option.RequestOption.Footer);
            _responseTransfer = CreateResponseTransfer();
        }
        #endregion



        #region prop
        public ViewRuleOption GetCurrentOption => _option;
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
                foreach (var batch in viewedItems.Batch(_option.BatchSize))
                {
                    var startItemIndex = _option.StartPosition + (numberOfBatch++ * _option.BatchSize);

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
                        request = CreateRequestTransfer4Data(batch, startItemIndex); //TODO: obrabotat Result<T>. Exception only for unhandled erros
                        if (request == null)
                            continue;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Ошибка формирования запроса или ответа ViewRuleId= {_option.Id}    {ex}");
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
        public ProviderTransfer<TIn> CreateProviderTransfer4Command(Command4Device command)
        {
            //ФОРМИРОВАНИЕ ЗАПРОСА--------------------------------------------------------------------------------------
            var requestCommand = CreateRequestTransfer4Command();

            //ФОРМИРОВАНИЕ ОБЪЕКТА ОТВЕТА.-------------------------------------------------------------------------------
            var response = CreateResponseTransfer();

            return new ProviderTransfer<TIn>
            {
                Request = requestCommand,
                Response = response,
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
                return items.GetRange(_option.StartPosition, _option.Count);
            }
            catch (Exception)
            {
                return null;
            }
        }    //TODO: вставить в метод как Func


        /// <summary>
        /// Создать Запрос (используя форматную строку RequestOption) из одного батча данных.
        /// </summary>
        private RequestTransfer<TIn> CreateRequestTransfer4Data(IEnumerable<TIn> batch, int startItemIndex) //TODO: return Result<T>
        {
            var items = batch.ToList();
            var format = _option.RequestOption.Format;
            var maxBodyLenght = _option.RequestOption.MaxBodyLenght;

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

            //DEPENDENT insearts----------------------------------------------------------------------------------
            var res = _requestDependentInseartsService?.ExecuteInseart(appendResultStr, format) ?? appendResultStr;

            //CHECK LIMIT----------------------------------------------------------------------------------------
            var limitRes = res.CheckLimitLenght(maxBodyLenght);
            if (limitRes.res)
            {
                _logger.Warning($"Строка тела запроса СЛИШКОМ БОЛЬШАЯ. Превышение на {limitRes.OutOfLimit}");
                return null;
            }

            //FORMAT SWITCHER-------------------------------------------------------------------------------------
            var (newStr, newFormat) = HelperFormatSwitcher.CheckSwitch2Hex(res, format);

            //ФОРМИРОВАНИЕ ОБЪЕКТА ЗАПРОСА.------------------------------------------------------------------------------------------------
            var request = new RequestTransfer<TIn>(_option.RequestOption)
            {
                StrRepresentBase = new StringRepresentation(res, format),
                StrRepresent = new StringRepresentation(newStr, newFormat),
                ProcessedItemsInBatch = new ProcessedItemsInBatch<TIn>(startItemIndex, items.Count, processedItems)
            };
            return request;
        }


        /// <summary>
        /// Создать запрос для комманды.
        /// </summary>
        /// <returns></returns>
        private RequestTransfer<TIn> CreateRequestTransfer4Command()
        {
            var format = _option.RequestOption.Format;

            //INDEPENDENT insearts-------------------------------------------------------------------------------
            var (sbBodyResult, _) = _requestBodyParserModel.ExecuteInsearts(null);
            var sbAppendResult = new StringBuilder().Append(_headerExecuteInseartsResult).Append(sbBodyResult).Append(_footerExecuteInseartsResult);
            var appendResultStr = sbAppendResult.ToString(); //TODO: Переход к старому коду зависимой вставки. Нужно его переделать на работу с StringBuilder

            //DEPENDENT insearts----------------------------------------------------------------------------------
            var res = _requestDependentInseartsService?.ExecuteInseart(appendResultStr, format) ?? appendResultStr;

            //FORMAT SWITCHER-------------------------------------------------------------------------------------
            var (newStr, newFormat) = HelperFormatSwitcher.CheckSwitch2Hex(res, format);

            //ФОРМИРОВАНИЕ ОБЪЕКТА ЗАПРОСА.------------------------------------------------------------------------------------------------
            var request = new RequestTransfer<TIn>(_option.RequestOption)
            {
                StrRepresentBase = new StringRepresentation(res, format),
                StrRepresent = new StringRepresentation(newStr, newFormat)
            };

            return request;
        }


        /// <summary>
        /// Создать строку Ответа (используя форматную строку ResponseOption).
        /// </summary>
        private ResponseTransfer CreateResponseTransfer()
        {
            var format = _option.ResponseOption.Format;
            var responseBodyParserModel = IndependentInsertsService.IndependentInsertsParserModelFactory(_option.ResponseOption.Body, Pattern);
            var responseDependentInseartsService = DependentInseartsService.DependentInseartsServiceFactory(_option.RequestOption.Header + _option.RequestOption.Body + _option.RequestOption.Footer);

            //INDEPENDENT insearts---------------------------------------------------------------------------------------------------------
            var (sbBodyResult, _) = responseBodyParserModel.ExecuteInsearts(new Dictionary<string, string> { { "AddressDevice", _addressDevice } });
            var appendResultStr = sbBodyResult.ToString();

            //DEPENDENT insearts-----------------------------------------------------------------------------------------------------------
            var res = responseDependentInseartsService?.ExecuteInseart(appendResultStr, format) ?? appendResultStr;

            //FORMAT SWITCHER--------------------------------------------------------------------------------------------------------------
            var (newStr, newFormat) = HelperFormatSwitcher.CheckSwitch2Hex(res, format);

            //ФОРМИРОВАНИЕ ОБЪЕКТА ОТВЕТА.--------------------------------------------------------------------------------------------------
            var response = new ResponseTransfer(_option.ResponseOption)
            {
                StrRepresentBase = new StringRepresentation(res, format),
                StrRepresent = new StringRepresentation(newStr, newFormat)
            };
            return response;
        }
        #endregion
    }
}