

namespace Shared.MiddleWares.Converters.StringConverters
{
    public abstract class BaseStringConverter : IConverterMiddleWare<string>
    {
        public string Convert(string inProp, int dataId)
        {
            return inProp == null ? NullHandler() : ConvertChild(inProp, dataId);
        }

        /// <summary>
        /// Определяет само поведение конвертора.
        /// </summary>
        protected abstract string ConvertChild(string inProp, int dataId);

        /// <summary>
        /// Задает базовое поведение обработки null строки.
        /// Потомки могут переопределить это значение.
        /// </summary>
        protected virtual string NullHandler() => null;
    }
} 