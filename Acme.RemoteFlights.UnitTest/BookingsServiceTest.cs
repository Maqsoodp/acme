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
    public class BookingsServiceTest
    {
        private Mock<IFlightBookingsRepository> repository;
        private BookingsService service;
        public BookingsServiceTest()
        {
            this.repository = new Mock<IFlightBookingsRepository>();
            var mapper = CreateMapper();
            this.service = new BookingsService(repository.Object, mapper);
        }

        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<ApplicationMappingProfile>(); });
            return config.CreateMapper();
        }

        private void prepare(Guid flightId, DateTime flightDate)
        {
            this.repository.Setup(r => r.MakeBooking(It.Is<FlightBookingsDTO>(t =>
               t.FlightId == flightId
               && t.FlightDate == flightDate)))
              .ReturnsAsync((bool)true);

            this.repository.Setup(r => r.IsItStillAvailable(It.Is<Guid>(t =>
               t == flightId), It.Is<DateTime>(u => u == flightDate), 1))
              .ReturnsAsync((bool)true);
        }

        [TestMethod]
        public async Task Test_MakeBooking_Happy()
        {
            //Arrange
            var booking = new FlightBookingViewModel
            {
                FlightId = Guid.NewGuid(),
                FlightDate = DateTime.Now, ///.AddMinutes(5),
                PassengerName = "Max"
            };
            this.prepare(booking.FlightId, booking.FlightDate);

            //Act
            var result = await this.service.MakeBooking(booking);

            //Assert
            Assert.IsTrue(result);

        }


        [TestMethod]
        public async Task Test_MakeBooking_With_InvalidDate()
        {
            //Arrange
            var booking = new FlightBookingViewModel
            {
                FlightId = Guid.NewGuid(),
                FlightDate = DateTime.Now.AddDays(-1),
                PassengerName = "Max"
            };
            this.prepare(booking.FlightId, booking.FlightDate);

            //Act
            var ex = await Assert.ThrowsExceptionAsync
                    <Exception>(
                        async () => await this.service.MakeBooking(booking)
                    );
            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.Message, $"Invalid Flight date {booking.FlightDate}");

        }

        [TestMethod]
        public async Task Test_MakeBooking_With_EmptyGuid()
        {
            //Arrange
            var booking = new FlightBookingViewModel
            {
                FlightId = Guid.Empty,
                FlightDate = DateTime.Now.AddDays(1),
                PassengerName = "Max"
            };
            this.prepare(booking.FlightId, booking.FlightDate);

            //Act

            var ex = await Assert.ThrowsExceptionAsync
                    <Exception>(
                        async () => await this.service.MakeBooking(booking)
                    );
            //Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.Message, $"Flight Id cannot be empty {booking.FlightId}");

        }

        [TestMethod]
        public async Task Test_Search_Happy()
        {
            //Arrange
            var searchObj = new SearchViewModel
            {
                passengerName = "Max",
                date = null,
                departureCity = "",
                arrivalCity = "",
                flightNumber = ""
            };

            var searchResult = new List<FlightBookingsDTO>();
            searchResult.Add(new FlightBookingsDTO
            {
                Flight = new FlightDTO { },
                FlightDate = DateTime.Now,
                FlightId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                PassengerName = "Max"
            });

            this.repository.Setup(r => r.Search(
                It.Is<string>(t => t == searchObj.passengerName),
                It.Is<DateTime?>(t => t == searchObj.date),
                It.Is<string>(t => t == searchObj.departureCity),
                It.Is<string>(t => t == searchObj.arrivalCity),
                It.Is<string>(t => t == searchObj.flightNumber)))
              .ReturnsAsync(searchResult);

            //Act
            var result = await this.service.Search(searchObj);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }
    }
}
