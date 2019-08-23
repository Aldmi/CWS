using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AbstractProduser.AbstractProduser;
using AbstractProduser.Helpers;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using CSharpFunctionalExtensions;
using KafkaProduser.Options;

namespace KafkaProduser
{
    /// <summary>
    /// Отправка на шину сообщений Kaffka
    /// </summary>
    public class KafkaProduserWrapper : BaseProduser<KafkaProduserOption>
    {
        #region field

        //private readonly ILogger _logger;
        private readonly Producer<Null, string> _producer;

        #endregion



        #region ctor

        public KafkaProduserWrapper(KafkaProduserOption option) : base(option)
        {
            //_logger = logger;

            var config = new Dictionary<string, object>
            {
                { "bootstrap.servers", option.BrokerEndpoints },
                { "api.version.request", true },
                { "socket.blocking.max.ms", 1 },
                { "queue.buffering.max.ms", 5 },
                { "queue.buffering.max.kbytes", 10240 },
#if DEBUG
                { "debug", "msg" },
#endif
                {
                    "default.topic.config",
                    new Dictionary<string, object>
                    {
                        { "message.timeout.ms", 3000 },        //таймаут на подключение к брокеру (если таймаут вышел, то выставляется message.Error в ответе)
                        { "request.required.acks", -1 }        // гарантированная доставка сообщения до конкретного партишена. (самая высокая гарантия доставки в брокера)
                    }
                }
            };
            _producer = new Producer<Null, string>(config, new NullSerializer(), new StringSerializer(Encoding.UTF8));
            _producer.OnLog += OnLog;
            _producer.OnError += OnError;
        }

        #endregion



        #region EventHandler

        private void OnLog(object sender, LogMessage logMessage)
        {
            //_logger.LogInformation(
            //    "Producing to Kafka. Client: {client}, syslog level: '{logLevel}', message: {logMessage}.",
            //    logMessage.Name,
            //    logMessage.Level,
            //    logMessage.Message);
        }


        private void OnError(object sender, Error error)
        {
            //_logger.LogInformation("Producer error: {error}. No action required.", error);
        }

        #endregion



        #region OvverideMembers

        protected override async Task<Result<string, ErrorWrapper>> SendConcrete(string message, string invokerName = null, CancellationToken ct = default(CancellationToken))
        {
            try
            {
                invokerName = invokerName ?? Option.TopicName;
                var res = await ProduceAsync(invokerName, message);
                return res.Error != null ? 
                    Result.Fail<string, ErrorWrapper>(new ErrorWrapper(ResultError.RespawnProduserError, res.Error.ToString())) 
                    : Result.Ok<string, ErrorWrapper>(res.Value);
            }
            catch (KafkaException ex)
            {
                return Result.Fail<string, ErrorWrapper>(new ErrorWrapper(ResultError.RespawnProduserError, ex));
            }
        }


        protected override async Task<Result<string, ErrorWrapper>> SendConcrete(object message, string invokerName = null, CancellationToken ct = default(CancellationToken))
        {
            ////TODO:заменить на реальную отправку kaffka
            //await Task.Delay(1000, ct);
            //return Result.Ok<string, ErrorWrapper>("Ок");
            throw new NotImplementedException();
        }



        private async Task<Message<Null, string>> ProduceAsync(string topic, string value, int partition = -1)
        {
            try
            {
                Message<Null, string> message = null;
                if (partition < 0)
                {
                    message = await _producer.ProduceAsync(topic, null, value);
                }
                else
                {
                    message = await _producer.ProduceAsync(topic, null, value, partition);
                }

                if (message.Error.HasError)          //Продюсеру не нужен переконнект к брокеру в случае ошибки, он либо может отправить данные , либо нет.
                {
                    throw new KafkaException(message.Error);
                }
                return message;
            }
            catch (Exception ex)
            {
                //_logger.LogError(
                //    new EventId(),
                //    ex,
                //    "Error producing to Kafka. Topic/partition: '{topic}/{partition}'. Message: {message}'.",
                //    topic,
                //    partition,
                //    message?.Value ?? "N/A");
                throw;
            }
        }

        #endregion


        #region Disposable

        public override void Dispose()
        {
           _producer.Dispose();
        }

        #endregion
    }
}