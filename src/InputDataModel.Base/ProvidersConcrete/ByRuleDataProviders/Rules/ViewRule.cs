using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.ProvidersOption;
using Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers;
using Domain.InputDataModel.Shared.StringInseartService.InlineInseart;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Serilog;
using Shared.Extensions;

namespace Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules
{
    /// <summary>
    /// Правило отображения порции даных
    /// </summary>
    public class ViewRule<TIn>
    {
        #region fields
        private readonly ILogger _logger;
        private readonly IReadOnlyList<UnitOfSending<TIn>> _uosList;
        #endregion



        #region prop
        public ViewRuleOption GetCurrentOption { get; }
        #endregion



        #region ctor
        private ViewRule(ViewRuleOption option, IReadOnlyList<UnitOfSending<TIn>> uosList, ILogger logger)
        {
            GetCurrentOption = option;
            _uosList = uosList;
            _logger = logger;
        }
        #endregion



        #region Methode
        public static ViewRule<TIn> Create(
            ViewRuleOption option,
            string addressDevice,
            IIndependentInseartsHandlersFactory inputTypeInseartsHandlersFactory,
            IReadOnlyDictionary<string, StringInsertModelExt> stringInsertModelExtDict,
            InlineInseartService inlineInseartService,
            ILogger logger)
        {
            var uosList = option.UnitOfSendings
                .Select(o => UnitOfSending<TIn>.Create(o, addressDevice, (batchSize: option.BatchSize, count: option.Count), inputTypeInseartsHandlersFactory, stringInsertModelExtDict, inlineInseartService, logger))
                .ToList();
            return new ViewRule<TIn>(option, uosList, logger);
        }


        /// <summary>
        /// Создать запрос/ответ ПОД ДАННЫЕ, подставив в форматную строку запроса значения переменных из списка items.
        /// Отправка данных, разбитых на батчи.
        /// Каждый батч отправляется списком _uosList
        /// </summary>
        /// <param name="items">элементы прошедшие фильтрацию для правила</param>
        /// <param name="ct"></param>
        /// <returns>строку запроса и батч данных в обертке</returns>
        public async IAsyncEnumerable<Result<ProviderTransfer<TIn>>> CreateProviderTransfer4Data(List<TIn> items, [EnumeratorCancellation] CancellationToken ct = default)
        {
            var (_, isFail, viewedItems, err) = GetViewedItems(items);
            if (isFail)
            {
                yield return Result.Failure<ProviderTransfer<TIn>>(err);
            }
            else
            {
                int numberOfBatch = 0;
                foreach (var batch in viewedItems.Batch(GetCurrentOption.BatchSize))
                {
                    var startItemIndex = GetCurrentOption.StartPosition + (numberOfBatch++ * GetCurrentOption.BatchSize);
                    foreach (var uos in _uosList)
                    {
                        var (isSuccess, _, requestTransfer, error) = uos.CreateRequestTransfer4Data(batch, startItemIndex, numberOfBatch);
                        yield return isSuccess ?
                            Result.Ok(new ProviderTransfer<TIn> { Request = requestTransfer, Response = uos.ResponseTransfer, Command = Command4Device.None }) :
                            Result.Failure<ProviderTransfer<TIn>>($"ViewRuleId= {GetCurrentOption.Id}. {error}");
                    }
                }
            }
        }


        /// <summary>
        /// Вернуть элементы из диапазона укзанного в правиле отображения
        /// Если границы диапазона не правильны вернуть null
        /// </summary>
        private Result<IEnumerable<TIn>> GetViewedItems(List<TIn> items)
        {
            try
            {
                var resultItems = items.GetRange(GetCurrentOption.StartPosition, GetCurrentOption.Count);
                return Result.Ok<IEnumerable<TIn>>(resultItems);
            }
            catch (Exception ex)
            {
                return Result.Failure<IEnumerable<TIn>>($"Границы диапазона выбора данных не правильны  StartPosition= '{GetCurrentOption.StartPosition}' Count= '{GetCurrentOption.Count}'  Exception='{ex.Message}'");
            }
        }


        /// <summary>
        /// Создать запрос/ответ ПОД КОМАНДУ.
        /// Body содержит готовый запрос для команды.
        /// </summary>
        public async IAsyncEnumerable<Result<ProviderTransfer<TIn>>> CreateProviderTransfer4Command(Command4Device command, [EnumeratorCancellation] CancellationToken ct = default)  //TODO: Когда будет отдельный список команд, нужно формировать Dictionary<Command,ProviderTransfer<TIn>>.
        {
            foreach (var uos in _uosList)
            {
                var (_, isFailure, requestTransfer, error) = uos.CreateRequestTransfer4Command();
                yield return isFailure ?
                    Result.Ok(new ProviderTransfer<TIn> { Request = requestTransfer, Response = uos.ResponseTransfer, Command = command }) :
                    Result.Failure<ProviderTransfer<TIn>>($"ViewRuleId= {GetCurrentOption.Id}. {error}");
            }
        }
        #endregion
    }
}