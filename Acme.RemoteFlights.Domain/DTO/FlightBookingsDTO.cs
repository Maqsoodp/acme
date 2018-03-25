using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.RemoteFlights.Domain.DTO
{
    public class FlightBookingsDTO
    {
        
        public Guid Id { get; set; }
        public Guid FlightId { get; set; }

        public FlightDTO Flight { get; set; }
        public DateTime FlightDate { get; set; }
        public string PassengerName { get; set; }

    }
}
