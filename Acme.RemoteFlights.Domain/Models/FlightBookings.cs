using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.RemoteFlights.Domain.Models
{
    public class FlightBooking
    {
        public Guid Id { get; set; }
        public Guid FlightId { get; set; }
        public DateTime FlightDate { get; set; }
        public string PassengerName { get; set; }
    }
}
