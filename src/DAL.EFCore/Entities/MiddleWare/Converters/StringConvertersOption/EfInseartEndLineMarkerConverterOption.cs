﻿namespace DAL.EFCore.Entities.MiddleWare.Converters.StringConvertersOption
{
    public class EfInseartEndLineMarkerConverterOption
    {
        public int LenghtLine { get; set; }  //Длинна строки, после которой вставляется маркер конца строки
        public string Marker { get; set; }  //Маркер конца строки
    }
}