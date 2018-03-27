using System;
using System.Collections.Generic;
using System.Threading;
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

        [TestMethod]
        public async Task Test_GetFlights()
        {
            var flights = new List<FlightDTO> {
                new FlightDTO
                {
                    Id =Guid.NewGuid(),
                    ArrivalCity = "Sydney",
                    DepartureCity = "Melbourne",
                    Capacity = 5,
                    startTime = TimeSpan.FromHours(10),
                    EndTime = TimeSpan.FromHours(12),
                    FlightNumber = "ARF1"
                }
            };

            //Arrange
            this.repository.Setup(r => r.GetAll(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(flights);

            //Act
            var result = await this.service.GetAll(CancellationToken.None);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(flights[0].FlightNumber, result[0].FlightNumber);
            Assert.AreEqual(flights[0].DepartureCity, result[0].DepartureCity);
            Assert.AreEqual(flights[0].ArrivalCity, result[0].ArrivalCity);
            Assert.AreEqual(flights[0].Capacity, result[0].Capacity);
            Assert.AreEqual(flights[0].startTime, result[0].startTime);
            Assert.AreEqual(flights[0].EndTime, result[0].EndTime);
        }

    }
}
