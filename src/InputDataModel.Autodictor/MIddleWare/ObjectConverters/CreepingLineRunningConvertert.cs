using System;
using System.Collections.Concurrent;
using Domain.InputDataModel.Autodictor.Entities;
using Shared.Extensions;
using Shared.MiddleWares.Converters;
using Shared.MiddleWares.Converters.StringConverters;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;

namespace Domain.InputDataModel.Autodictor.MIddleWare.ObjectConverters
{
    public class CreepingLineRunningConvertert : IConverterMiddleWare<object>, IMemConverterMiddleWare
    {
        private readonly CreepingLineRunningConvertertOption _option;
        private readonly ConcurrentDictionary<int, CreepingLineStateImmutable> _stateDict = new ConcurrentDictionary<int, CreepingLineStateImmutable>();


        public CreepingLineRunningConvertert(CreepingLineRunningConvertertOption option)
        {
            _option = option;
        }


        public object Convert(object inProp, int dataId)
        {
            var cl = (CreepingLine)inProp;
            CreepingLineStateImmutable SetState() => new CreepingLineStateImmutable(cl, _option.String4Reset, _option.Length, _option.Separator);
            var state = _stateDict.GetOrAddExt(dataId, SetState);
            if (!state.IsEqual(cl))
            {
                state.Dispose();
                state = SetState();
                _stateDict[dataId] = state;
            }
            var resState = state.GetState();
            return resState;
        }

        /// <summary>
        /// Сигнал поступления НОВЫХ данных на вход. Именно список AdInput поменялся, при этом само входное значение конвертора inProp может не поменятсья,
        /// например поменялся только Id в данных, это значит новый пакет данных отправленн.
        /// </summary>
        public void SendCommand(MemConverterCommand command)
        {
            if (command == MemConverterCommand.Reset)
            {
                //TODO: проверить сброс
                //_creepingLineStateDict
                //    .Values
                //    .ForEach(t => t.SendCommand(command));

                //_pagingConverter.SendCommand(command);
            }
        }

        private class CreepingLineStateImmutable : IDisposable
        {
            private CreepingLine BaseState { get; }
            private readonly TriggerStringMemConverter _trigConverter;
            private readonly SubStringMemConverter _pagingConverter;


            #region ctor
            public CreepingLineStateImmutable(CreepingLine cl, string string4Reset, int length, char separator)
            {
                BaseState = cl;
                var str = cl.NameRu;
                var duration = cl.Duration;
                var pagingCount = (int)Math.Ceiling((double)str.Length / length);
                var (resetTime, pagingTime) = CalcTimes(pagingCount, duration);

                _trigConverter = new TriggerStringMemConverter(new TriggerStringMemConverterOption
                {
                    ResetTime = resetTime,
                    String4Reset = string4Reset
                });
                _pagingConverter= new SubStringMemConverter(new SubStringMemConverterOption
                {
                    BanTime = pagingTime,
                    Separator = separator,
                    Lenght = length
                });
            }
            #endregion


            /// <summary>
            /// Вычислить времена для конверторов.
            /// duration - время для сброса строки задется напрямую в TriggerStringMemConverter
            /// pagingTime - время отображения порции данных, вычисляется из pagingCount
            /// 
            /// </summary>
            private static (int resetTime, int pagingTime) CalcTimes(int pagingCount, TimeSpan duration)
            {
                if (duration == TimeSpan.Zero)
                {
                    return (resetTime: 0, pagingTime: 0);
                }
                var resTime = (int)duration.TotalMilliseconds;
                var pagTime = resTime / pagingCount;
                return (resetTime: resTime, pagingTime: pagTime);
            }


            public bool IsEqual(CreepingLine cl) => BaseState.NameRu == cl.NameRu && BaseState.Duration == cl.Duration;


            public CreepingLine GetState() 
            {
                var resStep1 = _trigConverter.Convert(BaseState.NameRu, 1);
                var resStep2 = _pagingConverter.Convert(resStep1, 1);
                return new CreepingLine(resStep2, "", BaseState.Duration);
            }

            public void Dispose()
            {
                _trigConverter?.Dispose();
                _pagingConverter?.Dispose();
            }
        }
    }


    public class CreepingLineRunningConvertertOption
    {
        public string String4Reset { get; set; }        // Строка выставляется принудительно после сброса тригера
        public int Length { get; set; }                 // Длина подстроки которую нужно вернуть
        public char Separator { get; set; }             // Разделитель подстрок
    }
}