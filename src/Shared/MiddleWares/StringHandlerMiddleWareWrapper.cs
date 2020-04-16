using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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
            try
            {
                var option = JsonConvert.DeserializeObject<StringMiddleWareOption>(options[0]);
                _middleWare = new StringHandlerMiddleWare(option);
            }
            catch (Exception ex)
            {
                //TODO: вывод ошибки  в консоль убрать отсюда.
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Ошибка DeserializeObject StringMiddleWareOption для строки {options[0]}"); 
                Console.BackgroundColor = ConsoleColor.Black;
                throw;
            }
        }


        public string Convert(string str)
        {
            return _middleWare.Convert(str, 0);
        }
    }
}
