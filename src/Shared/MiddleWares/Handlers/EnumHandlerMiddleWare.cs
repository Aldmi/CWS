using System;
using Shared.MiddleWares.HandlersOption;

namespace Shared.MiddleWares.Handlers
{
    /// <summary>
    /// Обработчик byte переменной.
    /// Содержит имя переменной и цепочку обработчиков.
    /// </summary>
    public class EnumHandlerMiddleWare : BaseHandlerMiddleWare<Enum>
    {
        #region ctor
        public EnumHandlerMiddleWare(EnumHandlerMiddleWareOption option)
        {
            Converters.AddRange(option.CreateConverters());
        }
        #endregion
    }
}