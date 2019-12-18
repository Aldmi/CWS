using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Handlers;
using MoreLinq;
using Serilog;
using Shared.Helpers;

namespace Domain.InputDataModel.Base.InseartServices.IndependentInsearts
{
    public class IndependentInsertsService
    {
        #region field
        private readonly string _baseString;
        private readonly ILogger _logger;
        private readonly IIndependentInsertsHandler[] _independentInsertsHandler;   //Коллекция обработчиков для значений из _dict
        #endregion


        #region ctor
        public IndependentInsertsService(string baseString, ILogger logger, params IIndependentInsertsHandler[] independentInsertsHandler)
        {
            _independentInsertsHandler = independentInsertsHandler;
            _baseString = baseString;
            _logger = logger;
        }
        #endregion


        #region Methode
        //DEL
        //public static IndependentInsertsService IndependentInsertsParserModelFactory(string str, string pattern, ILogger logger, IIndependentInsertsHandler inTypeHandler = null)
        //{
        //    var independentInsertsHandlers = new List<IIndependentInsertsHandler>();
        //    if (inTypeHandler != null)
        //    {
        //        independentInsertsHandlers.Add(inTypeHandler);
        //    }

        //    if (Regex.Match(str, "{AddressDevice(.*)}").Success)
        //        independentInsertsHandlers.Add(new AddressDeviceIndependentInsertsHandler());

        //    if (str.Contains("rowNumber"))
        //        independentInsertsHandlers.Add(new RowNumberIndependentInsertsHandler());

        //    return new IndependentInsertsService(str, pattern, logger, independentInsertsHandlers.ToArray());
        //}



        /// <summary>
        ///  Для каждой выделенной {..} переменной из строки ищется обработчик (handler), которые сможеь найти замену этой переменной.
        /// </summary>
        /// <param name="inDatas">Данные для обработчика. Если тип данных подходит для обработчика, то обработчик выполняется с этими данными и ищет замену</param>
        /// <returns>result - результат вставки. inseartedDict - всатавленные данные</returns>
        public (StringBuilder result, Dictionary<string, string> inseartedDict) ExecuteInsearts(params object[] inDatas)
        {
            var sb = new StringBuilder(_baseString);
            var inseartedDict = new Dictionary<string, string>();

            if (_independentInsertsHandler == null || inDatas == null)
                return (result: sb, inseartedDict: inseartedDict);

            foreach (var handler in _independentInsertsHandler)                 //Обрабатываем подстановку 1-ым валидным способом
            {
                foreach (var inData in inDatas)
                {
                    var (_, isFailure, value, error) = handler.CalcInserts(inData);
                    if (isFailure)
                    {
                        _logger.Error( $"IndependentInsertsService.ExecuteInsearts Error= {error}");   //Ошибка вычисления значения подстановки
                        return (result: sb, inseartedDict: inseartedDict);
                    }

                    var (replacement, insertModel) = value;
                    if (replacement == null)      
                        continue;                                                                                  //inData НЕ подошла для handler 

                    sb.Replace(insertModel.Replacement, replacement);
                    inseartedDict.Add(insertModel.VarName, replacement);                                          //inData подошла для handler, handler вернул строку замены, выполнили замену в строке 
                }
            }
            return (result: sb, inseartedDict: inseartedDict);
        }

        //DEL
        //foreach (var (_, insert) in _dict)
        //{
        //    foreach (var handler in _independentInsertsHandler)                 //Обрабатываем подстановку 1-ым валидным способом
        //    {
        //        foreach (var inData in inDatas)
        //        {
        //            string replacementValue;
        //            try
        //            {
        //                replacementValue = handler.CalcInserts(insert, inData);
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.Error(ex, "IndependentInsertsService.ExecuteInsearts Exception");
        //                replacementValue = null;
        //                // throw;
        //            }
                        
        //            if (replacementValue == null) continue;
        //            sb.Replace(insert.Replacement, replacementValue);
        //            inseartedDict.Add(insert.VarName, replacementValue);
        //            goto LoopEnd;                                             //handler подошел для данных и вернул результат, надо переходить к другой паре (выход из 2-ого цикла)
        //        }
        //    }
        //    LoopEnd:;
        //}

        #endregion
    }
}