using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Domain.InputDataModel.Autodictor.Entities
{
    /// <summary>
    /// Маршрут.
    /// Формат : "Станция1-Станция2-Станция3"
    /// </summary>
    public class Route : TrainBase
    {
        public Route(string nameRu, string nameEng)
        {
            NameRu = nameRu;
            NameEng = nameEng;
            
            // Stations = RawValue?
            //     .Split('-')
            //     .Select(s => new Station{NameRu = s})
            //     .ToList();
        }

       // public IList<Station> Stations { get; }
    }
}