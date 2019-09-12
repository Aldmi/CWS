using System;
using System.Collections.Generic;
using Domain.InputDataModel.Base;

namespace Domain.InputDataModel.Autodictor.Model
{
    public class AdInputTypeIndependentInserts : IIndependentInserts
    {
        public Dictionary<string, object> CreateDictionary(object inData)
        {
            if (!(inData is AdInputType castUit))
                throw new InvalidCastException("Не удалолсь выполнить cast входной переменной к типу AdInputType");

            var lang = castUit.Lang;
            //ЗАПОЛНИТЬ СЛОВАРЬ ВСЕМИ ВОЗМОЖНЫМИ ВАРИАНТАМИ ВСТАВОК
            var typeTrain = castUit.TrainType?.GetName(lang);
            var typeAlias = castUit.TrainType?.GetNameAlias(lang);
            var eventTrain = castUit.Event?.GetName(lang);
            var addition = castUit.Addition?.GetName(lang);
            var stations = castUit.Stations?.GetName(lang);
            var stationsCut = castUit.StationsСut?.GetName(lang);
            var note = castUit.Note?.GetName(lang);
            var stationsCutStr = string.IsNullOrEmpty(stationsCut) ? "ПОСАДКИ НЕТ" : stationsCut;
            var daysFollowing = castUit.DaysFollowing?.GetName(lang);
            var daysFollowingAlias = castUit.DaysFollowing?.GetNameAlias(lang);
            var arrivalTime = castUit.ArrivalTime ?? DateTime.MinValue;
            var departureTime = castUit.DepartureTime ?? DateTime.MinValue;
            var time = (castUit.Event?.Num != null && castUit.Event.Num == 0) ? arrivalTime : departureTime;
            var delayTime = castUit.DelayTime ?? DateTime.MinValue;
            var expectedTime = (castUit.ExpectedTime == DateTime.MinValue) ? time : castUit.ExpectedTime;
            var dict = new Dictionary<string, object>
            {
                ["TypeName"] = string.IsNullOrEmpty(typeTrain) ? " " : typeTrain,
                ["TypeAlias"] = string.IsNullOrEmpty(typeAlias) ? " " : typeAlias,
                [nameof(castUit.NumberOfTrain)] = string.IsNullOrEmpty(castUit.NumberOfTrain) ? " " : castUit.NumberOfTrain,
                [nameof(castUit.PathNumber)] = string.IsNullOrEmpty(castUit.PathNumber) ? " " : castUit.PathNumber,
                [nameof(castUit.Platform)] = string.IsNullOrEmpty(castUit.Platform) ? " " : castUit.Platform,
                [nameof(castUit.Event)] = string.IsNullOrEmpty(eventTrain) ? " " : eventTrain,
                [nameof(castUit.Addition)] = string.IsNullOrEmpty(addition) ? " " : addition,
                ["Stations"] = string.IsNullOrEmpty(stations) ? " " : stations,
                ["StationsCut"] = stationsCutStr,
                [nameof(castUit.StationArrival)] = castUit.StationArrival?.GetName(lang) ?? " ",
                [nameof(castUit.StationDeparture)] = castUit.StationDeparture?.GetName(lang) ?? " ",
                [nameof(castUit.Note)] = string.IsNullOrEmpty(note) ? " " : note,
                ["DaysFollowing"] = string.IsNullOrEmpty(daysFollowing) ? " " : daysFollowing,
                ["DaysFollowingAlias"] = string.IsNullOrEmpty(daysFollowingAlias) ? " " : daysFollowingAlias,
                [nameof(castUit.DelayTime)] = castUit.DelayTime ?? DateTime.MinValue,
                [nameof(castUit.ExpectedTime)] = castUit.ExpectedTime,
                ["TArrival"] = arrivalTime,
                ["TDepart"] = departureTime,
                ["Hour"] = DateTime.Now.Hour,
                ["Minute"] = DateTime.Now.Minute,
                ["Second"] = DateTime.Now.Second,
                ["Time"] = time,
                ["DelayTime"] = delayTime,
                ["ExpectedTime"] = expectedTime,
                ["SyncTInSec"] = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second
            };
            return dict;
        }
    }
}