﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.RemoteFlights.Domain.DTO
{
    public class FlightDTO
    {
        public Guid Id { get; set; }

        public string FlightNumber{ get; set; }

        public int Capacity { get; set; }

        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }

        public DateTime startTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}