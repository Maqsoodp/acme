using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acme.RemoteFlights.Domain.Contracts;
using Acme.RemoteFlights.Domain.DTO;
using Acme.RemoteFlights.Domain.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Acme.RemoteFlights.Domain.Repositories
{
    public class FlightsRepository : IFlightsRepository
    {
        private readonly IMapper _mapper;
        private readonly FlightsDbContext _dbContext;
        public FlightsRepository(FlightsDbContext dbContext, IMapper mapper)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public async Task<List<FlightDTO>> GetAll(CancellationToken cancellationToken)
        {
            return await SqlRetryPolicy.BasicPolicy.ExecuteAsync(async () =>
            {
                var result = await this._dbContext.Flights.ToListAsync(cancellationToken);
                return result.Select(this._mapper.Map<FlightDTO>).ToList();
            });
        }

    }
}
