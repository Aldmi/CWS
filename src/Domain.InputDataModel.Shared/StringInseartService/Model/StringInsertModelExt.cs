using System;
using Shared.Extensions;
using Shared.Mathematic;
using Shared.MiddleWares.Handlers;
using Shared.MiddleWares.HandlersOption;
using Shared.Types;

namespace Domain.InputDataModel.Shared.StringInseartService.Model
{
    /// <summary>
    /// Расширение для StringInsertModel.
    /// Задает формат начального преобразования к строке используя format, для переменной типа ValueType.
    /// Потом, дальнейшее преобразование строки в конвеере StringMiddleWare.
    /// </summary>
    public class StringInsertModelExt : IDisposable
    {
        #region fields
        /// <summary>
        /// Цепочка обработчиков строкового значения
        /// </summary>
        private readonly Lazy<StringHandlerMiddleWare> _lazyStringMiddleWare;
        #endregion


        #region ctor
        public StringInsertModelExt(string key, string format, BorderSubString borderSubString, StringHandlerMiddleWareOption stringHandlerMiddleWareOption)
        {
            Key = key;
            Format = format ?? String.Empty;
            BorderSubString = borderSubString;
            StringHandlerMiddleWareOption = stringHandlerMiddleWareOption;
            _lazyStringMiddleWare = new Lazy<StringHandlerMiddleWare>(() =>
            {
                if (StringHandlerMiddleWareOption == null) return null;
                var middleWare = new StringHandlerMiddleWare(StringHandlerMiddleWareOption);
                return middleWare;
            });
        }
        #endregion



        #region prop
        /// <summary>
        /// Идентификатор для связи с переменной StringInsertModel Ключ в БД.
        /// </summary>
        public string Key { get; private set; }    //private set нужен для AutoMapper

        /// <summary>
        ///  Формат преобразования переменной к string из базового ValueType (int, DateTime, ...)
        /// </summary>
        public string Format { get; }

        /// <summary>
        /// НЕ ОБЯЗАТЕЛЕН.
        /// Задает границы для вычленения подстроки.
        /// </summary>
        public BorderSubString BorderSubString { get; }


        /// <summary>
        /// НЕ ОБЯЗАТЕЛЕН.
        /// Задает границы для вычленения подстроки.
        /// </summary>
        public MathematicService MathematicService { get; }

        /// <summary>
        /// НЕ ОБЯЗАТЕЛЕН.
        /// ОПЦИИ цепочки конвеера обработки строки.
        /// </summary>
        public StringHandlerMiddleWareOption StringHandlerMiddleWareOption { get; }
        #endregion



        #region Methode
        /// <summary>
        /// Обрабатывает string.
        /// Переменная Format не используется.
        /// string->string (конвеер обработки заданный StringHandlerMiddleWareOption)
        /// </summary>
        public string CalcFinishValue(string data)
        {
            return StartMiddleWarePipline(data);
        }


        /// <summary>
        /// Обрабатывает ValueType.
        /// ValueType->string используя Format.
        /// string->string (конвеер обработки заданный StringHandlerMiddleWareOption)
        /// </summary>
        public string CalcFinishValue<T>(T data)
        {
            var afterMath = ApplyMath(data);
            var formatVal = ConvertByFormat(data);
            return StartMiddleWarePipline(formatVal);
        }


        /// <summary>
        /// Выполняет математическую обработку над входной переменной.
        /// </summary>
        private T ApplyMath<T>(T data)
        {
            return data switch
            {
                //int intVal => intVal + 10,
                //DateTime dateTimeVal => dateTimeVal.AddMinutes(1.0),
                _ => data
            };

        }



        private string ConvertByFormat<T>(T data) 
        {
            return data switch
            {
                int intVal => intVal.Convert2StrByFormat(Format),
                DateTime dateTimeVal => dateTimeVal.Convert2StrByFormat(Format),
                byte[] byteArray => byteArray.BitConverter2StrByFormat(Format),
                Enum e => e.ToString(), 
                _ => data.ToString()
            };
            // throw new InvalidCastException("Тип переданного значнеия не соответсвует ни одному обработчику");
        }

        /// <summary>
        ///  string->string  Конечный конвеер обработки
        /// </summary>
        private string StartMiddleWarePipline(string data)
        {
            var mw = _lazyStringMiddleWare.Value;
            return (mw == null) ? data: mw.Convert(data, 0);
        }

        #endregion



        #region Disposable
        public void Dispose()
        {
        }
        #endregion
    }
}