using System.Collections.Generic;

namespace Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.StringConvertersOption
{
    public class SubStringMemConverterOption : BaseConverterOption
    {
        public int Lenght { get; set; }                 // Длина подстроки которую нужно вернуть
        public List<string> InitPharases { get; set; }  // Список фраз (подстрок), которые выделяются из базовой строки и при разбиении на подстроки найденная фраза аключается в начале каждой подстроки
        public char Separator { get; set; }             // Разделитель подстрок
    }
}