using System.Linq;
using System.Text;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;

namespace Shared.MiddleWares.Converters.StringConverters
{
    /// <summary>
    /// Замена символа  на строку
    /// Если символ не найден в словаре, то символ остается как есть.
    /// </summary>
    public class ReplaseCharStringConverter : BaseStringConverter
    {
        private readonly ReplaseCharStringConverterOption _option;


        public ReplaseCharStringConverter(ReplaseCharStringConverterOption option)
        {
            _option = option;
        }


        protected override string ConvertChild(string inProp, int dataId)
        {
            var sb = new StringBuilder();
            foreach (var c in inProp.ToCharArray())
            {
                var key = _option.ToLowerInvariant ? char.ToLowerInvariant(c) : c;
                if (_option.Mapping.TryGetValue(key, out var val))
                {
                    sb.Append(val);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}