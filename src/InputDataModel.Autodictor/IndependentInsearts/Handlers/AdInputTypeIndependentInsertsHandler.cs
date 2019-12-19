using System;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.Handlers;
using Shared.Helpers;

namespace Domain.InputDataModel.Autodictor.IndependentInsearts.Handlers
{
    public class AdInputTypeIndependentInsertsHandler : IIndependentInsertsHandler
    {
        public string CalcInserts(StringInsertModel inseart, object inData)
        {
            if (!(inData is AdInputType uit))
                return null; 

            var lang = uit.Lang;
            string str;
            switch (inseart.VarName)
            {
                case "TypeName":
                    str = uit.TrainType?.GetName(lang);
                    break;

                case "TypeAlias":
                    str = uit.TrainType?.GetNameAlias(lang);
                    break;

                case nameof(uit.NumberOfTrain):
                    str = uit.NumberOfTrain;
                    break;
                
                case nameof(uit.PathNumber):
                    str = uit.PathNumber;
                    break;

                case nameof(uit.Platform):
                    str = uit.Platform;
                    break;
                
                case nameof(uit.Event):
                    str = uit.Event?.GetName(lang);
                    break;

                case nameof(uit.Addition):
                    str = uit.Addition?.GetName(lang);
                    break;
                
                case "Stations":
                    str = uit.Stations?.GetName(lang);
                    break;

                case "StationsCut":
                    str = uit.StationsСut?.GetName(lang);
                    break;
                    //var stationsCut = uit.StationsСut?.GetName(lang);
                    //return string.IsNullOrEmpty(stationsCut) ? "ПОСАДКИ НЕТ" : stationsCut;

                case nameof(uit.StationArrival):
                    str = uit.StationArrival?.GetName(lang);
                    break;

                case nameof(uit.StationDeparture):
                    str = uit.StationDeparture?.GetName(lang);
                    break;

                case nameof(uit.Note):
                    str = uit.Note?.GetName(lang);
                    break;

                case "DaysFollowing":
                    str = uit.DaysFollowing?.GetName(lang);
                    break;

                case "DaysFollowingAlias":
                    str = uit.DaysFollowing?.GetNameAlias(lang);
                    break;

                case "TArrival":
                    var arrivalTime = uit.ArrivalTime ?? DateTime.MinValue;
                    var format = inseart.Format;
                    return SharedHandlers.DateTimeStrHandler(arrivalTime, format);

                case "TDepart":
                    var departureTime = uit.DepartureTime ?? DateTime.MinValue;
                    format = inseart.Format;
                    return SharedHandlers.DateTimeStrHandler(departureTime, format);

                case nameof(uit.DelayTime):
                    var delayTime = uit.DelayTime ?? DateTime.MinValue;
                    format = inseart.Format;
                    return SharedHandlers.DateTimeStrHandler(delayTime, format);

                case "Time":
                    arrivalTime = uit.ArrivalTime ?? DateTime.MinValue;
                    departureTime = uit.DepartureTime ?? DateTime.MinValue;
                    var time = (uit.Event?.Num != null && uit.Event.Num == 0) ? arrivalTime : departureTime;
                    format = inseart.Format;
                    return SharedHandlers.DateTimeStrHandler(time, format);

                case "ExpectedTime":
                    arrivalTime = uit.ArrivalTime ?? DateTime.MinValue;
                    departureTime = uit.DepartureTime ?? DateTime.MinValue;
                    time = (uit.Event?.Num != null && uit.Event.Num == 0) ? arrivalTime : departureTime;
                    var expectedTime = (uit.ExpectedTime == DateTime.MinValue) ? time : uit.ExpectedTime;
                    format = inseart.Format;
                    return SharedHandlers.DateTimeStrHandler(expectedTime, format);

                case "SyncTInSec":
                    var syncTInSec = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
                    format = inseart.Format;
                    return SharedHandlers.IntStrHandler(syncTInSec, format);

                case "Year":
                    var year = DateTime.Now.Year;
                    format = inseart.Format;
                    return SharedHandlers.IntStrHandler(year, format);

                case "Month":
                    var month = DateTime.Now.Month;
                    format = inseart.Format;
                    return SharedHandlers.IntStrHandler(month, format);

                case "Day":
                    var day = DateTime.Now.Day;
                    format = inseart.Format;
                    return SharedHandlers.IntStrHandler(day, format);

                case "Hour":
                    var hour = DateTime.Now.Hour;
                    format = inseart.Format;
                    return SharedHandlers.IntStrHandler(hour, format);

                case "Minute":
                    var minute = DateTime.Now.Minute;
                    format = inseart.Format;
                    return SharedHandlers.IntStrHandler(minute, format);

                case "Second":
                    var second = DateTime.Now.Second;
                    format = inseart.Format;
                    return SharedHandlers.IntStrHandler(second, format);

                default:
                    return null;
            }
            return str.GetSpaceOrString();
        }


        private static class SharedHandlers
        {
            /// <summary>
            /// Вставка DateTime по формату
            /// </summary>
            public static string DateTimeStrHandler(DateTime val, string formatValue)
            {
                const string defaultStr = " ";
                if (val == DateTime.MinValue)
                    return defaultStr;

                object resVal;
                if (formatValue.Contains("Sec")) //формат задан в секундах
                {
                    resVal = (val.Hour * 3600 + val.Minute * 60);
                    formatValue = Regex.Match(formatValue, @"\((.*)\)").Groups[1].Value;
                }
                else
                if (formatValue.Contains("Min")) //формат задан в минутах
                {
                    resVal = (val.Hour * 60 + val.Minute);
                    formatValue = Regex.Match(formatValue, @"\((.*)\)").Groups[1].Value;
                }
                else
                {
                    resVal = val;
                }
                var format = "{0" + formatValue + "}";
                return string.Format(format, resVal);
            }


            /// <summary>
            /// Вставка Int по формату
            /// </summary>
            public static string IntStrHandler(int val, string formatValue)
            {
                var format = "{0" + formatValue + "}";
                return string.Format(format, val);
            }
        }

        public Result<(string, StringInsertModel)> CalcInserts(object inData)
        {
            throw new NotImplementedException();
        }
    }
}
