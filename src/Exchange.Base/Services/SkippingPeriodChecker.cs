using System;
using System.Timers;

namespace Domain.Exchange.Services
{
    /// <summary>
    /// Проверка Пропуска по таймеру.
    /// Вызов StartSkippingPeriod() запускает таймер, в течении времени которого IsSkip = true.
    /// По истечении времени таймера IsSkip = false (И ТАЙМЕР АВТОМАТОМ НЕ ПЕРЕЗАПУСКАКТСЯ)
    /// Для повторного запуска таймера, нужно вызвать StartSkippingPeriod().
    /// Чтобы принудительно остановить ьаймер и прервать режим пропуска (IsSkip = false), нужно вызвать StopSkippingPeriod()
    /// </summary>
    public class SkippingPeriodChecker : IDisposable
    {
        #region fields

        private readonly Timer _timerSkiper;
        private readonly int _skipInterval;   //временной интервал в течении которого работает режим пропуска IsSkip = true

        #endregion



        #region ctor

        public SkippingPeriodChecker(int skipInterval)
        {
            _skipInterval = skipInterval;
            if (_skipInterval > 0)
            {
                _timerSkiper = new Timer { AutoReset = false };
                _timerSkiper.Elapsed += TimerSkipElapsed;
            }
            IsSkip = false;
        }

        #endregion



        #region prop

        /// <summary>
        /// Флаг пропуска.
        /// </summary>
        public bool IsSkip { get; private set; }

        #endregion



        #region EventHandlers

        private void TimerSkipElapsed(object sender, ElapsedEventArgs e)
        {
            IsSkip = false;
        }

        #endregion



        #region Mehods

        public void StartSkipping()
        {
            if (_skipInterval > 0)
            {
                IsSkip = true;
                _timerSkiper.Stop();
                _timerSkiper.Interval = _skipInterval;
                _timerSkiper.Start();
            }
        }

        public void StopSkipping()
        {
            if (_skipInterval > 0)
            {
                IsSkip = false;
                _timerSkiper.Stop();
            }
        }

        #endregion



        #region Disposable

        public void Dispose()
        {
            _timerSkiper?.Dispose();
        }

        #endregion
    }
}