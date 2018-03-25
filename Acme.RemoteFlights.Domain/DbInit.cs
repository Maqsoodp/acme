using System;
using System.Linq;
using Acme.RemoteFlights.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Acme.RemoteFlights.Domain
{
    public class DbInit
    {

        private readonly FlightsDbContext _dbContext;
        public DbInit(FlightsDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public void Load()
        {
            this._dbContext.Database.EnsureCreated();

            if (this._dbContext.Flights.Any())
            {
                return;
            }

            var flights = new Flight[]
            {
                new Flight{FlightNumber="ARF1",  Capacity = 5,  DepartureCity="Melbourne",    ArrivalCity="Sydney",   startTime=DateTime.Parse("2005-09-01 10:00:00"),  EndTime=DateTime.Parse("2005-09-01 11:00:00")},
                new Flight{FlightNumber="ARF2",  Capacity = 5,  DepartureCity="Melbourne",    ArrivalCity="Perth",    startTime=DateTime.Parse("2002-09-01 10:00:00"),  EndTime=DateTime.Parse("2002-09-01 12:00:00")},
                new Flight{FlightNumber="ARF3",  Capacity = 5,  DepartureCity="Melbourne",    ArrivalCity="Brisbane", startTime=DateTime.Parse("2003-09-01 12:00:00"),  EndTime=DateTime.Parse("2003-09-01 14:00:00")},
                new Flight{FlightNumber="ARF4",  Capacity = 5,  DepartureCity="Melbourne",    ArrivalCity="Hobart",   startTime=DateTime.Parse("2002-09-01 13:00:00"),  EndTime=DateTime.Parse("2002-09-01 15:00:00")},
            };
            foreach (Flight s in flights)
            {
                this._dbContext.Flights.Add(s);
            }
            this._dbContext.SaveChanges();
        }
    }
}
