using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acme.RemoteFlights.Domain.Contracts;
using Acme.RemoteFlights.Domain.DTO;
using Acme.RemoteFlights.Domain.Models;
using Acme.RemoteFlights.Shared;
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

        public async Task<List<FlightBookingsDTO>> GetAll(CancellationToken cancellationToken)
        {
            return await SqlRetryPolicy.BasicPolicy.ExecuteAsync(async () =>
            {
                var result = await this._dbContext.FlightBookings.ToListAsync(cancellationToken);
                return result.Select(this._mapper.Map<FlightBookingsDTO>).ToList();
            });
        }

        public async Task<List<FlightBookingsDTO>> Search(CancellationToken cancellationToken, string passengerName = null, DateTime? givenDate = null,
            string giveDepartureCity = null, string giveArrivalCity = null, string giveFlightNumber = null)
        {
            return await SqlRetryPolicy.BasicPolicy.ExecuteAsync(async() =>
            {
                return await (from fb in this._dbContext.FlightBookings.Where(x => x.FlightDate >= DateTime.Now.Date)
                              join f in this._dbContext.Flights on fb.FlightId equals f.Id
                              where fb.PassengerName.Contains(passengerName, StringComparison.InvariantCultureIgnoreCase) ||
                                      fb.FlightDate == (givenDate ?? DateTime.Now.Date) ||
                                      f.ArrivalCity.Contains(giveArrivalCity, StringComparison.InvariantCultureIgnoreCase) ||
                                      f.DepartureCity.Contains(giveDepartureCity, StringComparison.InvariantCultureIgnoreCase) ||
                                      f.FlightNumber.Contains(giveFlightNumber, StringComparison.InvariantCultureIgnoreCase)
                              select new FlightBookingsDTO
                              {
                                  Id = fb.Id,
                                  FlightId = fb.FlightId,
                                  FlightDate = fb.FlightDate,
                                  Flight = this._mapper.Map<FlightDTO>(f),
                                  PassengerName = fb.PassengerName
                              })?.ToListAsync(cancellationToken);

            });
            
        }

        public async Task<bool> IsItStillAvailable(Guid flightId, DateTime givenDate, int numberOfPax, CancellationToken cancellationToken)
        {
            return await SqlRetryPolicy.BasicPolicy.ExecuteAsync(async () =>
            {
                var currentBooking = await (from bookings in this._dbContext.FlightBookings.Where(fb => fb.FlightId == flightId && fb.FlightDate == givenDate)
                                            group bookings by bookings.FlightId into bookingsGroup
                                            select new
                                            {
                                                FlightId = bookingsGroup.Key,
                                                Count = bookingsGroup.Count()
                                            })
                                ?.SingleOrDefaultAsync(cancellationToken);

                if (currentBooking != null)
                {
                    var flight = await this._dbContext.Flights.Where(f => f.Id == currentBooking.FlightId).SingleAsync(cancellationToken);
                    return ((flight.Capacity - ((currentBooking?.Count ?? 0) + numberOfPax)) >= 0);
                }
                return true;
            });
        }

        public async Task<List<FlightDTO>> CheckAvailability(DateTime date, int numberOfPax, CancellationToken cancellationToken)
        {

            return await SqlRetryPolicy.BasicPolicy.ExecuteAsync(async () =>
            {
                var currentBookings = await (from bookings in this._dbContext.FlightBookings.Where(fb => fb.FlightDate >= DateTime.Now.Date && fb.FlightDate == date)
                                             group bookings by bookings.FlightId into bookingsGroup
                                             select new { id = bookingsGroup.Key, Count = bookingsGroup.Count() })
                    ?.Distinct()
                    ?.ToListAsync(cancellationToken);

                var flights = await this._dbContext.Flights.ToListAsync(cancellationToken);
                var result = this._mapper.Map<List<FlightDTO>>(flights);

                if (currentBookings != null && currentBookings.Count() > 0)
                {
                    result?.ForEach(f =>
                    {
                        var x = currentBookings.Where(r => r.id == f.Id).FirstOrDefault();
                        f.Capacity = (f.Capacity - (x?.Count ?? 0));
                    });
                }
                return result.Where(r => (r.Capacity - numberOfPax) >= 0).ToList();
            });
        }

        public async Task<bool> MakeBooking(FlightBookingsDTO bookingDto, CancellationToken cancellationToken)
        {
            var booking = this._mapper.Map<FlightBooking>(bookingDto);

            return await SqlRetryPolicy.BasicPolicy.ExecuteAsync(async () =>
            {
                this._dbContext.FlightBookings.Add(booking);
                return await this._dbContext.SaveChangesAsync(cancellationToken) > 0;
            });
        }
    }
}
