using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acme.RemoteFlights.Application.ViewModel
{
    public class FlightBookingRequest
    {
        public Guid FlightId { get; set; }
        public DateTime FlightDate { get; set; }
        public string PassengerName { get; set; }
    }
}
