using System;
using System.Collections.Concurrent;
using MoreLinq;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;

namespace Shared.MiddleWares.Converters.StringConverters
{
    public class TriggerStringMemConverter : BaseStringConverter, IMemConverterMiddleWare, IDisposable
    {
        private readonly TriggerStringMemConverterOption _option;
        private readonly ConcurrentDictionary<int, TriggerStateImmutable> _stateDict = new ConcurrentDictionary<int, TriggerStateImmutable>();


        public TriggerStringMemConverter(TriggerStringMemConverterOption option)
        {
            _option = option;
        }


        protected override string ConvertChild(string inProp, int dataId)
        {
            TriggerStateImmutable SetState() => new TriggerStateImmutable(inProp, _option.String4Reset, _option.ResetTime);
            var state = _stateDict.GetOrAdd(dataId, SetState());
            if (!state.EqualStr(inProp))
            {
                state.Dispose();
                state = SetState();
                _stateDict[dataId] = state;
            }
            var strResult = state.GetState(inProp);
            return strResult;
        }

       // public bool IsEqualResetTime(int resetTime) => _option.ResetTime == resetTime;
       

        public void SendCommand(MemConverterCommand command)
        {
            if (command == MemConverterCommand.Reset)
            {
                _stateDict.Values.ForEach(state => state.RestartTrigger());
            }
        }


        private class TriggerStateImmutable : BaseState4MemConverter
        {
            private string StateBefore { get; set; }
            private string StateAfter { get; set; }

            public TriggerStateImmutable(string baseStr, string resetStr, int resetTime) : base(resetTime)
            {
                StateBefore = baseStr;
                StateAfter = resetStr;
            }

            public bool EqualStr(string str) => StateBefore.Equals(str);

            public string GetState(string newState) => TriggerDisabledOrIsFire ? StateAfter : StateBefore;
        }

        public void Dispose()
        {
            _stateDict.ForEach(s => s.Value.Dispose());
        }
    }
}