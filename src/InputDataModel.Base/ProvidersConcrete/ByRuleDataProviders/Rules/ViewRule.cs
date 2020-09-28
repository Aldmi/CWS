using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.ProvidersOption;
using Domain.InputDataModel.Base.Response.ResponseValidators;
using Domain.InputDataModel.Shared.StringInseartService.DependentInseart;
using Domain.InputDataModel.Shared.StringInseartService.IndependentInseart;
using Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers;
using Domain.InputDataModel.Shared.StringInseartService.Model;
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
        private readonly ILogger _logger;
        private readonly StringBuilder _headerExecuteInseartsResult;                                              //Строка Header после IndependentInserts
        private readonly IndependentInsertsService _bodyIndependentInsertsService;                                //модель вставки IndependentInserts в ТЕЛО ЗАПРОСА
        private readonly StringBuilder _footerExecuteInseartsResult;                                              //Строка Footer после IndependentInserts
        /// <summary>
        /// Массив сервисов вставки зависимых данных в общий ЗАПРОС (header+body+footer).
        /// Для каждого батча -> своя строка (склееное body) -> из склееной строки выделяем все обработчики DependentInseart.
        /// Т.е. для кажого батча будет свой DependentInseartService (с разным кол-вом DependentInseart обработчиков). 
        /// </summary>
        private readonly DependentInseartService[] _requestdepInsServCollection;
        private readonly ResponseTransfer _responseTransfer;                                                      //Ответ после всех вставок
        #endregion



        #region ctor
        private ViewRule(ViewRuleOption option,
            StringBuilder headerExecuteInseartsResult,
            IndependentInsertsService bodyIndependentInsertsService,
            StringBuilder footerExecuteInseartsResult,
            DependentInseartService[] requestdepInsServCollection,
            ResponseTransfer responseTransfer,
            ILogger logger)
        {
            GetCurrentOption = option;
            _headerExecuteInseartsResult = headerExecuteInseartsResult;
            _bodyIndependentInsertsService = bodyIndependentInsertsService;
            _footerExecuteInseartsResult = footerExecuteInseartsResult;
            _requestdepInsServCollection = requestdepInsServCollection;
            _responseTransfer = responseTransfer;
            _logger = logger;
        }
        #endregion



        public static ViewRule<TIn> Create(
            string addressDevice,
            ViewRuleOption option,
            IIndependentInseartsHandlersFactory inputTypeInseartsHandlersFactory,
            IReadOnlyDictionary<string, StringInsertModelExt> stringInsertModelExtDict,
            ILogger logger)
        {
            var header = option.RequestOption.Header;
            var body = option.RequestOption.Body;
            var footer = option.RequestOption.Footer;

            //список фабрик, создающих нужные обработчики
            var handlerFactorys = new List<Func<StringInsertModel, IIndependentInsertsHandler>>
            {
                new DefaultIndependentInseartsHandlersFactory().Create,
                inputTypeInseartsHandlersFactory.Create
            };
            var hIndependentInsertsService = IndependentInsertsServiceFactory.CreateIndependentInsertsService(header, handlerFactorys, stringInsertModelExtDict, logger);
            var bIndependentInsertsService = IndependentInsertsServiceFactory.CreateIndependentInsertsService(body,  handlerFactorys, stringInsertModelExtDict, logger);
            var fIndependentInsertsService = IndependentInsertsServiceFactory.CreateIndependentInsertsService(footer, handlerFactorys, stringInsertModelExtDict, logger);

            var hExecuteInseartsResult = hIndependentInsertsService.ExecuteInsearts(new Dictionary<string, string> { { "AddressDevice", addressDevice } }).result;
            var fExecuteInseartsResult = fIndependentInsertsService.ExecuteInsearts(null).result;

            var requestdepInsServCollection = CreateDependentInseartServiceCollection(option.BatchSize, option.Count, header, body, footer, stringInsertModelExtDict);

            var (_, isFailure, responseTransfer, error) = CreateResponseTransfer(option.ResponseOption, addressDevice, stringInsertModelExtDict, logger);
            if (isFailure) throw new ArgumentException(error); //???

            var viewRule = new ViewRule<TIn>(option, hExecuteInseartsResult, bIndependentInsertsService, fExecuteInseartsResult, requestdepInsServCollection, responseTransfer, logger);
            return viewRule;
        }



        #region prop
        public ViewRuleOption GetCurrentOption { get; }
        #endregion



        #region Methode
        /// <summary>
        /// Создает массив из сервисов Зависимой вставки.
        /// Ко-во сервисов определяется как разбивается строка на батчи исходя из опций option.BatchSize, option.Count
        /// Для каждого сервиса DependentInseartsServiceFactory определяет свой набор обработчиков.
        /// Номер батча является индексом этого массива. Для выбора нужного сервиса.
        /// </summary>
        private static DependentInseartService[] CreateDependentInseartServiceCollection(int batchSize, int count, string header, string body, string footer, IReadOnlyDictionary<string, StringInsertModelExt> stringInsertModelExtDict)
        {
            //TODO:  Для Command не нужна логика обработки вх. данных. batchSize,  count, agregateFilter отсутствуют. Возможно сделать иерархию типов Rule и ViewRule для разных списков (список данных, комманд, Запросы инициализации, Аварийный список.)
            //Для Command.
            batchSize = batchSize == 0 ? 1 : batchSize;
            count = count == 0 ? 1 : count;

            var requestdepInsServCollection = HelperString.CalcBatchedSequence(body, count, batchSize)
                .Select(item => DependentInseartsServiceFactory.Create(header + item + footer, stringInsertModelExtDict))
                .ToArray();
            return requestdepInsServCollection;
        }


        /// <summary>
        /// Создать строку запроса ПОД ДАННЫЕ, подставив в форматную строку запроса значения переменных из списка items.
        /// </summary>
        /// <param name="items">элементы прошедшие фильтрацию для правила</param>
        /// <param name="ct"></param>
        /// <returns>строку запроса и батч данных в обертке </returns>
        public async IAsyncEnumerable<Result<ProviderTransfer<TIn>>> CreateProviderTransfer4Data(List<TIn> items, [EnumeratorCancellation] CancellationToken ct = default)
        {
            var viewedItems = GetViewedItems(items);
            if (viewedItems == null)
            {
                yield return Result.Failure<ProviderTransfer<TIn>>("ViewRule.CreateProviderTransfer4Data.GetViewedItems. Границы диапазона выбора данных не правильны");
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
                    //3.
                    //foreach (var sendingUnit in _option.SendingUnitList)
                    //{
                    //   ProviderTransfer<TIn> pr= _sendingUnit.CreateUnit(batch, startItemIndex, numberOfBatch);
                    //   yield return pr;
                    //   
                    //}
                    #endregion

                    string errorCreateRequestTransfer4Data = null;
                    RequestTransfer<TIn> request = null;
                    try
                    {
                        var (_, isFailure, value, error) = CreateRequestTransfer4Data(batch, startItemIndex, numberOfBatch);
                        if (isFailure)
                        {
                            errorCreateRequestTransfer4Data = error;
                        }
                        else
                        {
                            request = value;
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCreateRequestTransfer4Data = $"Неизвестная Ошибка формирования запроса или ответа ViewRuleId= {GetCurrentOption.Id}   {ex}";
                    }

                    yield return (errorCreateRequestTransfer4Data == null) ?
                        Result.Ok(new ProviderTransfer<TIn> {Request = request, Response = _responseTransfer, Command = Command4Device.None}) :
                        Result.Failure<ProviderTransfer<TIn>>(errorCreateRequestTransfer4Data);
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
        /// Если границы диапазона не правильны вернуть null
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
        private Result<RequestTransfer<TIn>> CreateRequestTransfer4Data(IEnumerable<TIn> batch, int startItemIndex, int numberOfBatch)
        {
            var items = batch.ToList();
            var format = GetCurrentOption.RequestOption.Format;
            var maxBodyLenght = GetCurrentOption.RequestOption.MaxBodyLenght;

            //INDEPENDENT insearts-----------------------------------------------------------------------------------------------------
            var processedItems = new List<ProcessedItem<TIn>>();
            var sbBodyResult = new StringBuilder();
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var currentRow = startItemIndex + i + 1;
                var (result, inseartedDict) = _bodyIndependentInsertsService.ExecuteInsearts(item, new Dictionary<string, string> { { "rowNumber", currentRow.ToString() } });
                processedItems.Add(new ProcessedItem<TIn>(item, inseartedDict));
                sbBodyResult.Append(result);
            }
            var sbAppendResult = new StringBuilder().Append(_headerExecuteInseartsResult).Append(sbBodyResult).Append(_footerExecuteInseartsResult);

            //DEPENDENT insearts------------------------------------------------------------------------------------------------------
            if (_requestdepInsServCollection != null)
            {
                var requestDependentInseartsService = _requestdepInsServCollection[numberOfBatch - 1];
                var (_, isFailure, value, error) = requestDependentInseartsService.ExecuteInsearts(sbAppendResult, format);
                if (isFailure)
                {
                    return Result.Failure<RequestTransfer<TIn>>(error);
                }
                sbAppendResult = value;
            }

            var str = sbAppendResult.ToString(); //TODO: Переход к старому коду зависимой вставки. Нужно его переделать на работу с StringBuilder
            //CHECK LIMIT---------------------------------------------------------------------------------------------------------------
            var (res, outOfLimit) = str.CheckLimitLenght(maxBodyLenght);
            if (res)
            {
                return Result.Failure<RequestTransfer<TIn>>($"Строка тела запроса СЛИШКОМ БОЛЬШАЯ. Превышение на {outOfLimit}");
            }

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
        /// Создать запрос для команды.
        /// </summary>
        /// <returns></returns>
        private Result<RequestTransfer<TIn>> CreateRequestTransfer4Command()
        {
            var format = GetCurrentOption.RequestOption.Format;

            //INDEPENDENT insearts-------------------------------------------------------------------------------
            var (sbBodyResult, _) = _bodyIndependentInsertsService.ExecuteInsearts(null);
            var sbAppendResult = new StringBuilder().Append(_headerExecuteInseartsResult).Append(sbBodyResult).Append(_footerExecuteInseartsResult);

            //DEPENDENT insearts----------------------------------------------------------------------------------
            if (_requestdepInsServCollection != null)
            {
                var requestDependentInseartsService = _requestdepInsServCollection[0];//Всегда только 1 элемент, т.к. для команд нет данных.
                var (_, isFailure, value, error) = requestDependentInseartsService.ExecuteInsearts(sbAppendResult, format);
                if (isFailure)
                {
                    return Result.Failure<RequestTransfer<TIn>>(error);
                }
                sbAppendResult = value;
            }

            var str = sbAppendResult.ToString(); //TODO: Переход к старому коду зависимой вставки. Нужно его переделать на работу с StringBuilder
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
        private static Result<ResponseTransfer> CreateResponseTransfer(ResponseOption responseOption, string addressDevice, IReadOnlyDictionary<string, StringInsertModelExt> stringInsertModelExtDict, ILogger logger)
        {
            //СОЗДАТЬ ВАЛИДАТОР ОТВЕТА-------------------------------------------------------------------------------------------------------------
            var (_, isFail, validator, err) = responseOption.CreateValidator();
            if (isFail)
                return Result.Failure<ResponseTransfer>(err);

            switch (validator)
            {
                //ДЛЯ equalValidator ВЫПОЛНИМ ВСТАВКИ В СТРОКУ И ВОЗМОЖНУЮ СМЕНУ ФОРМАТА
                case EqualResponseValidator equalValidator:
                    var format = equalValidator.ExpectedData.Format;
                    var body = equalValidator.ExpectedData.Str;
                    var handlerFactorys = new List<Func<StringInsertModel, IIndependentInsertsHandler>>
                    {
                       new DefaultIndependentInseartsHandlersFactory().Create,
                    };
                    var indInsServ = IndependentInsertsServiceFactory.CreateIndependentInsertsService(body, handlerFactorys, stringInsertModelExtDict, logger);

                    //INDEPENDENT insearts---------------------------------------------------------------------------------------------------------
                    var (sbBodyResult, _) = indInsServ.ExecuteInsearts(new Dictionary<string, string> { { "AddressDevice", addressDevice } });

                    //DEPENDENT insearts-----------------------------------------------------------------------------------------------------------
                    var depInsServ = DependentInseartsServiceFactory.Create(body, stringInsertModelExtDict);
                    if (depInsServ != null)
                    {
                        var (_, isFailure, value, error) = depInsServ.ExecuteInsearts(sbBodyResult, format);
                        if (isFailure)
                        {
                            return Result.Failure<ResponseTransfer>(error);
                        }
                        sbBodyResult = value;
                    }

                    var str = sbBodyResult.ToString(); //TODO: Переход к старому коду зависимой вставки. Нужно его переделать на работу с StringBuilder
                    //FORMAT SWITCHER--------------------------------------------------------------------------------------------------------------
                    var (newStr, newFormat) = HelperFormatSwitcher.CheckSwitch2Hex(str, format);

                    //ФОРМИРОВАНИЕ ОБЪЕКТА ОТВЕТА.--------------------------------------------------------------------------------------------------
                    validator = new EqualResponseValidator(new StringRepresentation(newStr, newFormat)); //пересоздать валидатор, с измененной строкой и форматом
                    var response = new ResponseTransfer(responseOption, validator)
                    {
                        StrRepresentBase = new StringRepresentation(str, format),
                        StrRepresent = new StringRepresentation(newStr, newFormat)
                    };
                    return Result.Ok(response);

                default:
                    return Result.Ok(new ResponseTransfer(responseOption, validator));
            }
        }
        #endregion
    }
}