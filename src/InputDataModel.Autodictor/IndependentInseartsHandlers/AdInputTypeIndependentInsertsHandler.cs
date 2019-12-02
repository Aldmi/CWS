using System;
using System.Text.RegularExpressions;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.IndependentInseartsHandlers;
using Shared.Helpers;

namespace Domain.InputDataModel.Autodictor.IndependentInseartsHandlers
{
    public class AdInputTypeIndependentInsertsHandler : IIndependentInsertsHandler
    {
        public string CalcInserts(IndependentInsertModel inseart, object inData)
        {
            if (!(inData is AdInputType uit))
                return null;

            var lang = uit.Lang;
            switch (inseart.VarName)
            {
                case "TypeName":
                    return uit.TrainType?.GetName(lang).GetSpaceOrString();

                case "TypeAlias":
                    return uit.TrainType?.GetNameAlias(lang).GetSpaceOrString();

                case nameof(uit.NumberOfTrain):
                    return uit.NumberOfTrain.GetSpaceOrString();

                case nameof(uit.PathNumber):
                    return uit.PathNumber.GetSpaceOrString();

                case nameof(uit.Platform):
                    return uit.Platform.GetSpaceOrString();

                case nameof(uit.Event):
                    return uit.Event?.GetName(lang).GetSpaceOrString();

                case nameof(uit.Addition):
                    return uit.Addition?.GetName(lang).GetSpaceOrString();

                case "Stations":
                    return uit.Stations?.GetName(lang).GetSpaceOrString();

                case "StationsCut":
                    var stationsCut = uit.StationsСut?.GetName(lang);
                    return string.IsNullOrEmpty(stationsCut) ? "ПОСАДКИ НЕТ" : stationsCut;

                case nameof(uit.StationArrival):
                    return uit.StationArrival?.GetName(lang).GetSpaceOrString();

                case nameof(uit.StationDeparture):
                    return uit.StationDeparture?.GetName(lang).GetSpaceOrString();

                case nameof(uit.Note):
                    return uit.Note?.GetName(lang).GetSpaceOrString();

                case "DaysFollowing":
                    return uit.DaysFollowing?.GetName(lang).GetSpaceOrString();

                case "DaysFollowingAlias":
                    return uit.DaysFollowing?.GetNameAlias(lang).GetSpaceOrString();

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

                case "Hour":
                    var hour = DateTime.Now.Hour;
                    format = inseart.Format;
                    return SharedHandlers.IntStrHandler(hour, format);

                case "Minute":
                    var minute = DateTime.Now.Hour;
                    format = inseart.Format;
                    return SharedHandlers.IntStrHandler(minute, format);

                case "Second":
                    var second = DateTime.Now.Hour;
                    format = inseart.Format;
                    return SharedHandlers.IntStrHandler(second, format);

                default:
                    return null;
            }
        }


        public static class SharedHandlers
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
    }
}
