using System;
using System.Collections.Concurrent;
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

        private readonly ConcurrentDictionary<int, Func<CancellationToken, Task>> _cycleTimeFuncDict = new ConcurrentDictionary<int, Func<CancellationToken, Task>>();
        private readonly ConcurrentQueue<Func<CancellationToken, Task>> _oneTimeFuncQueue = new ConcurrentQueue<Func<CancellationToken, Task>>();
        private readonly ILogger _logger;
        #endregion



        #region prop

        public KeyTransport KeyTransport { get; }

        #endregion



        #region ctor

        public HostingBackgroundTransport(KeyTransport keyTransport, bool autoStart, ILogger logger) : base(autoStart)
        {
            _logger = logger;
            KeyTransport = keyTransport;
        }

        #endregion




        #region Methode

        /// <summary>
        /// Добавление функций для циклического опроса
        /// </summary>
        public void AddCycleAction(Func<CancellationToken, Task> action)
        {
            if (action != null)
                _cycleTimeFuncDict.TryAdd(_cycleTimeFuncDict.Count, action);
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
        private int indexCycleFunc = 0;
        protected override async Task ProcessAsync(CancellationToken stoppingToken)
        {
            //вызов одиночной функции запроса---------------------------------------------------------------
            if (_oneTimeFuncQueue != null && _oneTimeFuncQueue.Count > 0)
            {
                while (_oneTimeFuncQueue.TryDequeue(out var oneTimeaction))
                {
                    await oneTimeaction(stoppingToken);
                }
            }
            else
            {
                //вызов циклических функций--------------------------------------------------------------------
                try//DEBUG
                {
                    if (_cycleTimeFuncDict != null && !_cycleTimeFuncDict.IsEmpty)
                    {
                        //if (indexCycleFunc > _cycleTimeFuncDict.Count - 1)
                        //    indexCycleFunc = 0;

                        if (_cycleTimeFuncDict.ContainsKey(indexCycleFunc))
                        {
                            await _cycleTimeFuncDict[indexCycleFunc](stoppingToken);
                            indexCycleFunc++;
                        }
                        else
                        {
                            indexCycleFunc = _cycleTimeFuncDict.FirstOrDefault().Key; //TODO: не оптимальное перечисление элементов словаря 
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Fatal($"HostingBackgroundTransport.ProcessAsync {ex}");
                }
            }

            await Task.Delay(CheckUpdateTime, stoppingToken);
            //Console.WriteLine($"BackGroundMasterSp  {KeyTransport.Key}"); //DEBUG
        }

        #endregion
    }
}