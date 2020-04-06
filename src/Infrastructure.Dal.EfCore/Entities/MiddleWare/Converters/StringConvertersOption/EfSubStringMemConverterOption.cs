using System.Collections.Generic;

namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.StringConvertersOption
{
    public class EfSubStringMemConverterOption
    {
        public int Lenght { get; set; }                 // Длина подстроки которую нужно вернуть
        public List<string> InitPharases { get; set; }  // Список фраз (подстрок), которые выделяются из базовой строки и при разбиении на подстроки найденная фраза аключается в начале каждой подстроки
        public char Separator { get; set; }             // Разделитель подстрок
    }
}