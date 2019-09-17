using System;
using System.Reactive.Subjects;
using System.Timers;
using Domain.Exchange.Enums;
using Domain.Exchange.RxModel;
using Timer = System.Timers.Timer;

namespace Domain.Exchange.Services
{
    /// <summary>
    /// Сервис проверки входных данных.
    /// Если за время checkInterval не будет вызванн метод InputDataEntry(), то InputDataState = ToLongNoEntry.
    /// Вызов метода InputDataEntry() сбрасывает таймер.
    /// </summary>
    public class InputCycleDataEntryCheker : IDisposable
    {
        #region fields

        private readonly string _key;
        private readonly int _checkInterval;
        private readonly Timer _timerInputCycleDataCheck;

        #endregion



        #region prop

        /// <summary>
        /// Состояние получения входных данных
        /// true - нормальное состояние
        /// </summary>
        public InputDataStatus InputDataStatus { get; private set; }

        #endregion



        #region InputDataRx

        /// <summary>
        /// Событие смены состояния получения данных
        /// </summary>
        public ISubject<InputDataStateRxModel> CycleDataEntryStateChangeRx { get; } = new Subject<InputDataStateRxModel>();

        #endregion



        #region ctor

        public InputCycleDataEntryCheker(string key, int checkInterval)
        {
            _key = key;
            _checkInterval = checkInterval;
            _timerInputCycleDataCheck = new Timer();
            _timerInputCycleDataCheck.Elapsed += TimerToLongNoEntryElapsed;
            InputDataStatus = InputDataStatus.NormalEntry;
        }

        #endregion



        #region EventHandlers

        private void TimerToLongNoEntryElapsed(object sender, ElapsedEventArgs e)
        {
            //ПЕРЕХОД В СОСТОЯНИЕ ДОЛГОГО ОТСУТСВИЯ ДАННЫХ.
            if (InputDataStatus == InputDataStatus.NormalEntry)
            {
                InputDataStatus = InputDataStatus.ToLongNoEntry;
                CycleDataEntryStateChangeRx.OnNext(new InputDataStateRxModel(_key, InputDataStatus));
            }
        }

        #endregion



        #region Mehods

        public void StartChecking()
        {
            if (_checkInterval > 0)
            {
                _timerInputCycleDataCheck.Interval = _checkInterval;
                _timerInputCycleDataCheck.Start();
            }
        }


        public void StopChecking()
        {
            _timerInputCycleDataCheck.Stop();
        }


        /// <summary>
        /// Входные данные пришли.
        /// СБРОС ТАЙМЕРА.
        /// ПЕРЕХОД В СОСТОЯНИЕ ПОЛУЧЕНИЯ ДАННЫХ.
        /// </summary>
        public void InputDataEntry()
        {
            if (_checkInterval <= 0)
               return;

            _timerInputCycleDataCheck.Interval = _checkInterval;
            if (InputDataStatus == InputDataStatus.ToLongNoEntry)
            {
                InputDataStatus = InputDataStatus.NormalEntry;
                CycleDataEntryStateChangeRx.OnNext(new InputDataStateRxModel(_key, InputDataStatus));
            }
        }

        #endregion



        #region Disposable

        public void Dispose()
        {
            _timerInputCycleDataCheck?.Dispose();
        }

        #endregion
    }
}