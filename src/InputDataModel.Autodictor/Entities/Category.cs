
namespace Domain.InputDataModel.Autodictor.Entities
{
    /// <summary>
    /// Категория поезда.
    /// ПРИГОРОД/ДАЛЬНИЕ/ПРОЧИЕ
    /// </summary>
    public class Category : TrainBase
    {
        public Category(string typeTrain)
        {
            switch (typeTrain.ToLowerInvariant())
            {
                //ПРИГОРОД
                case "мцд":
                case "пригородный поезд":
                case "пригородный экспресс":
                case "пригородный электропоезд":
                case "туристический электропоезд":
                case "аэроэкспресс":
                case "комфортный электропоезд":
                case "пригородный":
                    NameRu = "Пригородный";
                    NameAliasRu = "Приг.";
                    NameEng = "Suburban";
                    NameAliasEng = "Sub.";
                    break;

                //ДАЛЬНИЕ
                case "пассажирский":
                case "резервная":
                case "скоростной":
                case "скорый":
                case "туристический":
                case "фирменный":
                case "высокоскоростной":
                case "детский":
                    NameRu = "Дальний";
                    NameAliasRu = "Дал.";
                    NameEng = "Long-distance";
                    NameAliasEng = "Long";
                    break;

               //ОЧИСТКА
                case "":
                    break;

                //ПРОЧИЕ
                default:
                    NameRu = "Прочие";
                    NameAliasRu = "Проч.";
                    NameEng = "Others";
                    NameAliasEng = "Oth.";
                    break;

            }
        }
    }
}