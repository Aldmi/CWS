using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Services.InputData;
using Confluent.Kafka;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Base.InData;
using Infrastructure.Background.Abstarct;
using Infrastructure.MessageBroker.Abstract;
using Newtonsoft.Json;
using Serilog;

namespace App.Services.MessageBroker
{
    /// <summary>
    /// Фоновый прлцесс получения даныых от брокера сообщений
    /// Входные данные поступают на нужные ус-ва.
    /// СТАРТ/СТОП сервиса происходит через background.
    /// background - это отдельный сервис
    /// </summary>
    /// <typeparam name="TIn">Тип обрабатываемых входных данных</typeparam>
    public class ConsumerMessageBroker4InputData<TIn> : IDisposable where TIn : InputTypeBase
    {
        #region field
        private readonly IConsumer  _consumer;
        private readonly ILogger _logger;
        private readonly InputDataApplyService<TIn> _inputDataApplyService;
        private readonly ISimpleBackground _background;
        private readonly int _batchSize;
        private IDisposable _registration;
        #endregion



        #region prop
        public bool BgAutoStart => _background.AutoStart;
        #endregion




        #region ctor
        public ConsumerMessageBroker4InputData(
            ISimpleBackground background,
            IConsumer consumer,
            ILogger logger,
            InputDataApplyService<TIn> inputDataApplyService,
            int batchSize)
        {
            _background = background;
            _batchSize = batchSize;
            _consumer = consumer;
            _logger = logger;
            _inputDataApplyService = inputDataApplyService;
            background.AddOneTimeAction(RunAsync); 
        }
        #endregion




        #region Methode

        public string GetConsumerBgState()
        {
            var bgState = _background.IsStarted ? "Started" : "Stoped";
            return bgState;
        }


        public async Task<Result> StartConsumerBg()
        {
            try
            {
                if (_background.IsStarted)
                {
                    return Result.Failure("Listener already started !!!");
                }

                await _background.StartAsync(CancellationToken.None);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{Type}", "Ошибка в InputDataController/StartConsumerBg");
                return Result.Failure("Ошибка в InputDataController/StartConsumerBg");
            }
        }


        public async Task<Result> StopConsumerBg()
        {
            try
            {
                if (!_background.IsStarted)
                {
                    return Result.Failure("Listener already stoped !!!");
                }

                await _background.StopAsync(CancellationToken.None);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{Type}", "Ошибка в InputDataController/StopConsumerBg");
                return Result.Failure("Ошибка в InputDataController/StopConsumerBg");
            }
        }


        private async Task RunAsync(CancellationToken cancellationToken)
        {
            var observable = _consumer.Consume(cancellationToken);  //создаем наблюдаемую коллекцию сообщений с брокера

            var subscription = observable
                .Buffer(_batchSize)
                .Subscribe(async messages =>
                    {
                      await MessageHandler(messages.ToList());
                      _consumer.CommitAsync(messages[messages.Count - 1]).GetAwaiter().GetResult();  //ручной коммит офсета для этого консьюмера
                    });

            var taskCompletionSource = new TaskCompletionSource<object>();  // задача (отписка от RX события) заверешающаяся только при сработке cancellationToken 
            _registration = cancellationToken.Register(
                () =>
                {
                    subscription.Dispose();
                    taskCompletionSource.SetResult(null);
                });

            await taskCompletionSource.Task;
        }

        #endregion



        #region EventHandler

        private async Task MessageHandler(IReadOnlyList<Message<Null, string>> messages)
        {
            //Обработчик сообщений с kafka
            foreach (var message in messages)
            {
                try
                {
                    var value = message.Value;
                    var inputDatas = JsonConvert.DeserializeObject<List<InputData<TIn>>>(value);
                    var errors= await _inputDataApplyService.ApplyInputData(inputDatas);  //TODO: Выставлять обратно на шину (Produser) ответ об ошибках.
                }
                catch (Exception e)
                {
                    //LOG
                    Console.WriteLine(e);
                }

                //Console.WriteLine($"MessageBrokerGetingData {message.Topic}/{message.Partition} @{message.Offset}: '{message.Value}'");
            }
        }

        #endregion



        #region Disposable

        public void Dispose()
        {
            _registration?.Dispose();
            _consumer?.Dispose();
        }

        #endregion
    }
}