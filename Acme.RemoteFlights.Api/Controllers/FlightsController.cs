using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acme.RemoteFlights.Application.Services;
using Acme.RemoteFlights.Application.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Produces("application/json")]
    [Route("api/flight")]
    public class FlightsController : Controller
    {
        private readonly IFlightService _flightService;
        private readonly IBookingsService _bookingsService;

        public FlightsController(IFlightService flightService, IBookingsService bookingsService)
        {
            this._flightService = flightService;
            this._bookingsService = bookingsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await this._flightService.GetAll();
            return new ObjectResult(result);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IActionResult> Search([FromBody]SearchViewModel filter)
        {
            var result = await this._bookingsService.Search(filter);
            return new ObjectResult(result);
        }

        [Route("availability")]
        [HttpPost]
        public async Task<IActionResult> CheckAvailability([FromBody]CheckAvailabilityRequest request, CancellationToken cancellationToken)
        {
            var result = await this._bookingsService.CheckAvailability(request);
            return new ObjectResult(result);
        }

        [Route("bookings")]
        [HttpPost]
        public async Task<bool> MakeBooking([FromBody]FlightBookingViewModel bookingRequest)
        {
            return await this._bookingsService.MakeBooking(bookingRequest);
        }

    }
}