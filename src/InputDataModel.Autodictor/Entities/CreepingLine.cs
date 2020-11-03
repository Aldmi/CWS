using System;

namespace Domain.InputDataModel.Autodictor.Entities
{
    public class CreepingLine : TrainBase
    {
        public TimeSpan Duration { get; set; }    //Время анимацйии бегущей строки


        public CreepingLine(string nameRu, string nameEng, TimeSpan duration)
        {
            NameRu = nameRu;
            NameEng = nameEng;
            Duration = duration;
        }
    }
}