namespace InputDataModel.Autodictor.Entities
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
                    break;

                case 1:
                    NameRu = "Отправление";
                    NameAliasRu = "ОТПР.";
                    break;

                case 2:
                    NameRu = "Транзит";
                    NameAliasRu = "ТРАНЗ.";
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