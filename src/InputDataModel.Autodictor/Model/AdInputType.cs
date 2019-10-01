using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Base.InData;

namespace Domain.InputDataModel.Autodictor.Model
{
    public class AdInputType : InputTypeBase
    {
        #region prop
        public int ScheduleId { get; }                          //ID поезда в годовом расписании (из ЦИС)
        public int TrnId { get; }                               //уникальный ID отправления поезда из ЦИС

        public Lang Lang { get; }                               //Язык вывода

        public string NumberOfTrain { get; }                    //Номер поезда
        public string PathNumber { get; }                       //Номер пути
        public string Platform { get; }                         //Номер Платформы

        public EventTrain Event { get; }                        //Событие (ОТПР./ПРИБ./СТОЯНКА)
        public TypeTrain TrainType { get; }                     //тип поезда
        public VagonDirection VagonDirection { get; }           //Нумерация вагона (с головы, с хвоста)

        public Station StationDeparture { get; }
        public Station StationArrival { get; }
        public Station Stations { get; }                        //ФОРМИРУЕТСЯ при маппинге из StationDeparture - StationArrival
        public Station StationsСut { get; }                     //ФОРМИРУЕТСЯ при маппинге из StationDeparture - StationArrival
        public Station StationWhereFrom { get; }               //ближайшая станция после текущей
        public Station StationWhereTo { get; }                 //ближайшая станция после текущей
        public DirectionStation DirectionStation { get; }       //Направление.

        public DateTime? ArrivalTime { get; }                   //Время прибытия
        public DateTime? DepartureTime { get; }                 //Время отправления
        public DateTime? DelayTime { get; }                     //Время задержки (прибытия или отправления поезда)
        public DateTime ExpectedTime { get; }                   //Ожидаемое время (Время + Время задержки)
        public TimeSpan? StopTime { get; }                      //время стоянки (для транзитов: Время отпр - время приб)

        public Addition Addition { get; }                       //Дополнение (свободная строка)
        public Note Note { get; }                               //Примечание.
        public DaysFollowing DaysFollowing { get; }             //Дни следования
        #endregion



        #region ctor

        public AdInputType(int id, int scheduleId, int trnId, Lang lang, string numberOfTrain, string pathNumber, string platform, EventTrain @event,
        TypeTrain trainType, VagonDirection vagonDirection, Station stationDeparture, Station stationArrival, Station stationWhereFrom,
        Station stationWhereTo, DirectionStation directionStation, DateTime? arrivalTime, DateTime? departureTime, DateTime? delayTime,
        DateTime? expectedTime, TimeSpan? stopTime, Addition addition, Note note, DaysFollowing daysFollowing) : base(id)
        {
            ScheduleId = scheduleId;
            TrnId = trnId;
            Lang = lang;
            NumberOfTrain = numberOfTrain;
            PathNumber = pathNumber;
            Platform = platform;
            Event = @event;
            TrainType = trainType;
            VagonDirection = vagonDirection;
            StationDeparture = stationDeparture;
            StationArrival = stationArrival;
            StationWhereFrom = stationWhereFrom;
            StationWhereTo = stationWhereTo;
            DirectionStation = directionStation;
            ArrivalTime = arrivalTime;
            DepartureTime = departureTime;
            DelayTime = delayTime;
            ExpectedTime = expectedTime ?? DateTime.MinValue;
            StopTime = stopTime;
            Addition = addition;
            Note = note;
            DaysFollowing = daysFollowing;
            StationsСut = CreateStationsCut(StationArrival, StationDeparture, Event);
            Stations = CreateStations(StationArrival, StationDeparture);
        }


        public AdInputType(int id, string numberOfTrain, Note note, string pathNumber, EventTrain @event, TypeTrain trainType, Station stationDeparture, Station stationArrival,
            DateTime? arrivalTime, DateTime? departureTime, Lang lang = Lang.Ru)
            : this(id,0, 0, lang, numberOfTrain, pathNumber, null, @event, trainType, null, stationDeparture, stationArrival,
                null, null, null, arrivalTime, departureTime, null, DateTime.MinValue, null, null, note, null)
        {
           
        }

        /// <summary>
        /// для дефолтного создания объекта DefaultItemJson
        /// </summary>
        private AdInputType() : base(0)
        {
        }
        
        #endregion



        #region Methode

        private static Station CreateStationsCut(Station arrivalSt, Station departureSt, EventTrain ev)
        {
            string CreateStationCutName(string stArrivalName, string stDepartName)
            {
                if (!ev.Num.HasValue)
                    return string.Empty;

                stArrivalName = stArrivalName ?? string.Empty;
                stDepartName = stDepartName ?? string.Empty;
                var stations = string.Empty;
                switch (ev.Num)
                {
                    case 0: //"ПРИБ"
                        stations = stDepartName;
                        break;
                    case 1:  //"ОТПР"
                        stations = stArrivalName;
                        break;
                    case 2:   //"СТОЯНКА"
                        stations = $"{stDepartName}-{stArrivalName}";
                        break;
                }
                return stations;
            }

            var newStation = new Station
            {
                NameRu = CreateStationCutName(arrivalSt?.NameRu, departureSt?.NameRu),
                NameEng = CreateStationCutName(arrivalSt?.NameEng, departureSt?.NameEng),
                NameCh = CreateStationCutName(arrivalSt?.NameCh, departureSt?.NameCh)
            };
            return newStation;
        }


        private static Station CreateStations(Station arrivalSt, Station departureSt)
        {
            string CreateStationName(string stArrivalName, string stDepartName)
            {
                stArrivalName = stArrivalName ?? string.Empty;
                stDepartName = stDepartName ?? string.Empty;

                var stations = string.Empty;
                if (!string.IsNullOrEmpty(stArrivalName) && !string.IsNullOrEmpty(stDepartName))
                {
                    stations = $"{stDepartName}-{stArrivalName}";
                }
                return stations;
            }
            var newStation = new Station
            {
                NameRu = CreateStationName(arrivalSt?.NameRu, departureSt?.NameRu),
                NameEng = CreateStationName(arrivalSt?.NameEng, departureSt?.NameEng),
                NameCh = CreateStationName(arrivalSt?.NameCh, departureSt?.NameCh)
            };
            return newStation;
        }

        #endregion
    }
}