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
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await this._flightService.GetAll(cancellationToken);
            return new ObjectResult(result);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IActionResult> Search([FromBody]SearchViewModel filter, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._bookingsService.Search(filter, cancellationToken);
                return new ObjectResult(result);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae);
            }
        }

        [Route("availability")]
        [HttpPost]
        public async Task<IActionResult> CheckAvailability([FromBody]CheckAvailabilityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._bookingsService.CheckAvailability(request, cancellationToken);
                return new ObjectResult(result);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae);
            }
        }

        [Route("bookings")]
        [HttpPost]
        public async Task<IActionResult> MakeBooking([FromBody]FlightBookingRequest bookingRequest, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._bookingsService.MakeBooking(bookingRequest, cancellationToken);
                return new ObjectResult(result);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae);
            }
        }

    }
}