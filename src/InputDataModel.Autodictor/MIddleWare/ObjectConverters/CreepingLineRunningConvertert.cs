using System;
using System.Collections.Concurrent;
using Domain.InputDataModel.Autodictor.Entities;
using Shared.MiddleWares.Converters;
using Shared.MiddleWares.Converters.StringConverters;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;

namespace Domain.InputDataModel.Autodictor.MIddleWare.ObjectConverters
{
    public class CreepingLineRunningConvertert : IConverterMiddleWare<object>, IMemConverterMiddleWare
    {
        private readonly CreepingLineRunningConvertertOption _option;
        private readonly SubStringMemConverter _pagingConverter;
        private readonly ConcurrentDictionary<int, TriggerStringMemConverter> _triggerStringDict = new ConcurrentDictionary<int, TriggerStringMemConverter>();


        public CreepingLineRunningConvertert(CreepingLineRunningConvertertOption option)
        {
            _option = option;

            _pagingConverter = new SubStringMemConverter(new SubStringMemConverterOption
            {
                Lenght = _option.Lenght,
                Separator = _option.Separator
            });
        }


        public object Convert(object inProp, int dataId)
        {
            //TODO: чтобы постоянн не приводить тип, можно кешировать inProp, и заменять толко при новом значении
            var cl = (CreepingLine)inProp;
            var str = cl.NameRu;
            var resetTime = (int)cl.WorkTime.TotalMilliseconds;


            //Конвертор 1--------------------------------------------------------------
            TriggerStringMemConverter CreateTriggerConverter() => new TriggerStringMemConverter(new TriggerStringMemConverterOption
            {
                String4Reset = _option.String4Reset,
                ResetTime = resetTime
            });

            //Если данных нет в словаре. Добавить в словарь новые данные.
            if (!_triggerStringDict.ContainsKey(dataId))
            {
                _triggerStringDict.TryAdd(dataId, CreateTriggerConverter());
            }

            //Вызовем Convert на конверторе
            string resStep1 = null;
            if (_triggerStringDict.TryGetValue(dataId, out var triggConverter))
            {
                //Пришло новое значение таймера, нужно пересоздать TriggerConverter с новым значением таймера.
                if (!triggConverter.IsEqualResetTime(resetTime))
                {
                    triggConverter.Dispose();
                    var newConverter = CreateTriggerConverter();
                    _triggerStringDict[dataId] = newConverter;
                    resStep1 = newConverter.Convert(cl.NameRu, dataId);
                }
                else
                {
                    resStep1 = triggConverter.Convert(cl.NameRu, dataId);
                }
            }


            //Конвертор 2--------------------------------------------------------------
            //var resStep2 = _pagingConverter.Convert(resStep1, dataId);

            cl.NameRu = resStep1; //resStep2
            return cl;
        }

        public void SendCommand(MemConverterCommand command)
        {
            //NotImplemented
        }
    }


    public class CreepingLineRunningConvertertOption
    {
        public string String4Reset { get; set; }        // Строка выставляется принудительно после сброса тригера
        public int Lenght { get; set; }                 // Длина подстроки которую нужно вернуть
        public char Separator { get; set; }             // Разделитель подстрок
    }
}