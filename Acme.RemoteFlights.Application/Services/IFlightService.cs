using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Acme.RemoteFlights.Application.ViewModel;

namespace Acme.RemoteFlights.Application.Services
{
    public interface IFlightService
    {
        Task<List<FlightViewModel>> GetAll();
        
    }
}
