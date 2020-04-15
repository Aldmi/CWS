using System.Collections.Generic;
using System.Linq;
using MoreLinq.Extensions;
using Shared.MiddleWares.Converters;

namespace Shared.MiddleWares.Handlers
{
    public abstract class BaseHandlerMiddleWare<T>
    {
        public string PropName { get; protected set; }
        protected readonly List<IConverterMiddleWare<T>> Converters = new List<IConverterMiddleWare<T>>();


        #region Methode
        /// <summary>
        /// Вызываемый код по имени PropName находит свойство во входном типе и передает его сюда
        /// </summary>
        public virtual T Convert(T inProp, int dataId)
        {
            foreach (var converter in Converters)
            {
                inProp = converter.Convert(inProp, dataId);
            }
            return inProp;
        }

        /// <summary>
        /// Команда Сброс State в IMemConverterMiddleWare.
        /// </summary>
        /// <param name="command"></param>
        public void SendCommand4MemConverters(MemConverterCommand command)
        { 
            Converters
                .Where(c=>c is IMemConverterMiddleWare)
                .Cast<IMemConverterMiddleWare>()
                .ForEach(c=>c.SendCommand(command));
        }
        #endregion
    }
}