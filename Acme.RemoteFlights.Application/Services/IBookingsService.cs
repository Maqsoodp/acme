using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acme.RemoteFlights.Application.ViewModel;

namespace Acme.RemoteFlights.Application.Services
{
    public interface IBookingsService
    {
        Task<List<FlightBookingViewModel>> Search(SearchViewModel filter, CancellationToken cancellationToken);
        Task<List<AvailableViewModel>> CheckAvailability(CheckAvailabilityRequest request, CancellationToken cancellationToken);
        Task<bool> MakeBooking(FlightBookingRequest bookingRequest, CancellationToken cancellationToken);
    }
}
