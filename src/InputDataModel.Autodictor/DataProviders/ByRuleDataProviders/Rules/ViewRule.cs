using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DAL.Abstract.Entities.Options.Exchange.ProvidersOption;
using InputDataModel.Autodictor.Entities;
using InputDataModel.Autodictor.Model;
using Serilog;
using Shared.CrcCalculate;
using Shared.Extensions;
using Shared.Helpers;

namespace InputDataModel.Autodictor.DataProviders.ByRuleDataProviders.Rules
{
    /// <summary>
    /// Правило отображения порции даных
    /// STX - \u0002
    /// RTX - \u0003
    /// </summary>
    public class ViewRule
    {
        #region fields

        private readonly string _addressDevice;
        private ViewRuleOption _option;
        private readonly ILogger _logger;

        #endregion



        #region ctor

        public ViewRule(string addressDevice, ViewRuleOption option, ILogger logger)
        {
            _addressDevice = addressDevice;
            _option = option;
            _logger = logger;
        }

        #endregion



        #region MutabeleOptions

        public ViewRuleOption GetCurrentOption()
        {
            return _option;
        }


        public void SetCurrentOption(ViewRuleOption viewRuleOption)
        {
            _option = viewRuleOption;
        }

        #endregion



        #region Methode

        /// <summary>
        /// Создать строку запроса ПОД ДАННЫЕ, подставив в форматную строку запроса значения переменных из списка items.
        /// </summary>
        /// <param name="items">элементы прошедшие фильтрацию для правила</param>
        /// <returns>строку запроса и батч данных в обертке </returns>
        public IEnumerable<ViewRuleRequestModelWrapper> GetDataRequestString(List<AdInputType> items)
        {
            var viewedItems = GetViewedItems(items);
            if (viewedItems == null)
            {
                yield return null;
            }
            else
            {
                int numberOfBatch = 0;
                foreach (var batch in viewedItems.Batch(_option.BatchSize))
                {
                    var startItemIndex = _option.StartPosition + (numberOfBatch++ * _option.BatchSize);
                    var stringRequest = CreateStringRequest(batch, startItemIndex);
                    var stringResponse = CreateStringResponse();
                    if (stringRequest == null)
                        continue;

                    yield return new ViewRuleRequestModelWrapper
                    {
                        StartItemIndex = startItemIndex,
                        BatchSize = _option.BatchSize,
                        BatchedData = batch,              
                        StringRequest = stringRequest,
                        StringResponse = stringResponse,
                        RequestOption = _option.RequestOption,
                        ResponseOption = _option.ResponseOption
                    };
                }
            }
        }



        /// <summary>
        /// Создать строку запроса ПОД КОМАНДУ.
        /// Body содержит готовый запрос для команды.
        /// </summary>
        /// <returns></returns>
        public ViewRuleRequestModelWrapper GetCommandRequestString()
        {
            var header = _option.RequestOption.Header;
            var body = _option.RequestOption.Body;
            var footer = _option.RequestOption.Footer;
            var requestCommandOption = _option.RequestOption;

            //КОНКАТЕНИРОВАТЬ СТРОКИ В СУММАРНУЮ СТРОКУ-------------------------------------------------------------------------------------
            //resSumStr содержит только ЗАВИСИМЫЕ данные: {AddressDevice} {NByte} {CRC}}
            var resSumStr = header + body + footer;

            //ВСТАВИТЬ ЗАВИСИМЫЕ ДАННЫЕ В ТЕЛО ЗАПРОСА--------------------------------------------------------------------------------------
            var resBodyDependentStr = MakeBodyDependentInserts(resSumStr);

            //ВСТАВИТЬ ЗАВИСИМЫЕ ДАННЫЕ ({AddressDevice} {NByte} {CRC})---------------------------------------------------------------------
            var resDependencyStr = MakeDependentInserts(resBodyDependentStr, requestCommandOption);

            return new ViewRuleRequestModelWrapper
            {
                BatchedData = null,
                StringRequest = resDependencyStr,
                RequestOption = _option.RequestOption,
                ResponseOption = _option.ResponseOption
            };
        }


