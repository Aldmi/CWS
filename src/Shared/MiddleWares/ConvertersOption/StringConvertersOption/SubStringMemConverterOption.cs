﻿using System.Collections.Generic;

namespace Shared.MiddleWares.ConvertersOption.StringConvertersOption
{
    public class SubStringMemConverterOption
    {
        public int Lenght { get; set; }                 // Длина подстроки которую нужно вернуть
        public List<string> InitPharases { get; set; }  // Список фраз (подстрок), которые выделяются из базовой строки и при разбиении на подстроки найденная фраза аключается в начале каждой подстроки
        public char Separator { get; set; }             // Разделитель подстрок
        public int BanTime { get; set; }                // Время запрета работы таймера
    }
}