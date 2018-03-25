using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.RemoteFlights.Application.ViewModel
{
    public class AvailableViewModel
    {
        public DateTime Date { get; set; }
        public List<FlightViewModel> Flights { get; set; }
    }
}
