using System;
using System.Collections.Generic;
using System.Text;
using Acme.RemoteFlights.Domain.DTO;
using Acme.RemoteFlights.Domain.Models;
using AutoMapper;

namespace Acme.RemoteFlights.Domain
{
    public class DomainMappingProfile: Profile
    {
        public DomainMappingProfile()
        {
            this.CreateMap<Flight, FlightDTO>();
            this.CreateMap<FlightBooking, FlightBookingsDTO>();
            this.CreateMap<Passenger, PassengerDTO>();

            this.CreateMap<FlightDTO, Flight>();
            this.CreateMap<FlightBookingsDTO, FlightBooking>();
            this.CreateMap<PassengerDTO, Passenger>();
        }
    }
}
