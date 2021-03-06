﻿using System.Collections.Generic;
using Infrastructure.Background.Concrete;

namespace Infrastructure.Transport.Http
{
    public class HttpOption : BackgroundOption
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }
}