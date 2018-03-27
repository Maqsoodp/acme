using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acme.RemoteFlights.Domain.DTO;

namespace Acme.RemoteFlights.Domain.Contracts
{
    public interface IFlightBookingsRepository
    {
        Task<List<FlightBookingsDTO>> GetAll(CancellationToken cancellationToken);
        Task<List<FlightBookingsDTO>> Search(CancellationToken cancellationToken, string passengerName = null, DateTime? giveDate = null, string giveArrivalCity = null, string giveDepartureCity = null, string giveFlightNumber = null);
        Task<List<FlightDTO>> CheckAvailability(DateTime date, int numberOfPax, CancellationToken cancellationToken);
        Task<bool> MakeBooking(FlightBookingsDTO bookingRequestDto, CancellationToken cancellationToken);
        Task<bool> IsItStillAvailable(Guid flightId, DateTime giveDate, int numberOfPax, CancellationToken cancellationToken);
    }
}
