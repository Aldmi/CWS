using System.Collections.Generic;
using System.Linq;
using Domain.Device.MiddleWares.Converters.StringConverters;
using Domain.Device.Repository.Entities.MiddleWareOption.HandlersOption;

namespace Domain.Device.MiddleWares.Handlers
{
    /// <summary>
    /// Обработчик строковой переменной.
    /// Содержит имя переменной и цепочку обработчиков.
    /// </summary>
    public class StringHandlerMiddleWare : BaseHandlerMiddleWare<string>
    {
        #region ctor

        public StringHandlerMiddleWare(StringMiddleWareOption option)
        {
            PropName = option.PropName;
            foreach (var c in option.Converters)
            {
                Converters.Add(c.CreateStringConverter());
            }
        }
        #endregion
    }
}