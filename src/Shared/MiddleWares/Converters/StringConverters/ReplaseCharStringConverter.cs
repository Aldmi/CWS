﻿using System.Runtime.InteropServices;
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
        private const char DefaultChar = '\u0002';


        public ReplaseCharStringConverter(ReplaseCharStringConverterOption option)
        {
            _option = option;
        }


        /// <summary>
        /// null- заменяю на " ", и преобразую _option.Mapping.
        /// </summary>
        protected override string NullHandler()
        {
            return ConvertChild(" ", 1);
        }


        protected override string ConvertChild(string inProp, int dataId)
        {
            inProp = (inProp == string.Empty) ? " " : inProp; 
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
                    var defaultVal = _option.Mapping.TryGetValue(DefaultChar, out var def) ? def : c.ToString();
                    sb.Append(defaultVal);
                }
            }
            return sb.ToString();
        }
    }
}