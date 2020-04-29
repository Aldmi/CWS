﻿using System;
using CSharpFunctionalExtensions;
using Shared.Extensions;
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

        //TODO: что делать с блоком MATH

        #region ctor
        public StringInsertModelExt(string key, string format, BorderSubString borderSubString, StringHandlerMiddleWareOption stringHandlerMiddleWareOption)
        {
            Key = key;
            Format = format;
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
        /// ОПЦИИ цепочки конвеера обработки строки.
        /// </summary>
        public  StringHandlerMiddleWareOption StringHandlerMiddleWareOption { get; }
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


        ///// <summary>
        ///// Обрабатывает ValueType.
        ///// ValueType->string используя Format.
        ///// string->string (конвеер обработки заданный StringHandlerMiddleWareOption)
        ///// </summary>
        //public string CalcFinishValue(ValueType data)
        //{
        //    var formatVal = ConvertByFormat(data);
        //    return StartMiddleWarePipline(formatVal);
        //}


        /// <summary>
        /// Обрабатывает ValueType.
        /// ValueType->string используя Format.
        /// string->string (конвеер обработки заданный StringHandlerMiddleWareOption)
        /// </summary>
        public string CalcFinishValue<T>(T data)
        {
            var formatVal = ConvertByFormat(data);
            return StartMiddleWarePipline(formatVal);
        }


        /// <summary>
        /// Вернуть подстроку обозначенную границами BorderSubString
        /// </summary>
        public Result<string> CalcBorderSubString(string str )
        {
            var (_, isFailure, value, error) = str.SubstringBetweenCharacters(BorderSubString.StartCh, BorderSubString.EndCh, BorderSubString.IncludeBorder);
            return isFailure ? Result.Failure<string>(error) : Result.Ok(value);
        }


        ///// <summary>
        ///// ValueType->string используя Format.
        ///// Первоначальное преобразование ValueType к строке по формату.
        ///// </summary>
        //private string ConvertByFormat(ValueType data)
        //{
        //    return data switch
        //    {
        //        int intVal => intVal.Convert2StrByFormat(Format),//TODO: перенести в блок default
        //        DateTime dateTimeVal => dateTimeVal.Convert2StrByFormat(Format),
        //        //_ => data.Convert2StrByFormat(format) //Должно быть поведение по умолчанию
        //    };

        //   // throw new InvalidCastException("Тип переданного значнеия не соответсвует ни одному обработчику");
        //}



        private string ConvertByFormat<T>(T data)
        {
            return data switch
            {
                int intVal => intVal.Convert2StrByFormat(Format),//TODO: перенести в блок default
                DateTime dateTimeVal => dateTimeVal.Convert2StrByFormat(Format),
                byte[] byteArray => byteArray.BitConverter2StrByFormat(Format),
                //_ => data.Convert2StrByFormat(format) //Должно быть поведение по умолчанию
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