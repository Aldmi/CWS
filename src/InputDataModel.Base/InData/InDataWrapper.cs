using System.Collections.Generic;

namespace Domain.InputDataModel.Base.InData
{
    /// <summary>
    /// Контейнер-оболочка над входными данными для обменов
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    public class InDataWrapper<TIn>
    {
        public string DirectHandlerName { get; set; }       //Непосредственное имя обработчика (если == null, то логика сама выбирает обработчик )
        public List<TIn> Datas { get; set; }                //Данные
        public Command4Device Command { get; set; }         //Команды
    }
}