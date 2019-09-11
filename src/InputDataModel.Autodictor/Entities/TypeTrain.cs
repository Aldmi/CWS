namespace Domain.InputDataModel.Autodictor.Entities
{
    public class TypeTrain : TrainBase
    {
        public int? Num { get; set; }                            //Тип поезда в цифровом виде


        #region ctor

        //public TypeTrain(int? num)
        //{
        //    Num = num;
        //    switch (Num)
        //    {
        //        case null:
        //            NameRu = null;
        //            NameAliasRu = null;
        //            break;

        //        case 0:
        //            NameRu = "Скорый";
        //            NameAliasRu = "Скор.";
        //            break;

        //        case 1:
        //            NameRu = "Ласточка";
        //            NameAliasRu = "ласт.";
        //            break;

        //        case 2:
        //            break;

        //        case 3:
        //            break;

        //        case 4:
        //            break;
        //    }
        //}


        /// <summary>
        /// Num - Id в БД на стороне ад.
        /// </summary>
        public TypeTrain(string nameRu)
        {
            NameRu = nameRu;
            switch (nameRu)
            {
                case null:
                    Num = null;
                    NameAliasRu = null;
                    break;

                case "":
                    Num = null;
                    NameAliasRu = string.Empty;
                    break;

                case "Пассажирский":
                    NameAliasRu = "Пасс.";
                    Num = 1;
                    break;

                case "Скорый":
                    NameAliasRu = "Скор.";
                    Num = 2;
                    break;

                case "Фирменный":
                    NameAliasRu = "Фирм.";
                    Num = 3;
                    break;

                case "Скоростной":
                    NameAliasRu = "Скорос.";
                    Num = 4;
                    break;

                case "Высокоскоростной":
                    NameAliasRu = "Высоко.";
                    Num = 5;
                    break;

                case "Пригородный электропоезд":
                    NameAliasRu = "Приг.эл.";
                    Num = 6;
                    break;

                case "Пригородный поезд":
                    NameAliasRu = "Приг.";
                    Num = 7;
                    break;

                case "Скоростной электропоезд":
                    NameAliasRu = "Скор. эл.";
                    Num = 8;
                    break;

                case "Почтово-богажный":
                    NameAliasRu = "";
                    Num = 18;
                    break;

                case "Скорый-фирменный":
                    NameAliasRu = "";
                    Num = 22;
                    break;

                case "Туристический":
                    NameAliasRu = "";
                    Num = 23;
                    break;

                case "Детский":
                    NameAliasRu = "";
                    Num = 24;
                    break;

                case "Служебный":
                    NameAliasRu = "";
                    Num = 25;
                    break;

                case "Грузопассажирский":
                    NameAliasRu = "";
                    Num = 26;
                    break;

                case "Комфортный электропоезд":
                    NameAliasRu = "";
                    Num = 28;
                    break;

                case "Аэроэкспресс":
                    NameAliasRu = "";
                    Num = 29;
                    break;

                case "Туристический электропоезд":
                    NameAliasRu = "";
                    Num = 31;
                    break;

                case "Ускоренный э/п":
                    NameAliasRu = "";
                    Num = 32;
                    break;
            }
        }


        public TypeTrain()
        {
        }

        #endregion
    }
}