using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DAL.Abstract.Entities.Options.Exchange.ProvidersOption;
using Domain.InputDataModel.Autodictor.Model;
using Serilog;
using Shared.CrcCalculate;
using Shared.Extensions;
using Shared.Helpers;

namespace Domain.InputDataModel.Autodictor.DataProviders.ByRuleDataProviders.Rules
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
        private readonly ViewRuleOption _option;
        private readonly ILogger _logger;
        private readonly HelperStringTemplateInsert _helperStringTemplateInsert;

        #endregion



        #region ctor

        public ViewRule(string addressDevice, ViewRuleOption option, ILogger logger)
        {
            _addressDevice = addressDevice;
            _option = option;
            _logger = logger;
            _helperStringTemplateInsert= new HelperStringTemplateInsert(_logger);
        }

        #endregion




        #region prop

        public ViewRuleOption GetCurrentOption => _option;

        #endregion




        #region Methode

        /// <summary>
        /// Создать строку запроса ПОД ДАННЫЕ, подставив в форматную строку запроса значения переменных из списка items.
        /// </summary>
        /// <param name="items">элементы прошедшие фильтрацию для правила</param>
        /// <returns>строку запроса и батч данных в обертке </returns>
        public IEnumerable<ViewRuleTransferWrapper> GetDataRequestString(List<AdInputType> items)
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

                    #region РЕАЛИЗАЦИЯ СПИСКА ЗАПРОС/ОТВЕТ ДЛЯ КАЖДОГО ViewRule (SendingUnitList)

                    //1. requestOption и responseOption обернуть в тип SendingUnit
                    //2. ViewRule хранит List<SendingUnit> SendingUnitList
                    //foreach (var sendingUnit in _option.SendingUnitList)
                    //{
                    //    var requestOption = sendingUnit.RequestOption;
                    //    var responseOption = sendingUnit.ResponseOption;

                    //    var stringRequest = CreateStringRequest(batch, requestOption, startItemIndex); //requestOption передаем
                    //    var stringResponse = CreateStringResponse(responseOption);//responseOption передаем
                    //    if (stringRequest == null)
                    //        continue;

                    //    yield return new ViewRuleRequestModelWrapper
                    //    {
                    //        StartItemIndex = startItemIndex,
                    //        BatchSize = _option.BatchSize,
                    //        BatchedData = batch,
                    //        StringRequest = stringRequest,
                    //        StringResponse = stringResponse,
                    //        RequestOption = _option.RequestOption,
                    //        ResponseOption = _option.ResponseOption
                    //    };
                    //}
                    #endregion

                    RequestTransfer request;
                    ResponseTransfer response;
                    try
                    {
                        request = CreateStringRequest(batch, startItemIndex);
                        if (request == null)
                            continue;
                        response = CreateStringResponse();
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Ошибка формирования запроса или ответа ViewRuleId= {_option.Id}    {ex}");
                        continue;
                    }

                    yield return new ViewRuleTransferWrapper
                    {
                        StartItemIndex = startItemIndex,
                        BatchSize = _option.BatchSize,
                        BatchedData = batch,              
                        Request = request,
                        Response = response,
                    };
                }
            }
        }



        /// <summary>
        /// Создать строку запроса ПОД КОМАНДУ.
        /// Body содержит готовый запрос для команды.
        /// </summary>
        /// <returns></returns>
        public ViewRuleTransferWrapper GetCommandRequestString()
        {
            //TODO: ФОРМИРОВАНИЕ ЗАПРОСА ДЛЯ КОНМАДЫ ВЫНЕСТИ В ОТДЕШЛЬНЫЙ МЕТОД, ПО АНАЛОГИИ С CreateStringRequest()

            var header = _option.RequestOption.Header;
            var body = _option.RequestOption.Body;
            var footer = _option.RequestOption.Footer;
            var format = _option.RequestOption.Format;

            //КОНКАТЕНИРОВАТЬ СТРОКИ В СУММАРНУЮ СТРОКУ-------------------------------------------------------------------------------------
            //resSumStr содержит только ЗАВИСИМЫЕ данные: {AddressDevice} {NByte} {CRC}}
            var resSumStr = header + body + footer;

            //ВСТАВИТЬ ЗАВИСИМЫЕ ДАННЫЕ В ТЕЛО ЗАПРОСА--------------------------------------------------------------------------------------
            var resBodyDependentStr = MakeBodyDependentInserts(resSumStr);

            //ВСТАВИТЬ ЗАВИСИМЫЕ ДАННЫЕ ({AddressDevice} {NByte} {CRC})-------------------------------------------------------------------------
            var resDependencyStr = MakeDependentInserts(resBodyDependentStr, format);

            //ПРОВЕРКА НЕОБХОДИМОСТИ СМЕНЫ ФОРМАТА СТРОКИ.-----------------------------------------------------------------------------------------
            SwitchFormatCheck2Hex(resDependencyStr, format, out var newStr, out var newFormat);

            //ФОРМИРОВАНИЕ ОБЪЕКТА ЗАПРОСА.------------------------------------------------------------------------------------------------
            var request = new RequestTransfer(_option.RequestOption)
            {
                StrRepresentBase = new StringRepresentation(resDependencyStr, format),
                StrRepresent = new StringRepresentation(newStr, newFormat)
            };

            //ФОРМИРОВАНИЕ ОБЪЕКТА ОТВЕТА.-------------------------------------------------------------------------------
            var response = CreateStringResponse();

            return new ViewRuleTransferWrapper
            {
                BatchedData = null,
                Request = request,
                Response = response
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
        private RequestTransfer CreateStringRequest(IEnumerable<AdInputType> batch, int startItemIndex)
        {
            var items = batch.ToList();
            var header = _option.RequestOption.Header;
            var body = _option.RequestOption.Body;
            var footer = _option.RequestOption.Footer;
            var format = _option.RequestOption.Format;
            var maxBodyLenght = _option.RequestOption.MaxBodyLenght;

            //ЗАПОЛНИТЬ ТЕЛО ЗАПРОСА--------------------------------------------------------------------------------------------------------         
            var listBodyStr= new List<string>();
            for (var i = 0; i < items.Count; i++)
            {
                //ВСТАВИТЬ НЕЗАВИСИМЫЕ ДАННЫЕ В ТЕЛО ЗАПРОСА---------------------------------------------------------------------------------
                var item = items[i];
                var currentRow = startItemIndex + i + 1;
                var res = MakeBodySectionIndependentInserts(body, item, currentRow);

                //ВСТАВИТЬ ЗАВИСИМЫЕ ДАННЫЕ В ТЕЛО ЗАПРОСА--------------------------------------------------------------------------------------
                var resBodyDependentStr = MakeBodyDependentInserts(res);
                listBodyStr.Add(resBodyDependentStr);
            }

            //ПРОВЕРИТЬ ДЛИНУ ТЕЛА ЗАПРОСА (если превышение, то строка запроса не формируется)---------------------------------------------------------------------------------------------
            var listLimitBodyStr= CheckLimitBodySectionLenght(listBodyStr, maxBodyLenght);
            if (listLimitBodyStr == null)
               return null;

            //КОНКАТЕНИРОВАТЬ СТРОКИ В СУММАРНУЮ СТРОКУ-----------------------------------------------------------------------------------------
            //resSumStr содержит только ЗАВИСИМЫЕ данные: {AddressDevice} {NByte} {CRC}}
            var limitBodyStr = listLimitBodyStr.Aggregate((i, j) => i + j);
            var resSumStr = header + limitBodyStr + footer;

            //ВСТАВИТЬ ЗАВИСИМЫЕ ДАННЫЕ ({AddressDevice} {NByte} {CRC})-------------------------------------------------------------------------
            var resDependencyStr = MakeDependentInserts(resSumStr, format);

            //ПРОВЕРКА НЕОБХОДИМОСТИ СМЕНЫ ФОРМАТА СТРОКИ.-----------------------------------------------------------------------------------------
            SwitchFormatCheck2Hex(resDependencyStr, format, out var newStr, out var newFormat);
            //ФОРМИРОВАНИЕ ОБЪЕКТА ЗАПРОСА.------------------------------------------------------------------------------------------------
            var request = new RequestTransfer(_option.RequestOption)
            {
                StrRepresentBase = new StringRepresentation(resDependencyStr, format),
                StrRepresent = new StringRepresentation(newStr, newFormat)                
            };
            return request;
        }


        /// <summary>
        /// Создать строку Ответа (используя форматную строку ResponseOption).
        /// </summary>
        private ResponseTransfer CreateStringResponse()
        {
            var responseOption = _option.ResponseOption;
            if(responseOption == null)
                return null;

            var body = responseOption.Body;
            var format = responseOption.Format;
            //ВСТАВИТЬ ЗАВИСИМЫЕ ДАННЫЕ ({AddressDevice} {NByte} {CRC})-------------------------------------------------------------------------
            var resDependencyStr = MakeDependentInserts(body, format);

            //ПРОВЕРКА НЕОБХОДИМОСТИ СМЕНЫ ФОРМАТА СТРОКИ.-----------------------------------------------------------------------------------------
            SwitchFormatCheck2Hex(resDependencyStr, format, out var newStr, out var newFormat);
            //ФОРМИРОВАНИЕ ОБЪЕКТА ОТВЕТА.------------------------------------------------------------------------------------------------
            var response = new ResponseTransfer(responseOption)
            {
                StrRepresentBase = new StringRepresentation(resDependencyStr, format),
                StrRepresent = new StringRepresentation(newStr, newFormat)
            };
            return response;
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
            var stations = uit.Stations?.GetName(lang);  //CreateStationsStr(uit, lang);
            var stationsCut = uit.StationsСut?.GetName(lang);//CreateStationsCutStr(uit, lang);
            var note = uit.Note?.GetName(lang);
            //DEBUG------Костыль по ограничению------
            //if (noteStr.Length > 100)
            //{
            //    noteStr = "Информация об остановках передается по громкоговорящей связи";
            //}
            var stationsCutStr = string.IsNullOrEmpty(stationsCut) ? "ПОСАДКИ НЕТ" : stationsCut;
            //DEBUG------

            var daysFollowing = uit.DaysFollowing?.GetName(lang);
            var daysFollowingAlias = uit.DaysFollowing?.GetNameAlias(lang);
            var arrivalTime = uit.ArrivalTime ?? DateTime.MinValue;
            var departureTime = uit.DepartureTime ?? DateTime.MinValue;
            var time = (uit.Event?.Num != null && uit.Event.Num == 0) ? arrivalTime : departureTime;
            var delayTime= uit.DelayTime ?? DateTime.MinValue;
            var expectedTime = (uit.ExpectedTime == DateTime.MinValue) ? time : uit.ExpectedTime;
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
                ["StationsCut"] = stationsCutStr,//string.IsNullOrEmpty(stationsCut) ? " " : stationsCut,
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
                ["DelayTime"] = delayTime,
                ["ExpectedTime"] = expectedTime,
                ["SyncTInSec"] = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second,
                ["rowNumber"] = currentRow
            };
            //ВСТАВИТЬ ПЕРЕМЕННЫЕ ИЗ СЛОВАРЯ В body
            var resStr = _helperStringTemplateInsert.StringTemplateInsert(body, dict);
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
        private List<string> CheckLimitBodySectionLenght(List<string> bodyList, int maxBodyLenght)
        {
            double totalBodyCount= bodyList.Sum(s => s.Length);
            if (totalBodyCount < maxBodyLenght)
               return bodyList;

            _logger.Warning($"Строка тела запроса СЛИШКОМ БОЛЬШАЯ {totalBodyCount} > {maxBodyLenght}. Превышение на  {totalBodyCount - maxBodyLenght}");
            return null;
        }


        /// <summary>
        /// Первоначальная вставка ЗАВИСИМЫХ переменных
        ///  {AddressDevice} {NByte} {NumberOfCharacters} {CRC}
        /// </summary>
        private string MakeDependentInserts(string str, string format)
        {
            /*
              1. Вставит AddressDevice и вставить.
              2. Вычислить NByte (кол-во байт между {NByte} и {CRC}) и вставить.
              3. Вычислить CRC и вставить
            */
            str = MakeAddressDevice(str);
            str = MakeNByte(str, format);
            str = MakeCrc(str, format); 
            return str;
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
            var resStr = _helperStringTemplateInsert.StringTemplateInsert(str, dict);
            return resStr;
        }


        private string MakeNByte(string str, string format)
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
            var crcType = Regex.Match(str, "{CRC(.*):(.*)}").Groups[1].Value;
            var crcOption= Regex.Match(crcType, "\\[(.*)\\]").Groups[1].Value;  //Xor[0x02-0x03]
            var startEndChars = crcOption.Split('-');
            var startChar = (startEndChars.Length >= 1) ? startEndChars[0] : String.Empty;
            var endChar = (startEndChars.Length >= 2) ? startEndChars[1] : String.Empty;

            string matchString;
            if (string.IsNullOrEmpty(startChar) && string.IsNullOrEmpty(endChar))
            {
                //Не заданны симолы начала и конца подсчета строки CRC. Берем от начала строки до CRC.
                matchString = Regex.Match(str, "(.*){CRC(.*)}").Groups[1].Value;
            }
            else
            {
                // Оба заданы.
                var strTmp = str.Replace(crcOption, String.Empty);
                var pattern = $"{startChar}(.*){endChar}";
                matchString = Regex.Match(strTmp, pattern).Groups[1].Value;
            }

            //УБРАТЬ МАРКЕРНЫЕ СИМОЛЫ ИЗ ПОДСЧЕТА CRC
            matchString = matchString.Replace("\u0002", string.Empty).Replace("\u0003", string.Empty);

            //ВЫЧИСЛИТЬ МАССИВ БАЙТ ДЛЯ ПОДСЧЕТА CRC
            var crcBytes = HelpersBool.ContainsHexSubStr(matchString) ?
                matchString.ConvertStringWithHexEscapeChars2ByteArray(format).ToArray() :
                matchString.ConvertString2ByteArray(format);

            var replacement = $"CRC{crcType}";
            byte crc = 0x00;
            switch (crcType)
            {
                case string s when s.Contains("XorInverse"):
                    crc = CrcCalc.CalcXorInverse(crcBytes);
                    break;

                case string s when s.Contains("Xor"):
                    crc = CrcCalc.CalcXor(crcBytes);
                    break;

                case string s when s.Contains("Mod256"):
                    crc = CrcCalc.CalcMod256(crcBytes);
                    break;
            }
            str = string.Format(str.Replace(replacement, "0"), crc);
            return str;
        }


        private bool SwitchFormatCheck2Hex(string str, string format, out string newStr, out string newFormat)
        { 
            if (str.Contains("0x"))
            {
                var buf = str.ConvertStringWithHexEscapeChars2ByteArray(format);
                newStr = buf.ArrayByteToString("X2");
                newFormat = "HEX";
                return true;
            }

            newStr = str;
            newFormat = format;
            return false;
        }

        #endregion
    }
}