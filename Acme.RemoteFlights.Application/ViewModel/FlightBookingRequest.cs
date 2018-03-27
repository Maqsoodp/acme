using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Acme.RemoteFlights.Application.ViewModel
{
    public class FlightBookingRequest
    {
        [Required]
        public Guid FlightId { get; set; }

        [Required]
        public DateTime FlightDate { get; set; }

        [Required]
        public string PassengerName { get; set; }
    }
}
