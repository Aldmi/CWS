using System;
using Shared.MiddleWares.Handlers;
using Shared.MiddleWares.HandlersOption;
using Shared.Types;

namespace Domain.InputDataModel.Shared.StringInseartService.Model
{
    public class StringInsertModelExt : IDisposable
    {
        #region fields
        public readonly StringMiddleWareOption StringMiddleWareOption;
        #endregion


        #region ctor
        public StringInsertModelExt(string key, string format, BorderSubString borderSubString, StringMiddleWareOption stringMiddleWareOption)
        {
            Key = key;
            Format = format;
            BorderSubString = borderSubString;
            StringMiddleWareOption = stringMiddleWareOption;
            LazyStringMiddleWare = new Lazy<StringHandlerMiddleWare>(() =>
            {
                if (StringMiddleWareOption == null) return null;
                var middleWare = new StringHandlerMiddleWare(StringMiddleWareOption);
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
        /// Цепочка обработчиков строкового значения
        /// </summary>
        public Lazy<StringHandlerMiddleWare> LazyStringMiddleWare { get; }

        /// <summary>
        /// НЕ ОБЯЗАТЕЛЕН.
        /// Задает границы для вычленения подстроки.
        /// </summary>
        public BorderSubString BorderSubString { get; }
        #endregion


        #region Methode

        #endregion


        #region Disposable
        public void Dispose()
        {
        }
        #endregion
    }
}