using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption;
using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.StringConvertersOption;

namespace Domain.Device.MiddleWares.Converters.StringConverters
{
    /// <summary>
    /// Дополняет пробелами строку до длинны указанной в _option.Lenght.
    /// Если длинна строки больше или равна _option.Lenght, то дополнение не происходит.
    /// </summary>
    public class PadRightStringConverter : BaseStringConverter
    {
        private readonly PadRightStringConverterOption _option;
        public PadRightStringConverter(PadRightStringConverterOption option)
        {
            _option = option;
        }


        protected override string ConvertChild(string inProp, int dataId)
        {
            var res = inProp.PadRight(_option.Lenght);
            return res;
        }
    }
}