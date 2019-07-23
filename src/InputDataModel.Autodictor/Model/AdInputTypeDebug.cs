using System;
using InputDataModel.Autodictor.Entities;
using InputDataModel.Base;

namespace InputDataModel.Autodictor.Model
{
    //TODO: заменить поля на readonly сделать объект иммутабельным (проверить как работает мутация через рефлексию)
    public class AdInputTypeDebug : InputTypeBase
    {

        #region prop

        public int ScheduleId { get; }                         //ID поезда в годовом расписании (из ЦИС)
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
        public Station StationWhereFrom { get; }                //ближайшая станция после текущей
        public Station StationWhereTo { get; }                  //ближайшая станция после текущей
        public DirectionStation DirectionStation { get; }       //Направление.

        public DateTime? ArrivalTime { get; }                   //Время прибытия
        public DateTime? DepartureTime { get; }                 //Время отправления
        public DateTime? DelayTime { get; }                     //Время задержки (прибытия или отправления поезда)
        public DateTime ExpectedTime { get; }                   //Ожидаемое время (Время + Время задержки)
        public TimeSpan? StopTime { get; }                      //время стоянки (для транзитов: Время отпр - время приб)

        public Addition Addition { get; }                       //Дополнение (свободная строка)
        public Note Note { get; }                               //Примечание.
        public DaysFollowing DaysFollowing { get; }             //Дни следования


        //ReadOnly------------------------------------------------------------------------------
        //public readonly int ScheduleId;                       //ID поезда в годовом расписании (из ЦИС)
        //public readonly int TrnId;                            //уникальный ID отправления поезда из ЦИС

        //public readonly Lang Lang;                            //Язык вывода

        //public readonly string NumberOfTrain;                    //Номер поезда
        //public string PathNumber;                        //Номер пути
        //public string Platform;                         //Номер Платформы

        //public EventTrain Event;                        //Событие (ОТПР./ПРИБ./СТОЯНКА)
        //public TypeTrain TrainType;                     //тип поезда
        //public VagonDirection VagonDirection;          //Нумерация вагона (с головы, с хвоста)

        //public Station StationDeparture;
        //public Station StationArrival;
        //public Station Stations;                       //ФОРМИРУЕТСЯ при маппинге из StationDeparture - StationArrival
        //public Station StationsСut;                  //ФОРМИРУЕТСЯ при маппинге из StationDeparture - StationArrival
        //public Station StationWhereFrom;               //ближайшая станция после текущей
        //public Station StationWhereTo;                 //ближайшая станция после текущей
        //public DirectionStation DirectionStation;      //Направление.

        //public DateTime? ArrivalTime;                  //Время прибытия
        //public DateTime? DepartureTime;                 //Время отправления
        //public DateTime? DelayTime;                    //Время задержки (прибытия или отправления поезда)
        //public DateTime ExpectedTim;                  //Ожидаемое время (Время + Время задержки)
        //public TimeSpan? StopTime;                      //время стоянки (для транзитов: Время отпр - время приб)

        //public Addition Addition;                      //Дополнение (свободная строка)
        //public Note Note;                              //Примечание.
        //public DaysFollowing DaysFollowing;             //Дни следования

        #endregion




        #region ctor

        public AdInputTypeDebug(int scheduleId, int trnId, Lang lang, string numberOfTrain, string pathNumber, string platform, EventTrain @event, TypeTrain trainType, VagonDirection vagonDirection, Station stationDeparture, Station stationArrival, Station stationWhereFrom, Station stationWhereTo, DirectionStation directionStation, DateTime? arrivalTime, DateTime? departureTime, DateTime? delayTime, DateTime expectedTime, TimeSpan? stopTime, Addition addition, Note note, DaysFollowing daysFollowing)
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
            ExpectedTime = expectedTime;
            StopTime = stopTime;
            Addition = addition;
            Note = note;
            DaysFollowing = daysFollowing;
            StationsСut = CreateStationsCut(StationArrival, StationDeparture, Event);
            Stations = CreateStations(StationArrival, StationDeparture);
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