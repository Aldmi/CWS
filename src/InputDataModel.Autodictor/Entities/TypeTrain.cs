using System;

namespace Domain.InputDataModel.Autodictor.Entities
{
    public class TypeTrain : TrainBase
    {
        public int? Num { get; set; }                            //Тип поезда в цифровом виде


        #region ctor

        /// <summary>
        /// Num - Id в БД на стороне ад.
        /// </summary>
        public TypeTrain(string nameRu, string nameRuAlias, string nameEng, string nameAliasEng, int? num)
        {
            NameRu = nameRu;
            NameAliasRu = nameRuAlias;
            NameEng= nameEng;
            NameAliasEng = nameAliasEng;
            Num = num;
        }

        public TypeTrain()
        {
        }

        #endregion
    }
}