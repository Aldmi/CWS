using System;
using System.Collections.Concurrent;
using MoreLinq;
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

            var state = _stateDict.GetOrAdd(dataId, GetState());
            //установить новое состояние и сбросить таймер тригера для новой строки.
            var strResult = state.TrySetState(inProp);
            return strResult;
        }


        public bool IsEqualResetTime(int resetTime) => _option.ResetTime == resetTime;
  


        public void SendCommand(MemConverterCommand command)
        {
            if (command == MemConverterCommand.Reset)
            {
                _stateDict.Values.ForEach(state => state.RestartTrigger());
            }
        }


        private class TriggerState : State4MemConverterBase
        {
            private string StateBefore { get; set; }
            private string StateAfter { get; set; }


            public TriggerState(string baseStr, string resetStr, int resetTime) : base(resetTime)
            {
                StateBefore = baseStr;
                StateAfter = resetStr;
            }


            public string TrySetState(string newState)
            {
                bool EqualStateBefore(string str) => StateBefore.Equals(str);

                //ТРИГГЕР СРАБОТАЛ
                if (IsFired)
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
        }

        public void Dispose()
        {
            _stateDict.ForEach(s => s.Value.Dispose());
        }
    }
}