using System;
using System.Linq;
using Domain.Device.MiddleWares.Converters.EnumsConverters;
using Domain.Device.Repository.Entities.MiddleWareOption.HandlersOption;

namespace Domain.Device.MiddleWares.Handlers
{
    /// <summary>
    /// Обработчик byte переменной.
    /// Содержит имя переменной и цепочку обработчиков.
    /// </summary>
    public class EnumHandlerMiddleWare : BaseHandlerMiddleWare<Enum>
    {
        #region ctor
        public EnumHandlerMiddleWare(EnumMiddleWareOption option)
        {
            PropName = option.PropName;
            Converters.AddRange(option.CreateConverters());
        }
        #endregion
    }
}