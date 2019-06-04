using System.Collections.Generic;

namespace DAL.Abstract.Entities.Options.MiddleWare.Hadlers
{
    public class StringHandlerMiddleWareOption //TODO: вынести функционал в базовый класс
    {
        public string PropName { get; set; }                       //Имя свойства для обработки
        public List<string> HandlerNames { get; set; }            //Имя обработчиков в цепочке
    }
}