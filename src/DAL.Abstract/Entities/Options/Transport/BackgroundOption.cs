namespace DAL.Abstract.Entities.Options.Transport
{
    public class BackgroundOption : EntityBase
    {
        public bool AutoStartBg { get; set; }      //Авто старт бекграунда для данного транспорта
        public bool DutyCycleTimeBg { get; set; }  //Время скважности
    }
}