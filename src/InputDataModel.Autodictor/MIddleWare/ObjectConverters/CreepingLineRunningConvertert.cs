using System.Collections.Concurrent;
using Domain.InputDataModel.Autodictor.Entities;
using MoreLinq;
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

            ////Конвертор 1--------------------------------------------------------------
            //TriggerStringMemConverter CreateTriggerConverter() => new TriggerStringMemConverter(new TriggerStringMemConverterOption
            //{
            //    String4Reset = _option.String4Reset,
            //    ResetTime = resetTime
            //});
            //var triggConverter = _triggerStringDict.GetOrAdd(dataId, CreateTriggerConverter());
            //string resStep1;
            //if (!triggConverter.IsEqualResetTime(resetTime))   //Если Пришло новое значение таймера, нужно пересоздать TriggerConverter с новым значением таймера.т.к. TriggerStringMemConverter является иммутабельным.
            //{
            //    triggConverter.Dispose();
            //    var newTriggConverter = CreateTriggerConverter();
            //    _triggerStringDict[dataId] = newTriggConverter;
            //    resStep1 = newTriggConverter.Convert(str, dataId);
            //}
            //else
            //{
            //    resStep1 = triggConverter.Convert(str, dataId);
            //}
            ////Конвертор 2--------------------------------------------------------------
            //var resStep2 = _pagingConverter.Convert(resStep1, dataId);

            //cl.NameRu = resStep2;
            return cl;
        }

        /// <summary>
        /// Сигнал поступления НОВЫХ данных на вход. Именно список AdInput поменялся, при этом само входное значение конвертора inProp может не поменятсья,
        /// например поменялся только Id в данных, это значит новый пакет данных отправленн.
        /// </summary>
        public void SendCommand(MemConverterCommand command)
        {
            if (command == MemConverterCommand.Reset)
            {
                _triggerStringDict
                    .Values
                    .ForEach(t => t.SendCommand(command));

                _pagingConverter.SendCommand(command);
            }
        }
    }


    public class CreepingLineRunningConvertertOption
    {
        public string String4Reset { get; set; }        // Строка выставляется принудительно после сброса тригера
        public int Lenght { get; set; }                 // Длина подстроки которую нужно вернуть
        public char Separator { get; set; }             // Разделитель подстрок
    }
}