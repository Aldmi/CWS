using System.Collections.Generic;
using Domain.InputDataModel.Base.Enums;

namespace Domain.InputDataModel.Base.InData
{
    public class InputData<TIn>
    {
        /// <summary>
        /// Устройство
        /// </summary>
        public string DeviceName { get; set; }   
        
        /// <summary>
        /// Обмен
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// Непосредственное имя обработчика
        /// </summary>
        public string DirectHandlerName { get; set; }

        /// <summary>
        /// Действие
        /// </summary>
        public DataAction DataAction { get; set; }

        /// <summary>
        /// Данные
        /// </summary>
        public List<TIn> Data { get; set; }

        /// <summary>
        /// Команда
        /// </summary>
        public Command4Device Command { get; set; }


        public InputData<TIn> CloneWithOutDataArray()=> new InputData<TIn>
            {
                DeviceName = DeviceName,
                ExchangeName = ExchangeName,
                DirectHandlerName = DirectHandlerName,
                DataAction = DataAction,
                Command = Command,
            };
        
    }
}