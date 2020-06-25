using Shared.MiddleWares.ConvertersOption.StringConvertersOption;

namespace Shared.MiddleWares.Converters.StringConverters
{
    /// <summary>
    /// Вернуть подстроку между индексами
    /// </summary>
    public class SubStringConverter : BaseStringConverter
    {
        private readonly SubStringConverterOption _option;

        public SubStringConverter(SubStringConverterOption option)
        {
            _option = option;
        }



        protected override string ConvertChild(string inProp, int dataId)
        {
            if (_option.StartIndex.HasValue && _option.StartIndex.Value > inProp.Length)
                return inProp;

            if (_option.EndIndex.HasValue && _option.EndIndex.Value > inProp.Length)
                return inProp;

            //ЗАДАНЫ StartIndex И EndIndex => ВЕРНУТЬ СТРОКУ МЕЖДУ НИМИМ
            if (_option.StartIndex.HasValue && _option.EndIndex.HasValue)
            {
                var lenght = _option.EndIndex.Value - _option.StartIndex.Value;
                return inProp.Substring(_option.StartIndex.Value, lenght);
            }

            //ЗАДАНЫ StartIndex => ВЕРНУТЬ СТРОКУ ОТ StartIndex ДО КОНЦА СТРОКИ
            if (_option.StartIndex.HasValue)
            {
                return inProp.Substring(_option.StartIndex.Value);
            }

            //ЗАДАНЫ EndIndex => ВЕРНУТЬ СТРОКУ ОТ НАЧАЛА СТРОКИ ДО EndIndex
            if (_option.EndIndex.HasValue)
            {
                return inProp.Substring(0, _option.EndIndex.Value);
            }

            return null;
        }
    }
}