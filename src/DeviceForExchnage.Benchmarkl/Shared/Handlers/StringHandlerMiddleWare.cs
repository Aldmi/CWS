using DAL.Abstract.Entities.Options.MiddleWare.Handlers;
using DeviceForExchnage.Benchmark.Shared.Converters.StringConverters;

namespace DeviceForExchnage.Benchmark.Shared.Handlers
{
    /// <summary>
    /// Обработчик строковой переменной.
    /// Содержит имя переенной и цепочку обработчиков.
    /// </summary>
    public class StringHandlerMiddleWare : BaseHandlerMiddleWare<string>
    {
        #region ctor

        public StringHandlerMiddleWare(StringHandlerMiddleWareOption option)
        {
            PropName = option.PropName;

            if (option.InseartStringConverterOption != null)
            {
                Converters.Add(new InseartStringConverter(option.InseartStringConverterOption));
            }
            if (option.LimitStringConverterOption != null)
            {
               Converters.Add(new LimitStringComverter(option.LimitStringConverterOption));
            }
            if (option.ReplaceEmptyStringConverterOption != null)
            {
                Converters.Add(new ReplaceEmptyStringConverter(option.ReplaceEmptyStringConverterOption));
            }
        }

        #endregion
    }
}