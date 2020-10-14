using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using System.Timers;
using MoreLinq;
using Shared.MiddleWares.Converters.Exceptions;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;

namespace Shared.MiddleWares.Converters.StringConverters
{
    public class TriggerStringMemConverter : BaseStringConverter, IMemConverterMiddleWare, IDisposable
    {
        private readonly TriggerStringMemConverterOption _option;
        private readonly ConcurrentDictionary<int, TriggerState> _stateDict = new ConcurrentDictionary<int, TriggerState>();


        public TriggerStringMemConverter(TriggerStringMemConverterOption option)
        {
            _option = option;
        }

        protected override string ConvertChild(string inProp, int dataId)
        {
            TriggerState GetState() => new TriggerState(inProp, _option.String4Reset, _option.ResetTime);

            //Если данных нет в словаре. Добавить в словарь новые данные.
            if (!_stateDict.ContainsKey(dataId))
            {
                _stateDict.TryAdd(dataId, GetState());
            }

            //Попытка установить новое состояние и сбросить таймер тригерра для новой строки.
            if (_stateDict.TryGetValue(dataId, out var triggerState))
            {
               var state = triggerState.TrySetState(inProp);
               return state;
            }

            throw new StringConverterException($"TriggerStringMemConverter НЕ СМОГ ИЗВЛЕЧЬ РЕЗУЛЬТАТ ОБРАБОТКИ ИЗ СЛОВАРЯ {inProp}");
        }


        public bool IsEqualResetTime(int resetTime) => _option.ResetTime == resetTime;
  


        public void SendCommand(MemConverterCommand command)
        {
            if (command == MemConverterCommand.Reset)
            {
                _stateDict.Values.ForEach(state => state.RestartTrigger());
            }
        }


        private class TriggerState : IDisposable
        {
            private readonly int _resetTime;
            private readonly Timer _timerInvoke;
            private bool _isFired;
            private string StateBefore { get; set; }
            private string StateAfter { get; set; }


            public TriggerState(string baseStr, string resetStr, int resetTime)
            {
                StateBefore = baseStr;
                _resetTime = resetTime;
                _timerInvoke = new Timer();
                _timerInvoke.Elapsed += (sender, args) =>
                {
                    _isFired = true;
                    StateAfter = resetStr;
                };
                RestartTrigger();
            }


            public string TrySetState(string newState)
            {
                //ТРИГГЕР СРАБОТАЛ
                if (_isFired)
                {
                    //данные старые
                    if (EqualStateBefore(newState))
                    {
                        return StateAfter;
                    }
                    //данные новые
                    StateBefore = newState;
                    RestartTrigger();
                    return StateBefore;
                }

                // ТРИГГЕР НЕ СРАБОТАЛ
                //данные старые
                if (EqualStateBefore(newState))
                {
                    return StateBefore;
                }
                //данные новые
                StateBefore = newState;
                RestartTrigger();
                return StateBefore;
            }


            private bool EqualStateBefore(string str)=> StateBefore.Equals(str);


            public void RestartTrigger()
            {
                _isFired = false;
                _timerInvoke.Stop();
                _timerInvoke.Interval = _resetTime;
                _timerInvoke.Start();
            }


            public void Dispose()
            {
                _timerInvoke.Stop();
                _timerInvoke.Dispose();
            }
        }


        public void Dispose()
        {
            _stateDict.ForEach(s => s.Value.Dispose());
        }
    }
}