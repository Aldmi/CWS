using Infrastructure.Dal.Abstract;

namespace Infrastructure.Background.Concrete
{
    public class BackgroundOption : EntityBase
    {
        public bool AutoStartBg { get; set; }      //Авто старт бекграунда для данного транспорта
        public int DutyCycleTimeBg { get; set; }   //Время скважности для цикл. функций
    }
}