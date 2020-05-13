namespace Domain.InputDataModel.Autodictor.Entities
{
    public class EventTrain : TrainBase
    {
        public int? Num { get; set; }                            //Тип поезда в цифровом виде


        #region ctor

        public EventTrain(int? num)
        {
            Num = num;
            if(!Num.HasValue)
                return;

            switch (Num.Value)
            {
                case 0:
                    NameRu = "Прибытие";
                    NameAliasRu = "ПРИБ.";
                    NameEng = "Arrival";
                    NameAliasEng = "ARRIV.";
                    break;

                case 1:
                    NameRu = "Отправление";
                    NameAliasRu = "ОТПР.";
                    NameEng = "Departure";
                    NameAliasEng = "DEPAR.";
                    break;

                case 2:
                    NameRu = "Транзит";      
                    NameAliasRu = "ТРАН.";
                    NameEng = "Transit";
                    NameAliasEng = "TRAN.";
                    break;
            }
        }

        //TODO: зачем нужен public конструктор
        public EventTrain()
        {
        }

        #endregion
    }
}