using System;
using System.Collections.Generic;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.Services;

namespace Domain.InputDataModel.Autodictor.Services
{
    public class AdInputTypeIndependentInsertsService : IIndependentInsertsService
    {
        public Dictionary<string, object> CreateDictionary(object inData)
        {
            if (!(inData is AdInputType uit))
                throw new InvalidCastException("Не удалолсь выполнить cast входной переменной к типу AdInputType");

            var lang = uit.Lang;
            //ЗАПОЛНИТЬ СЛОВАРЬ ВСЕМИ ВОЗМОЖНЫМИ ВАРИАНТАМИ ВСТАВОК
            var typeTrain = uit.TrainType?.GetName(lang);
            var typeAlias = uit.TrainType?.GetNameAlias(lang);
            var eventTrain = uit.Event?.GetName(lang);
            var addition = uit.Addition?.GetName(lang);
            var stations = uit.Stations?.GetName(lang);
            var stationsCut = uit.StationsСut?.GetName(lang);
            var note = uit.Note?.GetName(lang);
            var stationsCutStr = string.IsNullOrEmpty(stationsCut) ? "ПОСАДКИ НЕТ" : stationsCut;
            var daysFollowing = uit.DaysFollowing?.GetName(lang);
            var daysFollowingAlias = uit.DaysFollowing?.GetNameAlias(lang);
            var arrivalTime = uit.ArrivalTime ?? DateTime.MinValue;
            var departureTime = uit.DepartureTime ?? DateTime.MinValue;
            var time = (uit.Event?.Num != null && uit.Event.Num == 0) ? arrivalTime : departureTime;
            var delayTime = uit.DelayTime ?? DateTime.MinValue;
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
                ["StationsCut"] = stationsCutStr,
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
                ["SyncTInSec"] = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second
            };
            return dict;
        }
    }
}