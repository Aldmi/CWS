using System.Linq;
using System.Text;
using Shared.Helpers;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;


namespace Shared.MiddleWares.Converters.StringConverters
{
    /// <summary>
    /// Переводит строку в верхний регистр.
    /// </summary>
    public class ToUpperConverter : BaseStringConverter
    {
        private readonly ToUpperConverterOption _option;


        public ToUpperConverter(ToUpperConverterOption option)
        {
            _option = option;
        }


        protected override string ConvertChild(string inProp, int dataId)
        {
            var res = inProp.ToUpperInvariant();
            return res;
        }
    }
}