using System;
using InputDataModel.Autodictor.Entities;
using InputDataModel.Base;

namespace InputDataModel.Autodictor.Model
{
    public class AdInputTypeDebug : InputTypeBase
    {
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
        }


        public AdInputTypeDebug(int scheduleId)
        {
            ScheduleId = scheduleId;
        }



        public int  ScheduleId { get; set; }                         //ID поезда в годовом расписании (из ЦИС)
        public int TrnId { get; set; }                               //уникальный ID отправления поезда из ЦИС

        public Lang Lang { get; set; }                               //Язык вывода

        public string NumberOfTrain { get; set; }                    //Номер поезда
        public string PathNumber { get; set; }                       //Номер пути
        public string Platform { get; set; }                         //Номер Платформы
        
        public EventTrain Event { get; set; }                        //Событие (ОТПР./ПРИБ./СТОЯНКА)
        public TypeTrain TrainType { get; set; }                     //тип поезда
        public VagonDirection VagonDirection { get; set; }           //Нумерация вагона (с головы, с хвоста)

        public Station StationDeparture { get; set; }
        public Station StationArrival { get; set; }
        public Station Stations { get; set; }                        //ФОРМИРУЕТСЯ при маппинге из StationDeparture - StationArrival
        public Station StationsСut { get; set; }                     //ФОРМИРУЕТСЯ при маппинге из StationDeparture - StationArrival
        public Station  StationWhereFrom { get; set; }               //ближайшая станция после текущей
        public Station  StationWhereTo { get; set; }                 //ближайшая станция после текущей
        public DirectionStation DirectionStation { get; set; }       //Направление.

        public DateTime? ArrivalTime { get; set; }                   //Время прибытия
        public DateTime? DepartureTime { get; set; }                 //Время отправления
        public DateTime? DelayTime { get; set; }                     //Время задержки (прибытия или отправления поезда)
        public DateTime ExpectedTime { get; set; }                   //Ожидаемое время (Время + Время задержки)
        public TimeSpan? StopTime { get; set; }                      //время стоянки (для транзитов: Время отпр - время приб)

        public Addition Addition { get; set; }                       //Дополнение (свободная строка)
        public Note Note { get; set; }                               //Примечание.
        public DaysFollowing DaysFollowing { get; set; }             //Дни следования
    }
}