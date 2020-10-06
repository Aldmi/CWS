using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Base.InData;

namespace Domain.InputDataModel.Autodictor.Model
{
    public class AdInputType : InputTypeBase
    {
        #region prop
        public int ScheduleId { get; private set; }                          //ID поезда в годовом расписании (из ЦИС)
        public int TrnId { get; private set; }                               //уникальный ID отправления поезда из ЦИС

        public Lang Lang { get; private set; }                               //Язык вывода

        public string NumberOfTrain { get; private set; }                    //Номер поезда
        public string PathNumber { get; private set; }                       //Номер пути
        public string Platform { get; private set; }                         //Номер Платформы

        public EventTrain Event { get; private set; }                        //Событие (ОТПР./ПРИБ./СТОЯНКА)
        public TypeTrain TrainType { get; private set; }                     //тип поезда
        public VagonDirection VagonDirection { get; private set; }           //Нумерация вагона (с головы, с хвоста)

        public Station StationDeparture { get; private set; }
        public Station StationArrival { get; private set; }
        public Station Stations { get; private set; }                        //ФОРМИРУЕТСЯ при маппинге из StationDeparture - StationArrival
        public Station StationsCut { get; private set; }                     //ФОРМИРУЕТСЯ при маппинге из StationDeparture - StationArrival
        public Station StationWhereFrom { get; private set; }                //ближайшая станция после текущей
        public Station StationWhereTo { get; private set; }                 //ближайшая станция после текущей
        public DirectionStation DirectionStation { get; private set; }       //Направление.

        public DateTime? ArrivalTime { get; private set; }                   //Время прибытия
        public DateTime? DepartureTime { get; private set; }                 //Время отправления
        public DateTime? DelayTime { get; private set; }                     //Время задержки (прибытия или отправления поезда)
        public DateTime ExpectedTime { get; private set; }                   //Ожидаемое время (Время + Время задержки)
        public TimeSpan? StopTime { get; private set; }                      //время стоянки (для транзитов: Время отпр - время приб)
         
        public Addition Addition { get; private set; }                       //Дополнение (свободная строка)
        public Note Note { get; private set; }                               //Примечание.
        public DaysFollowing DaysFollowing { get; private set; }             //Дни следования

        public Emergency Emergency { get; private set; }                     //Нештатки
        public Category Category { get; private set; }                       //Категория поезда. ПРИГОРОД/ДАЛЬНИЕ/ПРОЧИЕ
        #endregion



        #region ctor

        public AdInputType(int id, int scheduleId, int trnId, Lang lang, string numberOfTrain, string pathNumber, string platform, EventTrain @event,
        TypeTrain trainType, VagonDirection vagonDirection, Station stationDeparture, Station stationArrival, Station stationWhereFrom,
        Station stationWhereTo, DirectionStation directionStation, DateTime? arrivalTime, DateTime? departureTime, DateTime? delayTime,
        DateTime? expectedTime, TimeSpan? stopTime, Addition addition, Note note, DaysFollowing daysFollowing, Emergency emergency, Category category) : base(id)
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
            StationsCut = CreateStationsCut(StationArrival, StationDeparture, Event);
            Stations = CreateStations(StationArrival, StationDeparture);
            Emergency = emergency;
            Category = category;
        }


        public AdInputType(int id, string numberOfTrain, Note note, string pathNumber, EventTrain @event, TypeTrain trainType, Station stationDeparture, Station stationArrival,
            DateTime? arrivalTime, DateTime? departureTime, Lang lang = Lang.Ru)
            : this(id,0, 0, lang, numberOfTrain, pathNumber, null, @event, trainType, null, stationDeparture, stationArrival,
                null, null, null, arrivalTime, departureTime, null, DateTime.MinValue, null, null, note, null, null, null)
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

                stArrivalName ??= string.Empty;
                stDepartName ??= string.Empty;
                var stations = ev.Num switch
                {
                    0 => stArrivalName,//"ПРИБ"
                    1 => stDepartName,//"ОТПР"
                    2 => $"{stDepartName}-{stArrivalName}",//"СТОЯНКА"
                    _ => string.Empty
                };
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
                stArrivalName ??= string.Empty;
                stDepartName ??= string.Empty;

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