using System;

namespace Domain.InputDataModel.Autodictor.Entities
{
    public class CreepingLine : TrainBase
    {
        public TimeSpan WorkTime { get; set; }    //Время анимацйии бегущей строки


        public CreepingLine(string nameRu, string nameEng, TimeSpan workTime)
        {
            NameRu = nameRu;
            NameEng = nameEng;
            WorkTime = workTime;
        }
    }
}