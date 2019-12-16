using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.IndependentInseartsHandlers;
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
        private readonly Dictionary<string, StringInsertModel> _dict;          //Объектное преставление строки _baseString
        private readonly IIndependentInsertsHandler[] _independentInsertsHandler;   //Коллекция обработчиков для значений из _dict
        #endregion


        #region ctor
        /// <summary>
        /// Конструктор только для Default вставок
        /// </summary>
        private IndependentInsertsService(string baseString, string pattern, ILogger logger, params IIndependentInsertsHandler[] independentInsertsHandler) : this(baseString, pattern, logger)
        {
            _independentInsertsHandler = independentInsertsHandler;
        }

        private IndependentInsertsService(string baseString, string pattern, ILogger logger)
        {
            _baseString = baseString;
            _logger = logger;
            _dict = HelperStringFormatInseart.CreateInseartDict(baseString, pattern);
        }
        #endregion


        #region Methode
        public static IndependentInsertsService IndependentInsertsParserModelFactory(string str, string pattern, ILogger logger, IIndependentInsertsHandler inTypeHandler = null)
        {
            var independentInsertsHandlers = new List<IIndependentInsertsHandler>();
            if (inTypeHandler != null)
            {
                independentInsertsHandlers.Add(inTypeHandler);
            }

            if (Regex.Match(str, "{AddressDevice(.*)}").Success)
                independentInsertsHandlers.Add(new AddressDeviceIndependentInsertsHandler());

            if (str.Contains("rowNumber"))
                independentInsertsHandlers.Add(new RowNumberIndependentInsertsHandler());

            return new IndependentInsertsService(str, pattern, logger, independentInsertsHandlers.ToArray());
        }



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

            foreach (var (_, insert) in _dict)
            {
                foreach (var handler in _independentInsertsHandler)                 //Обрабатываем подстановку 1-ым валидным способом
                {
                    foreach (var inData in inDatas)
                    {
                        string replacementValue;
                        try
                        {
                            replacementValue = handler.CalcInserts(insert, inData);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex, "IndependentInsertsService.ExecuteInsearts Exception");
                            replacementValue = null;
                            // throw;
                        }
                        
                        if (replacementValue == null) continue;
                        sb.Replace(insert.Replacement, replacementValue);
                        inseartedDict.Add(insert.VarName, replacementValue);
                        goto LoopEnd;                                             //handler подошел для данных и вернул результат, надо переходить к другой паре (выход из 2-ого цикла)
                    }
                }
                LoopEnd:;
            }
            return (result: sb, inseartedDict: inseartedDict);
        }

        #endregion
    }
}