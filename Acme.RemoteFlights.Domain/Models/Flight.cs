using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.RemoteFlights.Domain.Models
{
    public class Flight
    {
        public Guid Id { get; set; }

        public string FlightNumber{ get; set; }

        public int Capacity { get; set; }

        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }

        public TimeSpan startTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
