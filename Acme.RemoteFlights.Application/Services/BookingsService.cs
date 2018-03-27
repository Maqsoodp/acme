using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acme.RemoteFlights.Application.ViewModel;
using Acme.RemoteFlights.Domain.Contracts;
using Acme.RemoteFlights.Domain.DTO;
using AutoMapper;

namespace Acme.RemoteFlights.Application.Services
{
    public class BookingsService : IBookingsService
    {
        private readonly IFlightBookingsRepository _flightBookingsRepository;
        private readonly IMapper _mapper;
        public BookingsService(IFlightBookingsRepository flightBookingsRepository, IMapper mapper)
        {
            this._flightBookingsRepository = flightBookingsRepository;
            this._mapper = mapper;
        }

        public async Task<List<FlightBookingViewModel>> Search(SearchViewModel filter, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(filter.passengerName) &&
                filter.date == null &&
                string.IsNullOrWhiteSpace(filter.departureCity) &&
                string.IsNullOrWhiteSpace(filter.arrivalCity) &&
                string.IsNullOrWhiteSpace(filter.flightNumber))
            {
                throw new ArgumentException("No filter parameters passed to search", nameof(filter));
            }

            var result = await this._flightBookingsRepository.Search(cancellationToken, filter.passengerName, filter.date, filter.departureCity, filter.arrivalCity, filter.flightNumber);
            return result.Select(this._mapper.Map<FlightBookingViewModel>).ToList();
        }

        public async Task<List<AvailableViewModel>> CheckAvailability(CheckAvailabilityRequest request, CancellationToken cancellationToken)
        {

            if (request.startDate < DateTime.Now.Date || request.startDate == DateTime.MinValue)
            {
                throw new ArgumentException($"Invalid start date {request.startDate}", nameof(request.startDate));
            }
            if (request.endDate < DateTime.Now || request.startDate == DateTime.MinValue)
            {
                throw new ArgumentException($"Invalid end date {request.endDate}", nameof(request.endDate));
            }
            if (request.numberOfPax < 1)
            {
                throw new ArgumentException($"Passenger count has to be atleast 1 {request.numberOfPax}", nameof(request.numberOfPax));

            }

            var result = new List<AvailableViewModel>();
            for (DateTime date = request.startDate; date.Date <= request.endDate.Date; date = date.AddDays(1))
            {
                var dateValue = await this._flightBookingsRepository.CheckAvailability(date, request.numberOfPax, cancellationToken);
                if(dateValue != null)
                {
                    result.Add(new AvailableViewModel { Date = date, Flights = this._mapper.Map<List<FlightViewModel>>(dateValue) });
                }
            }
            return result;
        }

        private async Task<bool> IsItStillAvailable(Guid flightId, DateTime giveDate, int numberOfPax, CancellationToken cancellationToken)
        {
            return await this._flightBookingsRepository.IsItStillAvailable(flightId, giveDate, numberOfPax, cancellationToken);
        }

        public async Task<bool> MakeBooking(FlightBookingRequest bookingRequest, CancellationToken cancellationToken)
        {
            if (bookingRequest == null)
            {
                throw new ArgumentException($"Invalid request", nameof(bookingRequest));
            }
            if (bookingRequest.FlightDate < DateTime.Now.Date || bookingRequest.FlightDate == DateTime.MinValue)
            {
                throw new ArgumentException($"Invalid Flight date {bookingRequest.FlightDate}", nameof(bookingRequest.FlightDate));
            }
            if (bookingRequest.FlightId == Guid.Empty)
            {
                throw new ArgumentException($"Flight Id cannot be empty {bookingRequest.FlightId}", nameof(bookingRequest.FlightId));
            }
            if (string.IsNullOrWhiteSpace(bookingRequest.PassengerName))
            {
                throw new ArgumentException($"Passenger name cannot be empty {bookingRequest.PassengerName}", nameof(bookingRequest.PassengerName));
            }
            var canBook = await this.IsItStillAvailable(bookingRequest.FlightId, bookingRequest.FlightDate, 1, cancellationToken);
            if (!canBook)
            {
                throw new Exception($"No Seats available");
            }
            var bookingRequestDto = this._mapper.Map<FlightBookingsDTO>(bookingRequest);
            return await this._flightBookingsRepository.MakeBooking(bookingRequestDto, cancellationToken);
        }
    }
}
