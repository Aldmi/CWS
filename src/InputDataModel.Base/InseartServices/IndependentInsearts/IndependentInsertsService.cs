using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.IndependentInseartsHandlers;
using MoreLinq;

namespace Domain.InputDataModel.Base.InseartServices.IndependentInsearts
{
    public class IndependentInsertsService
    {
        #region field
        private readonly string _baseString; 
        private readonly Dictionary<string, IndependentInsertModel> _dict;          //Объектное преставление строки _baseString
        private readonly IIndependentInsertsHandler[] _independentInsertsHandler;   //Коллекция обработчиков для значений из _dict
        #endregion


        #region ctor
        /// <summary>
        /// Конструктор только для Default вставок
        /// </summary>
        public IndependentInsertsService(string baseString,
            string pattern, 
           params IIndependentInsertsHandler[] independentInsertsHandler) : this(baseString, pattern)
        {
            _independentInsertsHandler = independentInsertsHandler;
        }

        private IndependentInsertsService(string baseString, string pattern)
        {
            _baseString = baseString;
            _dict = ConvertString2IndependentInseartCollection(baseString, pattern)
                .DistinctBy(insert => insert.Replacement)
                .ToDictionary(insert => insert.VarName, insert => insert);
        }
        #endregion


        #region Methode
        public static IndependentInsertsService IndependentInsertsParserModelFactory(string str, string pattern, IIndependentInsertsHandler inTypeHandler = null)
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

           

            return new IndependentInsertsService(str, pattern, independentInsertsHandlers.ToArray());
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
                        var replacementValue = handler.CalcInserts(insert, inData);
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


        private static IEnumerable<IndependentInsertModel> ConvertString2IndependentInseartCollection(string str, string pattern)
        {
            //var pattern = @"\{(.*?)(:.+?)?\}";
            var matches = Regex.Matches(str, pattern)
                .Select(match => new IndependentInsertModel(match.Groups[0].Value, match.Groups[1].Value, match.Groups[2].Value));

            return matches;
        }
        #endregion
    }
}