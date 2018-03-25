using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Acme.RemoteFlights.Application.ViewModel;

namespace Acme.RemoteFlights.Application.Services
{
    public interface IBookingsService
    {
        Task<List<FlightBookingViewModel>> Search(SearchViewModel filter);
        Task<List<AvailableViewModel>> CheckAvailability(CheckAvailabilityRequest request);
        Task<bool> MakeBooking(FlightBookingViewModel bookingRequest);
    }
}
