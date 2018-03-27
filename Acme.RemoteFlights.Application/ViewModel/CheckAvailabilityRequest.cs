using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Acme.RemoteFlights.Application.ViewModel
{
    public class CheckAvailabilityRequest
    {
        [Required]
        public DateTime startDate { get; set; }

        [Required]
        public DateTime endDate { get; set; }

        [Required]
        public int numberOfPax { get; set; }
    }
}
