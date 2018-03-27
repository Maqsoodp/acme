using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acme.RemoteFlights.Domain.DTO;

namespace Acme.RemoteFlights.Domain.Contracts
{
    public interface IFlightsRepository
    {
        Task<List<FlightDTO>> GetAll(CancellationToken cancellationToken);
    }
}
