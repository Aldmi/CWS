using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MoreLinq;
using MoreLinq.Extensions;
using Serilog;
using Shared.Types;
using Worker.Background.Abstarct;

namespace Worker.Background.Concrete.HostingBackground
{
    public class HostingBackgroundTransport : HostingBackgroundBase, ITransportBackground
    {
        #region Field

        private readonly ConcurrentDictionary<Guid, Func<CancellationToken, Task>> _cycleTimeFuncDict = new ConcurrentDictionary<Guid, Func<CancellationToken, Task>>();
        private IEnumerator<KeyValuePair<Guid, Func<CancellationToken, Task>>> _enumeratorCycleTimeFuncDict;
        private readonly ConcurrentQueue<Func<CancellationToken, Task>> _oneTimeFuncQueue = new ConcurrentQueue<Func<CancellationToken, Task>>();
        private readonly ILogger _logger;

        private int _dutyCycleCounter;

        #endregion



        #region prop

        public KeyTransport KeyTransport { get; }
        public int DutyCycleTime { get; set; }       //Скважность (определяет время ожидания внутри одного периода циклического обмена)

        #endregion



        #region ctor

        public HostingBackgroundTransport(KeyTransport keyTransport, bool autoStart, int dutyCycleTime, ILogger logger) : base(autoStart)
        {
            KeyTransport = keyTransport;
            DutyCycleTime = dutyCycleTime;
            _logger = logger; 
            _enumeratorCycleTimeFuncDict = _cycleTimeFuncDict.GetEnumerator();       
        }

        #endregion




        #region Methode

        /// <summary>
        /// Добавление функций для циклического опроса
        /// </summary>
        public void AddCycleAction(Func<CancellationToken, Task> action)
        {
            if (action != null)
                _cycleTimeFuncDict.TryAdd(Guid.NewGuid(), action);
        }


        /// <summary>
        /// Удаление функции для циклического опроса
        /// </summary>
        public void RemoveCycleFunc(Func<CancellationToken, Task> action)
        {
            if (action != null)
            {
                var key = _cycleTimeFuncDict.FirstOrDefault(entry => entry.Value == action).Key;
                _cycleTimeFuncDict.TryRemove(key, out action);
            }
        }


        /// <summary>
        /// Добавление данных для одиночной функции запроса DataExchangeAsync
        /// </summary>
        public override void AddOneTimeAction(Func<CancellationToken, Task> action)
        {
            if (action != null)
                _oneTimeFuncQueue.Enqueue(action);
        }


        /// <summary>
        /// ПОКА ЕСТЬ ОДИНОЧНЫЕ ФУНКЦИИ, ОБРАБАТЫВАЮТСЯ ТОЛЬКО ОНИ.
        /// ЕСЛИ ОДИНОЧНЫХ ФУНКЦИЙ НЕТ ВЫПОЛНЯЮТСЯ ЦИКЛИЧЕСКИЕ ФУНКЦИИ.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
     
        protected override async Task ProcessAsync(CancellationToken stoppingToken)
        {
            //вызов одиночной функции запроса---------------------------------------------------------------
            if (_oneTimeFuncQueue != null && _oneTimeFuncQueue.Count > 0)
            {
                try
                {
                    while (_oneTimeFuncQueue.TryDequeue(out var oneTimeAction))
                    {
                        await oneTimeAction(stoppingToken);
                    }
                }
                catch(Exception ex)
                {
                    _logger.Fatal($"HostingBackgroundTransport.ProcessAsync Однократные функции {ex}");
                }
            }
            else
            {
                //вызов циклических функций--------------------------------------------------------------------
                try
                {
                    if (_cycleTimeFuncDict != null && !_cycleTimeFuncDict.IsEmpty)
                    {
                        if (_enumeratorCycleTimeFuncDict.MoveNext())
                        {
                            var cycleFunc = _enumeratorCycleTimeFuncDict.Current.Value;
                            await cycleFunc(stoppingToken);
                            if (++_dutyCycleCounter >= _cycleTimeFuncDict.Count)
                            {
                                _dutyCycleCounter = 0;
                                await Task.Delay(DutyCycleTime, stoppingToken);   //TODO: На время ожидангия блокируется разматывание _oneTimeFuncQueue (можно переделать на использовангие таймера, пока таймер считает цикл функции не разматываем, а однокртаные можно )
                            }
                        }
                        else
                        {
                            _enumeratorCycleTimeFuncDict.Dispose();
                            _enumeratorCycleTimeFuncDict = _cycleTimeFuncDict.GetEnumerator();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Fatal($"HostingBackgroundTransport.ProcessAsync Циклические функции {ex}");
                }
            }
            await Task.Delay(CheckUpdateTime, stoppingToken);
            //Console.WriteLine($"BackGroundMasterSp  {KeyTransport.Key}"); //DEBUG
        }

        #endregion
    }
}