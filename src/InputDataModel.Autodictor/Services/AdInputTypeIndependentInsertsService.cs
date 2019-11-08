using System;
using System.Collections.Generic;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.Services;
using Shared.Extensions;
using Shared.Helpers;

namespace Domain.InputDataModel.Autodictor.Services
{
    public class AdInputTypeIndependentInsertsService : IIndependentInsertsService
    {
        /// <summary>
        /// Создать словарь подстановок для  AdInputType
        /// </summary>
        public IndependentInserts CreateIndependentInserts(object inData)
        {
            if (!(inData is AdInputType uit))
                throw new InvalidCastException("Не удалолсь выполнить cast входной переменной к типу AdInputType");

            var lang = uit.Lang;
            //ЗАПОЛНИТЬ СЛОВАРЬ ВСЕМИ ВОЗМОЖНЫМИ ВАРИАНТАМИ ВСТАВОК
            var typeTrain = uit.TrainType?.GetName(lang).GetSpaceOrString();
            var typeAlias = uit.TrainType?.GetNameAlias(lang).GetSpaceOrString();
            var numberOfTrain = uit.NumberOfTrain.GetSpaceOrString();
            var pathNumber = uit.PathNumber.GetSpaceOrString();
            var platform = uit.Platform.GetSpaceOrString();
            var eventTrain = uit.Event?.GetName(lang).GetSpaceOrString();
            var addition = uit.Addition?.GetName(lang).GetSpaceOrString();
            var stations = uit.Stations?.GetName(lang).GetSpaceOrString();
            var stationsCut = uit.StationsСut?.GetName(lang);
            var stationsCutStr = string.IsNullOrEmpty(stationsCut) ? "ПОСАДКИ НЕТ" : stationsCut;
            var stationArrival = uit.StationArrival?.GetName(lang).GetSpaceOrString();
            var stationDeparture = uit.StationDeparture?.GetName(lang).GetSpaceOrString();
            var note = uit.Note?.GetName(lang).GetSpaceOrString();
            var daysFollowing = uit.DaysFollowing?.GetName(lang).GetSpaceOrString();
            var daysFollowingAlias = uit.DaysFollowing?.GetNameAlias(lang).GetSpaceOrString();
            var arrivalTime = uit.ArrivalTime ?? DateTime.MinValue;
            var departureTime = uit.DepartureTime ?? DateTime.MinValue;
            var delayTime = uit.DelayTime ?? DateTime.MinValue;
            var time = (uit.Event?.Num != null && uit.Event.Num == 0) ? arrivalTime : departureTime;
            var expectedTime = (uit.ExpectedTime == DateTime.MinValue) ? time : uit.ExpectedTime;
            var syncTInSec = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;

            var independentInseart = new IndependentInserts();
            independentInseart.TryAddValue("TypeName", typeTrain);
            independentInseart.TryAddValue("TypeAlias", typeAlias);
            independentInseart.TryAddValue(nameof(uit.NumberOfTrain), numberOfTrain);
            independentInseart.TryAddValue(nameof(uit.PathNumber), pathNumber);
            independentInseart.TryAddValue(nameof(uit.Platform), platform);
            independentInseart.TryAddValue(nameof(uit.Event), eventTrain);
            independentInseart.TryAddValue(nameof(uit.Addition), addition);
            independentInseart.TryAddValue("Stations", stations);
            independentInseart.TryAddValue("StationsCut", stationsCutStr);
            independentInseart.TryAddValue(nameof(uit.StationArrival), stationArrival);
            independentInseart.TryAddValue(nameof(uit.StationDeparture), stationDeparture);
            independentInseart.TryAddValue(nameof(uit.Note), note);
            independentInseart.TryAddValue("DaysFollowing", daysFollowing);
            independentInseart.TryAddValue("DaysFollowingAlias", daysFollowingAlias);
            independentInseart.TryAddValue("TArrival", arrivalTime);
            independentInseart.TryAddValue("TDepart", departureTime);
            independentInseart.TryAddValue(nameof(uit.DelayTime), delayTime);
            independentInseart.TryAddValue("Time", time);
            independentInseart.TryAddValue("ExpectedTime", expectedTime);
            independentInseart.TryAddValue("SyncTInSec", syncTInSec);
            independentInseart.TryAddValue("Hour", DateTime.Now.Hour);
            independentInseart.TryAddValue("Minute", DateTime.Now.Minute);
            independentInseart.TryAddValue("Second", DateTime.Now.Second);

            //var dict = new Dictionary<string, object>
            //{
            //    ["TypeName"] = typeTrain,
            //    ["TypeAlias"] = typeAlias,
            //    [nameof(uit.NumberOfTrain)] = numberOfTrain,
            //    [nameof(uit.PathNumber)] = pathNumber,
            //    [nameof(uit.Platform)] = platform,
            //    [nameof(uit.Event)] = eventTrain,
            //    [nameof(uit.Addition)] = addition,
            //    ["Stations"] = stations,
            //    ["StationsCut"] = stationsCutStr,
            //    [nameof(uit.StationArrival)] = stationArrival,
            //    [nameof(uit.StationDeparture)] = stationDeparture,
            //    [nameof(uit.Note)] = note,
            //    ["DaysFollowing"] =  daysFollowing,
            //    ["DaysFollowingAlias"] = daysFollowingAlias,
            //    ["TArrival"] = arrivalTime,
            //    ["TDepart"] = departureTime,
            //    [nameof(uit.DelayTime)] = delayTime,
            //    ["Time"] = time,
            //    ["ExpectedTime"] = expectedTime,
            //    ["SyncTInSec"] = syncTInSec,
            //    ["Hour"] = DateTime.Now.Hour,
            //    ["Minute"] = DateTime.Now.Minute,
            //    ["Second"] = DateTime.Now.Second,
            //};
            return independentInseart;
        }
    }
}