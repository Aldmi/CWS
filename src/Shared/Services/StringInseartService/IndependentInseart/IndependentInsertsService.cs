using System.Collections.Generic;
using System.Text;
using Serilog;

namespace Shared.Services.StringInseartService.IndependentInseart
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

            foreach (var handler in _independentInsertsHandler)                                                     //Обрабатываем подстановку 1-ым валидным способом
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
                        continue;                                                                                   //inData НЕ подошла для handler 

                    sb.Replace(insertModel.Replacement, replacement);
                    inseartedDict.Add(insertModel.VarName, replacement);                                            //inData подошла для handler, handler вернул строку замены, выполнили замену в строке 
                }
            }
            return (result: sb, inseartedDict: inseartedDict);
        }
        #endregion
    }
}