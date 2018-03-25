using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.RemoteFlights.Application.ViewModel;
using Acme.RemoteFlights.Domain.Contracts;
using AutoMapper;

namespace Acme.RemoteFlights.Application.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightsRepository _flightsRepository;
        private readonly IMapper _mapper;
        public FlightService(IFlightsRepository flightsRepository, IMapper mapper)
        {
            this._flightsRepository = flightsRepository;
            this._mapper = mapper;
        }

        public async Task<List<FlightViewModel>> GetAll()
        {
            var result = await this._flightsRepository.GetAll();
            return result.Select(this._mapper.Map<FlightViewModel>).ToList();
        }
        
    }
}
