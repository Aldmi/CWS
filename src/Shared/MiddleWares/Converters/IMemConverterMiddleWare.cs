using System;
using System.Timers;

namespace Shared.MiddleWares.Converters
{
    public interface IMemConverterMiddleWare
    {
        void SendCommand(MemConverterCommand command);
    }

    public enum MemConverterCommand { None, Reset }



    /// <summary>
    /// Тайменр отсчитывает время сработки триггера.
    /// Сброс тригера и перезапуск таймера делается вручную.
    /// </summary>
    public abstract class BaseState4MemConverter : IDisposable
    {
        private readonly int _fireTime;
        private readonly Timer _triggerTimer;
        public bool IsFired { get; private set; }


        protected BaseState4MemConverter(int fireTime)
        {
            _fireTime = fireTime;
            _triggerTimer = new Timer();
            _triggerTimer.Elapsed += (sender, args) =>
            {
                IsFired = true;
            };
            RestartTrigger();
        }


        public void RestartTrigger()
        {
            IsFired = false;
            _triggerTimer.Stop();
            _triggerTimer.Interval = _fireTime;
            _triggerTimer.Start();
        }


        public void Dispose()
        {
            _triggerTimer.Stop();
            _triggerTimer.Dispose();
        }
    }
}