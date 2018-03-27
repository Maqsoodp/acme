using System;
using System.Collections.Generic;
using System.Text;
using Acme.RemoteFlights.Application.ViewModel;
using Acme.RemoteFlights.Domain.DTO;
using AutoMapper;

namespace Acme.RemoteFlights.Application
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {

            this.CreateMap<FlightBookingRequest, FlightBookingsDTO>()
                .ForMember(m => m.Flight, o => o.Ignore());
            //this.CreateMap<FlightBookingsViewModel, FlightBookingsDTO>();
            //this.CreateMap<FlightViewModel, FlightDTO>();
            //this.CreateMap<FlightBookingsViewModel, FlightBookingsDTO>();

            this.CreateMap<FlightDTO, FlightViewModel>();
            this.CreateMap<FlightBookingsDTO, FlightBookingViewModel>();
        }
    }
}
