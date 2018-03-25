using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acme.RemoteFlights.Application.ViewModel
{
    public class CheckAvailabilityRequest
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int numberOfPax { get; set; }
    }
}
