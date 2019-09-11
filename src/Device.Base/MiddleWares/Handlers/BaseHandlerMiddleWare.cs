using System.Collections.Generic;
using Domain.Device.MiddleWares.Converters;

namespace Domain.Device.MiddleWares.Handlers
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

        #endregion
    }
}