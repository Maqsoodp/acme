using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<List<FlightBookingViewModel>> Search(SearchViewModel filter)
        {
            if (string.IsNullOrWhiteSpace(filter.passengerName) &&
                filter.date == null &&
                string.IsNullOrWhiteSpace(filter.departureCity) &&
                string.IsNullOrWhiteSpace(filter.arrivalCity) &&
                string.IsNullOrWhiteSpace(filter.flightNumber))
            {
                throw new Exception("No filter parameters passed to search");
            }

            filter.passengerName = (!string.IsNullOrWhiteSpace(filter.passengerName)) ? filter.passengerName.ToLower() : null;
            filter.departureCity = (!string.IsNullOrWhiteSpace(filter.departureCity)) ? filter.departureCity.ToLower() : null;
            filter.arrivalCity = (!string.IsNullOrWhiteSpace(filter.arrivalCity)) ? filter.arrivalCity.ToLower() : null;
            filter.flightNumber = (!string.IsNullOrWhiteSpace(filter.flightNumber)) ? filter.flightNumber.ToLower() : null;

            var result = await this._flightBookingsRepository.Search(filter.passengerName, filter.date, filter.departureCity, filter.arrivalCity, filter.flightNumber);
            return result.Select(this._mapper.Map<FlightBookingViewModel>).ToList();
        }

        public async Task<List<AvailableViewModel>> CheckAvailability(CheckAvailabilityRequest request)
        {

            if (request.startDate < DateTime.Now)
            {
                throw new Exception($"Invalid start date {request.startDate}");
            }
            if (request.numberOfPax < 1)
            {
                throw new Exception($"Passenger count has to be atleast 1 {request.numberOfPax}");

            }

            var result = new List<AvailableViewModel>();
            for (DateTime date = request.startDate; date.Date <= request.endDate.Date; date = date.AddDays(1))
            {
                var dateValue = await this._flightBookingsRepository.CheckAvailability(date, request.numberOfPax);
                if(dateValue != null)
                {
                    result.Add(new AvailableViewModel { Date = date, Flights = this._mapper.Map<List<FlightViewModel>>(dateValue) });
                }
            }
            return result;
        }

        private async Task<bool> IsItStillAvailable(Guid flightId, DateTime giveDate, int numberOfPax)
        {
            return await this._flightBookingsRepository.IsItStillAvailable(flightId, giveDate, numberOfPax);
        }

        public async Task<bool> MakeBooking(FlightBookingViewModel bookingRequest)
        {
            if (bookingRequest == null)
            {
                throw new NullReferenceException($"Invalid request");
            }
            if (bookingRequest.FlightDate < DateTime.Now.Date)
            {
                throw new Exception($"Invalid Flight date {bookingRequest.FlightDate}");
            }
            if (bookingRequest.FlightId == Guid.Empty)
            {
                throw new Exception($"Flight Id cannot be empty {bookingRequest.FlightId}");
            }
            if (string.IsNullOrWhiteSpace(bookingRequest.PassengerName))
            {
                throw new Exception($"Passenger name cannot be empty {bookingRequest.PassengerName}");
            }
            var canBook = await this.IsItStillAvailable(bookingRequest.FlightId, bookingRequest.FlightDate, 1);
            if (!canBook)
            {
                throw new Exception($"No Seats available");
            }
            var bookingRequestDto = this._mapper.Map<FlightBookingsDTO>(bookingRequest);
            return await this._flightBookingsRepository.MakeBooking(bookingRequestDto);
        }
    }
}
