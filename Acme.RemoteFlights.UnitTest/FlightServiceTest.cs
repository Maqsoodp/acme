using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.RemoteFlights.Application;
using Acme.RemoteFlights.Application.Services;
using Acme.RemoteFlights.Application.ViewModel;
using Acme.RemoteFlights.Domain.Contracts;
using Acme.RemoteFlights.Domain.DTO;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Acme.RemoteFlights.UnitTest
{
    [TestClass]
    public class FlightServiceTest
    {

        private Mock<IFlightsRepository> repository;
        private FlightService service;
        public FlightServiceTest()
        {
            this.repository = new Mock<IFlightsRepository>();
            var mapper = CreateMapper();
            this.service = new FlightService(repository.Object, mapper);
        }

        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<ApplicationMappingProfile>(); });
            return config.CreateMapper();
        }


        
    }
}
