using System.Collections.Generic;
using DeviceForExchnage.Benchmark.Shared.Converters;

namespace DeviceForExchnage.Benchmark.Shared.Handlers
{
    public abstract class BaseHandlerMiddleWare<T>
    {
        public string PropName { get; protected set; }
        protected readonly List<IConverterMiddleWare<T>> Converters = new List<IConverterMiddleWare<T>>();



        #region Methode


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inProp"></param>
        /// <returns></returns>
        public virtual T Convert(T inProp)
        {
            //Вызываемый код по имени PropName находит свойство во входном типе и передает его сюда
            foreach (var converter in Converters)
            {
                inProp = converter.Convert(inProp);
            }
            return inProp;
        }

        #endregion
    }
}