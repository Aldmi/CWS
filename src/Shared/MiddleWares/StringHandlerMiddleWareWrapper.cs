using System;
using System.Collections.Generic;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;
using Shared.MiddleWares.Handlers;
using Shared.MiddleWares.HandlersOption;

namespace Shared.MiddleWares
{
    /// <summary>
    /// Обертка для StringHandlerMiddleWare.
    /// На базе списка опций, создает конвеер обработчиков для StringHandlerMiddleWare.
    /// </summary>
    public class StringHandlerMiddleWareWrapper
    {
        private readonly StringHandlerMiddleWare _middleWare;

        public StringHandlerMiddleWareWrapper(IReadOnlyList<string> options)
        {
            var converters = new List<UnitStringConverterOption>();
            if (int.TryParse(options[0], out var lenght))
            {
                converters.Add(new UnitStringConverterOption
                {
                    PadRightStringConverterOption = new PadRightStringConverterOption { Lenght = lenght }
                });
            }
            else
            {
                //TODO: Этот Exception перекрываетвся Exception  при вызове   var owner= dataProviderFactory[_option.Provider.Name](_option.Provider); в Exchange.
                throw new ArgumentException("Опции для StringHandlerMiddleWareWrapper указанны не верно");
            }

            var option = new StringMiddleWareOption { Converters = converters };
            _middleWare = new StringHandlerMiddleWare(option);
        }


        public string Convert(string str)
        {
            return _middleWare.Convert(str, 0);
        }
    }
}
