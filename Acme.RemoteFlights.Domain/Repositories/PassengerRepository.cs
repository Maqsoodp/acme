using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.RemoteFlights.Domain.DTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Acme.RemoteFlights.Domain.Contracts
{
    public class PassengerRepository: IPassengerRepository
    {
        private readonly IMapper _mapper;
        private readonly FlightsDbContext _dbContext;
        public PassengerRepository(FlightsDbContext dbContext, IMapper mapper)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public async Task<List<PassengerDTO>> GetAll()
        {
            var result = await this._dbContext.Passengers.ToListAsync();
            return result.Select(this._mapper.Map<PassengerDTO>).ToList();
        }
    }
}
