using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.RemoteFlights.Application.ViewModel
{
    public class SearchViewModel
    {
        public string passengerName { get; set; }
        public DateTime? date { get; set; }
        public string departureCity { get; set; }
        public string arrivalCity { get; set; }
        public string flightNumber { get; set; }
        
    }
}
