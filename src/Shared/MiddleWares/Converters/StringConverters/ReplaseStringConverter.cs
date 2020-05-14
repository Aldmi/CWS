using Shared.MiddleWares.ConvertersOption.StringConvertersOption;

namespace Shared.MiddleWares.Converters.StringConverters
{
    /// <summary>
    /// Замена строкового значения, по словарю Mapping.
    /// ключ значения по умолчанию "_";
    /// Сначала ищем значение по ключу. Если ключа нету, ищем дефолтный ключ "_", если его нету, возвращаем inProp.
    /// </summary>
    public class ReplaseStringConverter : BaseStringConverter
    {
        private readonly ReplaseStringConverterOption _option;

        public ReplaseStringConverter(ReplaseStringConverterOption option)
        {
            _option = option;
        }


        protected override string NullHandler()
        {
            return _option.Mapping.TryGetValue("_", out var defVal) ? defVal : null;
        }


        protected override string ConvertChild(string inProp, int dataId)
        {
            var key = _option.ToLowerInvariant ? inProp.ToLowerInvariant() : inProp;
            if (_option.Mapping.TryGetValue(key, out var val))
                return val;

            return _option.Mapping.TryGetValue("_", out var defaultVal) ? defaultVal : inProp;
        }
    }
}