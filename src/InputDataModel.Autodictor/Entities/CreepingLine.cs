using System;

namespace Domain.InputDataModel.Autodictor.Entities
{
    public class CreepingLine : TrainBase
    {
        public TimeSpan Duration { get; set; }    //Время анимацйии бегущей строки
        public DateTime StartTime { get; set; }   //Время  получения бегущей строки


        public CreepingLine(string nameRu, string nameEng, TimeSpan duration, DateTime startTime)
        {
            NameRu = nameRu;
            NameEng = nameEng;
            Duration = duration;
            StartTime = startTime;
        }

        public CreepingLine(string nameRu, string nameEng, TimeSpan duration) : this(nameRu, nameEng, duration, DateTime.Now) { }
    }
}