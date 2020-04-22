using Shared.MiddleWares.HandlersOption;

namespace Shared.MiddleWares.Handlers
{
    /// <summary>
    /// Обработчик строковой переменной.
    /// Содержит имя переменной и цепочку обработчиков.
    /// </summary>
    public class StringHandlerMiddleWare : BaseHandlerMiddleWare<string>
    {
        #region ctor
        public StringHandlerMiddleWare(StringHandlerMiddleWareOption option)
        {
            Converters.AddRange(option.CreateConverters());
        }
        #endregion
    }
}