using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Domain.InputDataModel.Autodictor.Entities
{
    /// <summary>
    /// Маршрут.
    /// Формат : "Станция1-Станция2-Станция3"
    /// </summary>
    public class Route : TrainBase
    {
        public Route(string rawValue)
        {
            RawValue = rawValue;
            NameRu = rawValue;
            Stations = RawValue?
                .Split('-')
                .Select(s => new Station{NameRu = s})
                .ToList();
        }

        public string RawValue { get; }
        
        public IList<Station> Stations { get; }
    }
}