        /// <summary>
        /// Вернуть элементы из диапазона укзанного в правиле отображения
        /// Если границы диапазона не прпавильны вернуть null
        /// </summary>
        private IEnumerable<AdInputType> GetViewedItems(List<AdInputType> items)
        {
            try
            {
                return items.GetRange(_option.StartPosition, _option.Count);
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// Создать строку Запроса (используя форматную строку RequestOption) из одного батча данных.
        /// </summary>
        private string CreateStringRequest(IEnumerable<AdInputType> batch, int startItemIndex)
        {
            var items = batch.ToList();
            var header = _option.RequestOption.Header;
            var body = _option.RequestOption.Body;
            var footer = _option.RequestOption.Footer;
            var requestOption = _option.RequestOption;

            //ЗАПОЛНИТЬ ТЕЛО ЗАПРОСА (вставить НЕЗАВИСИМЫЕ данные)-------------------------------------------------------------------------         
            var listBodyStr= new List<string>();
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var currentRow = startItemIndex + i + 1;
                var res = MakeBodySectionIndependentInserts(body, item, currentRow);

                //ВСТАВИТЬ ЗАВИСИМЫЕ ДАННЫЕ В ТЕЛО ЗАПРОСА--------------------------------------------------------------------------------------
                var resBodyDependentStr = MakeBodyDependentInserts(res);
                listBodyStr.Add(resBodyDependentStr);
            }

            //ПРОВЕРИТЬ ДЛИНУ ТЕЛА ЗАПРОСА (если превышение, то строка запроса не формируется)---------------------------------------------------------------------------------------------
            var listLimitBodyStr= CheckLimitBodySectionLenght(listBodyStr);
            if(listLimitBodyStr == null)
               return null;

            //КОНКАТЕНИРОВАТЬ СТРОКИ В СУММАРНУЮ СТРОКУ-----------------------------------------------------------------------------------------
            //resSumStr содержит только ЗАВИСИМЫЕ данные: {AddressDevice} {NByte} {CRC}}
            var limitBodyStr = listLimitBodyStr.Aggregate((i, j) => i + j);
            var resSumStr = header + limitBodyStr + footer;

            //ВСТАВИТЬ ЗАВИСИМЫЕ ДАННЫЕ ({AddressDevice} {NByte} {CRC})-------------------------------------------------------------------------
            var resDependencyStr = MakeDependentInserts(resSumStr, requestOption);

            return resDependencyStr;
        }


        /// <summary>
        /// Создать строку Ответа (используя форматную строку ResponseOption).
        /// </summary>
        private string CreateStringResponse()
        {
            var str = _option.ResponseOption.Body;
            var responseOption = _option.ResponseOption;
            //ВСТАВИТЬ ЗАВИСИМЫЕ ДАННЫЕ ({AddressDevice} {NByte} {CRC})-------------------------------------------------------------------------
            var resDependencyStr = MakeDependentInserts(str, responseOption);
            return resDependencyStr;
        }


        /// <summary>
        /// Первоначальная вставка НЕЗАВИСИМЫХ переменных
        /// </summary>
        private string MakeBodySectionIndependentInserts(string body, AdInputType uit, int currentRow)
        {
            var lang = uit.Lang;
            //ЗАПОЛНИТЬ СЛОВАРЬ ВСЕМИ ВОЗМОЖНЫМИ ВАРИАНТАМИ ВСТАВОК
            var typeTrain = uit.TrainType?.GetName(lang);
            var typeAlias = uit.TrainType?.GetNameAlias(lang);
            var eventTrain = uit.Event?.GetName(lang);
            var addition = uit.Addition?.GetName(lang);
            var stations = CreateStationsStr(uit, lang);
            var stationsCut = CreateStationsCutStr(uit, lang);
            var note = uit.Note?.GetName(lang);
            var daysFollowing = uit.DaysFollowing?.GetName(lang);
            var daysFollowingAlias = uit.DaysFollowing?.GetNameAlias(lang);
            var arrivalTime = uit.ArrivalTime ?? DateTime.MinValue;
            var departureTime = uit.DepartureTime ?? DateTime.MinValue;
            var time = (uit.Event?.Num != null && uit.Event.Num == 0) ? arrivalTime : departureTime;
            var dict = new Dictionary<string, object>
            {
                ["TypeName"] = string.IsNullOrEmpty(typeTrain) ? " " : typeTrain,
                ["TypeAlias"] = string.IsNullOrEmpty(typeAlias) ? " " : typeAlias,
                [nameof(uit.NumberOfTrain)] = string.IsNullOrEmpty(uit.NumberOfTrain) ? " " : uit.NumberOfTrain,
                [nameof(uit.PathNumber)] = string.IsNullOrEmpty(uit.PathNumber) ? " " : uit.PathNumber,
                [nameof(uit.Platform)] = string.IsNullOrEmpty(uit.Platform) ? " " : uit.Platform,
                [nameof(uit.Event)] = string.IsNullOrEmpty(eventTrain) ? " " : eventTrain,
                [nameof(uit.Addition)] = string.IsNullOrEmpty(addition) ? " " : addition,
                ["Stations"] = string.IsNullOrEmpty(stations) ? " " : stations,
                ["StationsCut"] = string.IsNullOrEmpty(stationsCut) ? " " : stationsCut,
                [nameof(uit.StationArrival)] = uit.StationArrival?.GetName(lang) ?? " ",
                [nameof(uit.StationDeparture)] = uit.StationDeparture?.GetName(lang) ?? " ",
                [nameof(uit.Note)] = string.IsNullOrEmpty(note) ? " " : note,
                ["DaysFollowing"] = string.IsNullOrEmpty(daysFollowing) ? " " : daysFollowing,
                ["DaysFollowingAlias"] = string.IsNullOrEmpty(daysFollowingAlias) ? " " : daysFollowingAlias,
                [nameof(uit.DelayTime)] = uit.DelayTime ?? DateTime.MinValue,
                [nameof(uit.ExpectedTime)] = uit.ExpectedTime,
                ["TArrival"] = arrivalTime,
                ["TDepart"] = departureTime,
                ["Hour"] = DateTime.Now.Hour,
                ["Minute"] = DateTime.Now.Minute,
                ["Second"] = DateTime.Now.Second,
                ["Time"] = time,
                ["SyncTInSec"] = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second,
                ["rowNumber"] = currentRow
            };
            //ВСТАВИТЬ ПЕРЕМЕННЫЕ ИЗ СЛОВАРЯ В body
            var resStr = HelpersString.StringTemplateInsert(body, dict);
            return resStr;
        }


        /// <summary>
        /// Вставить зависимые (вычисляемые) данные в ТЕЛО запроса 
        /// {NumberOfCharacters}
        /// </summary>
        /// <param name="str">входная строка тела в которой только ЗАВИСИМЫЕ данные</param>
        /// <returns></returns>
        private string MakeBodyDependentInserts(string str)
        {
            if (str.Contains("}"))                                                           //если указанны переменные подстановки
            {
                var subStr = str.Split('}');
                StringBuilder resStr = new StringBuilder();
                for (var index = 0; index < subStr.Length; index++)
                {
                    var s = subStr[index];
                    var replaseStr = (s.Contains("{")) ? (s + "}") : s;
                    //1. Подсчет кол-ва символов
                    if (replaseStr.Contains("NumberOfCharacters"))
                    {
                        var targetStr = (subStr.Length > (index + 1)) ? subStr[index + 1] : string.Empty;
                        if (Regex.Match(targetStr, "\\\"(.*)\"").Success) //
                        {
                            var matchString = Regex.Match(targetStr, "\\\"(.*)\\\"").Groups[1].Value;
                            if (!string.IsNullOrEmpty(matchString))
                            {
                                var lenght = matchString.TrimEnd('\\').Length;
                                var dateFormat = Regex.Match(replaseStr, "\\{NumberOfCharacters:(.*)\\}").Groups[1].Value;
                                var formatStr = !string.IsNullOrEmpty(dateFormat) ?
                                    string.Format(replaseStr.Replace("NumberOfCharacters", "0"), lenght.ToString(dateFormat)) :
                                    string.Format(replaseStr.Replace("NumberOfCharacters", "0"), lenght);
                                resStr.Append(formatStr);
                            }
                        }
                        continue;
                    }
                    ////2. Вставка хххх
                    //if (replaseStr.Contains("хххх"))
                    //{
                    //    continue;
                    //}

                    //Добавим в неизменном виде спецификаторы байтовой информации.
                    resStr.Append(replaseStr);
                }
                return resStr.ToString().Replace("\\\"", string.Empty); //заменить \"
            }
            return str;
        }


        /// <summary>
        /// Ограничить длинну строки
        /// </summary>
        private List<string> CheckLimitBodySectionLenght(List<string> bodyList)
        {
            double totalBodyCount= bodyList.Sum(s => s.Length);
            var maxBodyLenght = _option.RequestOption.MaxBodyLenght;
            if (totalBodyCount < maxBodyLenght)
               return bodyList;
            else
            {
                _logger.Warning($"Строка тела запроса СЛИШКОМ БОЛЬШАЯ {totalBodyCount} > {maxBodyLenght}. Превышение на  {totalBodyCount - maxBodyLenght}");
                return null;
            }
        }


        /// <summary>
        /// Первоначальная вставка ЗАВИСИМЫХ переменных
        ///  {AddressDevice} {NByte} {NumberOfCharacters} {CRC}
        /// </summary>
        private string MakeDependentInserts(string str, RequestResonseOption option)
        {
            /*
              1. Вставит AddressDevice и Вычислить NumberOfCharacters и вставить.
              2. Вычислить NByte (кол-во байт между {NByte} и {CRC}) и вставить.
              3. Вычислить CRC и вставить
            */
            str = MakeAddressDevice(str);
            str = MakeNByte(str);
            str = MakeCrc(str, option.Format);
            str = SwitchFormatCheck2Hex(str, option);
            return str;
        }


        /// <summary>
        /// Вернуть станции в зависимости от События поезда("ПРИБ"/"ОТПР"/"СТОЯНКА").
        /// </summary>
        private string CreateStationsCutStr(AdInputType uit, Lang lang)
        {
            var eventNum = uit.Event?.Num;
            if (!eventNum.HasValue)
                return string.Empty;

            var stArrival = uit.StationArrival?.GetName(lang);
            var stDepart = uit.StationDeparture?.GetName(lang);
            var stations = string.Empty;
            switch (eventNum.Value)
            {
                case 0: //"ПРИБ"
                    stations = stArrival;
                    break;
                case 1:  //"ОТПР"
                    stations = stDepart;
                    break;
                case 2:   //"СТОЯНКА"
                    stations = $"{stArrival}-{stDepart}";
                    break;
            }
            return stations;
        }


        /// <summary>
        /// Вернуть станции {stArrival}-{stDepart}, если обе не NULL
        /// </summary>
        private string CreateStationsStr(AdInputType uit, Lang lang)
        {
            var stArrival = uit.StationArrival?.GetName(lang);
            var stDepart = uit.StationDeparture?.GetName(lang);
            var stations = string.Empty;
            if (!string.IsNullOrEmpty(stArrival) && !string.IsNullOrEmpty(stDepart))
            {
                stations = $"{stArrival}-{stDepart}";
            }
            return stations;
        }


        /// <summary>
        /// Заменить все переменные NumberOfCharacters.
        /// Вычислить N символов след. за NumberOfCharacters в кавычках
        /// </summary>
        private string MakeAddressDevice(string str)
        {
            var dict = new Dictionary<string, object>
            {
                ["AddressDevice"] =  int.TryParse(_addressDevice, out var address) ? address : 0
            };
            //ВСТАВИТЬ ПЕРЕМЕННЫЕ ИЗ СЛОВАРЯ В body
            var resStr = HelpersString.StringTemplateInsert(str, dict);
            return resStr;
        }


        private string MakeNByte(string str)
        {
            var requestFillBodyWithoutConstantCharacters = str.Replace("STX", string.Empty).Replace("ETX", string.Empty);

            //ВЫЧИСЛЯЕМ NByte---------------------------------------------------------------------------
            int lenght = 0;
            string matchString = null;

            if(Regex.Match(requestFillBodyWithoutConstantCharacters, "{NbyteFull(.*)}(.*){CRC(.*)}").Success)
            {
                matchString = Regex.Match(requestFillBodyWithoutConstantCharacters, "{Nbyte(.*)}(.*){CRC(.*)}").Groups[2].Value;          
                if (HelpersBool.ContainsHexSubStr(matchString))
                {
                    var format = _option.RequestOption.Format;
                    var buf = matchString.ConvertStringWithHexEscapeChars2ByteArray(format);
                    var lenghtBody = buf.Count;            
                    var lenghtAddress= 1;
                    var lenghtNByte = 1;
                    var lenghtCrc = 1;
                    lenght = lenghtBody + lenghtAddress + lenghtNByte + lenghtCrc;
                }
                else
                {
                    lenght = matchString.Length;
                }
            }
            else
            if (Regex.Match(requestFillBodyWithoutConstantCharacters, "{Nbyte(.*)}(.*){CRC(.*)}").Success) //вычислили длинну строки между Nbyte и CRC
            {
                matchString = Regex.Match(requestFillBodyWithoutConstantCharacters, "{Nbyte(.*)}(.*){CRC(.*)}").Groups[2].Value;
                lenght = matchString.Length;
            }
            else if (Regex.Match(requestFillBodyWithoutConstantCharacters, "{Nbyte(.*)}(.*)").Success)//вычислили длинну строки от Nbyte до конца строки
            {
                matchString = Regex.Match(requestFillBodyWithoutConstantCharacters, "{Nbyte(.*)}(.*)").Groups[1].Value;
                lenght = matchString.Length;
            }


            //ЗАПОНЯЕМ ВСЕ СЕКЦИИ ДО CRC
            var subStr = requestFillBodyWithoutConstantCharacters.Split('}');
            StringBuilder resStr = new StringBuilder();
            foreach (var s in subStr)
            {
                var replaseStr = (s.Contains("{")) ? (s + "}") : s;
                if (replaseStr.Contains("NbyteFull"))
                {
                    var formatStr = string.Format(replaseStr.Replace("NbyteFull", "0"), lenght);
                    resStr.Append(formatStr);
                }
                else
                if (replaseStr.Contains("Nbyte"))
                {
                    var formatStr = string.Format(replaseStr.Replace("Nbyte", "0"), lenght);
                    resStr.Append(formatStr);
                }
                else
                {
                    resStr.Append(replaseStr);
                }
            }
            return resStr.ToString();
        }


        private string MakeCrc(string str, string format)
        {
            var matchString = Regex.Match(str, "(.*){CRC(.*)}").Groups[1].Value;
            matchString = matchString.Replace("\u0002", string.Empty).Replace("\u0003", string.Empty);
            var crcBytes = HelpersBool.ContainsHexSubStr(matchString) ?
                matchString.ConvertStringWithHexEscapeChars2ByteArray(format).ToArray() :
                matchString.ConvertString2ByteArray(format);
      
            //вычислить CRC по правилам XOR
            if (str.Contains("CRCXor"))
            {
                byte crc = CrcCalc.CalcXor(crcBytes);
                str = string.Format(str.Replace("CRCXor", "0"), crc);
            }
            else
            if (str.Contains("CRCMod256"))
            {
                byte crc = CrcCalc.CalcMod256(crcBytes);
                str = string.Format(str.Replace("CRCMod256", "0"), crc);
            }
      
            return str;
        }


        private string SwitchFormatCheck2Hex(string str, RequestResonseOption option)
        {
            var format = option.Format;
            if (str.Contains("0x"))
            {
                var buf = str.ConvertStringWithHexEscapeChars2ByteArray(format);
                var res = buf.ArrayByteToString("X2");
                option.SwitchFormat("HEX");
                return res;
            }

            return str;
        }

        #endregion
    }


    /// <summary>
    /// Единица запроса обработанная ViewRule.
    /// 
    /// </summary>
    public class ViewRuleRequestModelWrapper
    {
        public int StartItemIndex { get; set; }                     //Начальный индекс (в базовом массиве, после TakeItems) элемента после разбиения на батчи.
        public int BatchSize { get; set; }                          //Размер батча.
        public IEnumerable<AdInputType> BatchedData { get; set; }   //Набор входных данных на базе которых созданна StringRequest.
        public string StringRequest { get; set; }                   //Строка запроса, созданная по правилам RequestOption.
        public string StringResponse { get; set; }                  //Строка ответа, созданная по правилам ResponseOption.
        public int BodyLenght { get; set; }                         //Размер тела запроса todo: НЕ ИСПОЛЬЗУЕТСЯ
        public RequestOption RequestOption { get; set; }            //Запрос.
        public ResponseOption ResponseOption { get; set; }          //Ответ.
    }
}