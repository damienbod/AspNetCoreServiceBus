﻿using System;

namespace AspNetCoreServiceBusApi2.Model
{
    public class Payload
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Goals { get; set; }
        public DateTime Created { get; set; }
    }
}
