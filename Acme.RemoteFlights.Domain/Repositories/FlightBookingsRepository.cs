using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.RemoteFlights.Domain.Contracts;
using Acme.RemoteFlights.Domain.DTO;
using Acme.RemoteFlights.Domain.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Acme.RemoteFlights.Domain.Repositories
{
    public class FlightBookingsRepository : IFlightBookingsRepository
    {
        private readonly IMapper _mapper;
        private readonly FlightsDbContext _dbContext;
        public FlightBookingsRepository(FlightsDbContext dbContext, IMapper mapper)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public async Task<List<FlightBookingsDTO>> GetAll()
        {
            var result = await this._dbContext.FlightBookings.ToListAsync();
            return result.Select(this._mapper.Map<FlightBookingsDTO>).ToList();
        }

        public async Task<List<FlightBookingsDTO>> Search(string passengerName = null, DateTime? givenDate = null,
            string giveDepartureCity = null, string giveArrivalCity = null, string giveFlightNumber = null)
        {
            var result = await (from fb in this._dbContext.FlightBookings.Where(x => x.FlightDate >= DateTime.Now)
                                join f in this._dbContext.Flights on fb.FlightId equals f.Id
                                where fb.PassengerName.ToLower().Contains(passengerName) ||
                                        fb.FlightDate == (givenDate ?? DateTime.Now) ||
                                        f.ArrivalCity.ToLower().Contains(giveArrivalCity) ||
                                        f.DepartureCity.ToLower().Contains(giveDepartureCity) ||
                                        f.FlightNumber.ToLower().Contains(giveFlightNumber)
                                select new FlightBookingsDTO
                                {
                                    Id = fb.Id,
                                    FlightId = fb.FlightId,
                                    FlightDate = fb.FlightDate,
                                    Flight = this._mapper.Map<FlightDTO>(f),
                                    PassengerName = fb.PassengerName
                                })?.ToListAsync();
            return result;
        }

        public async Task<bool> IsItStillAvailable(Guid flightId, DateTime givenDate, int numberOfPax)
        {

            var result = await (from bookings in this._dbContext.FlightBookings.Where(fb => fb.FlightId == flightId && fb.FlightDate == givenDate)
                                group bookings by bookings.FlightId into bookingsGroup
                                select new
                                {
                                    FlightId = bookingsGroup.Key,
                                    Count = bookingsGroup.Count()
                                })
                                ?.SingleOrDefaultAsync();

            if (result != null)
            {
                var flight = await this._dbContext.Flights.Where(f => f.Id == result.FlightId).SingleAsync();
                return (result.Count < flight.Capacity);
            }
            return true;
        }

        public async Task<List<FlightDTO>> CheckAvailability(DateTime date, int numberOfPax)
        {


            var groupResult = await (from bookings in this._dbContext.FlightBookings.Where(fb => fb.FlightDate >= DateTime.Now.Date && fb.FlightDate == date)
                                group bookings by bookings.FlightId into bookingsGroup
                                select new { id = bookingsGroup.Key, Count = bookingsGroup.Count() })
                    ?.Distinct()
                    ?.ToListAsync();

            var flights = await this._dbContext.Flights.ToListAsync();
            var result = this._mapper.Map<List<FlightDTO>>(flights);

            if (groupResult != null && groupResult.Count() > 0)
            {
                result?.ForEach(f =>
                {
                    var x = groupResult.Where(r => r.id == f.Id).FirstOrDefault();
                    f.Capacity = (f.Capacity - (x?.Count ?? 0));
                });
            }
            return result.Where(r => (r.Capacity - numberOfPax) >= 0).ToList();

        }

        public async Task<bool> MakeBooking(FlightBookingsDTO bookingDto)
        {
            var booking = this._mapper.Map<FlightBooking>(bookingDto);
            this._dbContext.FlightBookings.Add(booking);

            return await this._dbContext.SaveChangesAsync() > 0;
        }
    }
}
