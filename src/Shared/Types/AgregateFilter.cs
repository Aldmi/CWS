using System.Collections.Generic;

namespace Shared.Types
{
    public class AgregateFilter
    {
        public List<Predicate> Filters { get; set; }
    }

    public class Predicate
    {
        public string Where { get; set; }             //Булевое выражение для фильтрации (например "(ArrivalTime > DateTime.Now.AddMinute(-100) && ArrivalTime < DateTime.Now.AddMinute(100)) || ((EventTrain == \"Transit\") && (ArrivalTime > DateTime.Now))")
        public string OrderBy { get; set; }           //Имя св-ва для упорядочевания по возрастанию (например "ArrivalTime").
        public int? Take { get; set; }                //N первых элементов. Если элементов меньше, то ДОПОЛНИТЬ список пустыми элементами.
    }
}