using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.RemoteFlights.Application.ViewModel
{
    public class FlightBookingViewModel
    {
        public Guid Id { get; set; }
        public Guid FlightId { get; set; }
        public FlightViewModel Flight{ get; set; }
        public DateTime FlightDate { get; set; }
        public string PassengerName { get; set; }
    }
}
