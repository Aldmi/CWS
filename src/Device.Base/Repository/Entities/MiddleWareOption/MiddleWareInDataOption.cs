﻿using System.Collections.Generic;
using Domain.Device.Enums;
using Domain.Device.Repository.Entities.MiddleWareOption.HandlersOption;

namespace Domain.Device.Repository.Entities.MiddleWareOption
{
    public class MiddleWareInDataOption
    {
        public string Description { get; set; }

        public List<StringMiddleWareOption> StringHandlers { get; set; }
        public List<DateTimeMiddleWareOption> DateTimeHandlers { get; set; }
        public List<EnumMiddleWareOption> EnumHandlers { get; set; }

        public InvokerOutput InvokerOutput { get; set; }
    }


    public class InvokerOutput
    {
        public InvokerOutputMode Mode { get; set; }
        public int Time { get; set; }                        //Время сработки события отправки данных
    }
}