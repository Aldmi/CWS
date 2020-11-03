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

        /// <summary>
        /// Триггер сработал
        /// </summary>
        private bool IsFired { get; set; }

        /// <summary>
        /// триггер отключен, таймер не создан и не считает
        /// </summary>
        protected bool TriggerDisabled => _fireTime == 0;

        /// <summary>
        /// Триггер отключен или сработал.
        /// </summary>
        public bool TriggerDisabledOrIsFire => TriggerDisabled || IsFired;
        public bool TriggerEnabledAndIsFire => !TriggerDisabled && IsFired;


        protected BaseState4MemConverter(int fireTime)
        {
            _fireTime = fireTime;
            if(TriggerDisabled)
                return;

            _triggerTimer = new Timer();
            _triggerTimer.Elapsed += (sender, args) =>
            {
                IsFired = true;
            };
            RestartTrigger();
        }


        public void RestartTrigger()
        {
            if (TriggerDisabled)
                return;

            IsFired = false;
            _triggerTimer.Stop();
            _triggerTimer.Interval = _fireTime;
            _triggerTimer.Start();
        }


        public void Dispose()
        {
            if (TriggerDisabled)
                return;

            _triggerTimer.Stop();
            _triggerTimer.Dispose();
        }
    }
}