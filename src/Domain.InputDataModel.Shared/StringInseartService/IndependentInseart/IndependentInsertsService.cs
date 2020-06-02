using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers;
using Serilog;
using Shared.Collections;
using Shared.Types;

namespace Domain.InputDataModel.Shared.StringInseartService.IndependentInseart
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
        public (StringBuilder result, ReadOnlyDictionary<string, Change<string>> inseartedDict) ExecuteInsearts(params object[] inDatas)
        {
            var sb = new StringBuilder(_baseString);
            var emptyDict = new ReadOnlyDictionary<string, Change<string>>(new Dictionary<string, Change<string>>());
            
            if (_independentInsertsHandler == null || inDatas == null)
                return (result: sb, inseartedDict: emptyDict);

            var inseartedDict = new Dictionary<string, Change<string>>();
            foreach (var handler in _independentInsertsHandler)                                                     //Обрабатываем подстановку 1-ым валидным способом
            {
                foreach (var inData in inDatas)
                {
                    var (_, isFailure, value, error) = handler.CalcInserts(inData);
                    if (isFailure)
                    {
                        _logger.Error( $"IndependentInsertsService.ExecuteInsearts Error= {error}");    //Ошибка вычисления значения подстановки
                        return (result: sb, inseartedDict: emptyDict);
                    }

                    var (change, insertModel) = value;
                    if (change?.FinishVal == null)      
                        continue;                                                                                   //inData НЕ подошла для handler 

                    sb.Replace(insertModel.Replacement, change.FinishVal);
                    inseartedDict.TryAdd(insertModel.VarName, change);                                              //inData подошла для handler, handler вернул строку замены, выполнили замену в строке 
                }
            }
            return (result: sb, inseartedDict: new ReadOnlyDictionary<string, Change<string>>(inseartedDict)); 
        }
        #endregion
    }
}