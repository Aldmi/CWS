using System;
using Newtonsoft.Json;
using Shared.MiddleWares.Handlers;
using Shared.MiddleWares.HandlersOption;

namespace Shared.MiddleWares
{
    /// <summary>
    /// StringMiddleWareOption - десериализуются из строки
    /// </summary>
    public static class StringHandlerMiddleWareFactoryFromStrOptions
    {
        public static StringHandlerMiddleWare Create(string optionStr)
        {
            try
            {
                var option = JsonConvert.DeserializeObject<StringMiddleWareOption>(optionStr);
               var middleWare = new StringHandlerMiddleWare(option);
                return middleWare;
            }
            catch (Exception)
            {
                //TODO: вывод ошибки  в консоль убрать отсюда.
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Ошибка DeserializeObject StringMiddleWareOption для строки {optionStr}"); 
                Console.BackgroundColor = ConsoleColor.Black;
                throw;
            }
        }
    }
}
