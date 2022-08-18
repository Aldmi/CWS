namespace Domain.InputDataModel.Autodictor.Entities
{
    /// <summary>
    /// Нештатные ситуации
    /// </summary>
    public class Emergency : TrainBase
    {
        public int? Num { get; set; }                            //числовое представление нештатки


        public Emergency(string numStr)
        {
            var res = int.TryParse(numStr, out int num);
            if (!res) return;
            Num = num;
            switch (Num.Value)
            {
                case 0:
                    NameRu = " ";
                    NameAliasRu = " ";
                    NameEng = " ";
                    NameAliasEng = " ";
                    break;

                case 1:
                    NameRu = "Отменен";
                    NameAliasRu = "Отм.";
                    NameEng = "Canceled";
                    NameAliasEng = "Canc.";
                    break;

                case 2:
                    NameRu = "Задержка прибытия";
                    NameAliasRu = "Задерж. приб.";
                    NameEng = "Arrival delay";
                    NameAliasEng = "Arriv. del.";
                    break;

                case 4:
                    NameRu = "Задержка отправления";
                    NameAliasRu = "Задерж. отпр.";
                    NameEng = "Departure Delay";
                    NameAliasEng = "Depart. Del.";
                    break;

                case 8:
                    NameRu = "Отправление по готовности";
                    NameAliasRu = "Отпр. по готов.";
                    NameEng = "Departure when ready";
                    NameAliasEng = "Depar. when ready";
                    break;
            }
        }
    }
}