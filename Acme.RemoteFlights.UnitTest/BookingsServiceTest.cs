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
    public class BookingsServiceTestBase
    {
        protected Mock<IFlightBookingsRepository> repository;
        protected BookingsService service;

        [TestInitialize]
        public void BaseSetup()
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

        [TestClass]
        public class SearchBookingsTest : BookingsServiceTestBase
        {
            private List<FlightBookingsDTO> searchResult;
            public void MockResults(SearchViewModel searchObj)
            {
                var flightId = Guid.NewGuid();
                this.searchResult = new List<FlightBookingsDTO>();
                this.searchResult.Add(new FlightBookingsDTO
                {
                    Flight = new FlightDTO
                    {
                        Id = flightId,
                        FlightNumber = searchObj.flightNumber,
                        Capacity = 5,
                        DepartureCity = searchObj.departureCity,
                        ArrivalCity = searchObj.arrivalCity,
                        startTime = TimeSpan.FromHours(10),
                        EndTime = TimeSpan.FromHours(12),
                    },
                    FlightDate = searchObj.date.HasValue ? searchObj.date.Value : DateTime.MinValue,
                    FlightId = flightId,
                    Id = Guid.NewGuid(),
                    PassengerName = searchObj.passengerName
                });

                this.repository.Setup(r => r.Search(
                   It.IsAny<CancellationToken>(),
                   It.Is<string>(t => t == searchObj.passengerName),
                   It.Is<DateTime?>(t => t == searchObj.date),
                   It.Is<string>(t => t == searchObj.departureCity),
                   It.Is<string>(t => t == searchObj.arrivalCity),
                   It.Is<string>(t => t == searchObj.flightNumber)))
                 .ReturnsAsync(this.searchResult);
            }
            private SearchViewModel prepareInput(string flightNumber, string departureCity, string arrivalCity, DateTime? flightDate, string passengerName)
            {
                return new SearchViewModel
                {
                    passengerName = passengerName,
                    date = flightDate,
                    departureCity = departureCity,
                    arrivalCity = arrivalCity,
                    flightNumber = flightNumber
                };
            }

            [TestMethod]
            public async Task Test_Search_Happy()
            {
                //Arrange
                var searchObj = this.prepareInput("", "", "", DateTime.Now.Date, "Max");
                this.MockResults(searchObj);

                //Act
                var result = await this.service.Search(searchObj, CancellationToken.None);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(searchObj.passengerName, result[0].PassengerName);
                Assert.AreEqual(searchObj.date, result[0].FlightDate);
            }

            [TestMethod]
            public async Task Test_Search_With_Passenger_Name()
            {
                //Arrange
                var searchObj = this.prepareInput("", "", "", null, "Max");
                this.MockResults(searchObj);

                //Act
                var result = await this.service.Search(searchObj, CancellationToken.None);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(searchObj.passengerName, result[0].PassengerName);
            }

            [TestMethod]
            public async Task Test_Search_With_DepartureCity()
            {
                //Arrange
                var searchObj = this.prepareInput("", "Melbourne", "", null, "");
                this.MockResults(searchObj);

                //Act
                var result = await this.service.Search(searchObj, CancellationToken.None);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(searchObj.departureCity, result[0].Flight.DepartureCity);
            }

            [TestMethod]
            public async Task Test_Search_With_ArrivalCity()
            {
                //Arrange
                var searchObj = this.prepareInput("", "", "Sydney", null, "");
                this.MockResults(searchObj);

                //Act
                var result = await this.service.Search(searchObj, CancellationToken.None);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(searchObj.arrivalCity, result[0].Flight.ArrivalCity);
            }

            [TestMethod]
            public async Task Test_Search_With_FlightNumber()
            {
                //Arrange
                var searchObj = this.prepareInput("ARF1", "", "Sydney", null, "");
                this.MockResults(searchObj);

                //Act
                var result = await this.service.Search(searchObj, CancellationToken.None);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(searchObj.flightNumber, result[0].Flight.FlightNumber);
            }

            [TestMethod]
            public async Task Test_Search_With_FlightDate()
            {
                //Arrange
                var searchObj = this.prepareInput("", "", "", DateTime.Now.Date, "");
                this.MockResults(searchObj);

                //Act
                var result = await this.service.Search(searchObj, CancellationToken.None);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(searchObj.date, result[0].FlightDate);
            }
        }

        [TestClass]
        public class MakeBookingsServiceTest : BookingsServiceTestBase
        {
            private void MockRepository(Guid flightId, DateTime flightDate)
            {
                this.repository.Setup(r => r.MakeBooking(It.Is<FlightBookingsDTO>(t =>
                   t.FlightId == flightId
                   && t.FlightDate == flightDate), It.IsAny<CancellationToken>()))
                  .ReturnsAsync((bool)true);

                this.repository.Setup(r => r.IsItStillAvailable(It.Is<Guid>(t =>
                   t == flightId), It.Is<DateTime>(u => u == flightDate), 1, It.IsAny<CancellationToken>()))
                  .ReturnsAsync((bool)true);
            }

            private FlightBookingRequest prepareInput(Guid flightId, DateTime flightDate, string passengerName)
            {
                return new FlightBookingRequest
                {
                    FlightId = flightId,
                    FlightDate = flightDate,
                    PassengerName = passengerName
                };
            }

            [TestMethod]
            public async Task Test_MakeBooking_Happy()
            {
                //Arrange
                var booking = new FlightBookingRequest
                {
                    FlightId = Guid.NewGuid(),
                    FlightDate = DateTime.Now.Date,
                    PassengerName = "Max"
                };
                this.MockRepository(booking.FlightId, booking.FlightDate);

                //Act
                var result = await this.service.MakeBooking(booking, CancellationToken.None);

                //Assert
                Assert.IsTrue(result);

            }

            [TestMethod]
            public async Task Test_MakeBooking_With_NegativeFlightDate()
            {
                //Arrange
                var booking = this.prepareInput(Guid.NewGuid(), DateTime.Now.AddDays(-1), "Max");
                this.MockRepository(booking.FlightId, booking.FlightDate);

                //Act
                var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await this.service.MakeBooking(booking, CancellationToken.None));

                //Assert
                Assert.IsNotNull(ex);
                Assert.IsTrue(ex.Message.Contains($"Invalid Flight date {booking.FlightDate}"));

            }

            [TestMethod]
            public async Task Test_MakeBooking_With_EmptyGuid()
            {
                //Arrange
                var booking = this.prepareInput(Guid.Empty, DateTime.Now.Date, "Max");
                this.MockRepository(booking.FlightId, booking.FlightDate);

                //Act
                var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await this.service.MakeBooking(booking, CancellationToken.None));

                //Assert
                Assert.IsNotNull(ex);
                Assert.IsTrue(ex.Message.Contains($"Flight Id cannot be empty {Guid.Empty}"));

            }

            [TestMethod]
            public async Task Test_MakeBooking_With_EmptyPassenger()
            {
                //Arrange
                var booking = this.prepareInput(Guid.NewGuid(), DateTime.Now.Date, "");
                this.MockRepository(booking.FlightId, booking.FlightDate);

                //Act
                var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await this.service.MakeBooking(booking, CancellationToken.None));

                //Assert
                Assert.IsNotNull(ex);
                Assert.IsTrue(ex.Message.Contains($"Passenger name cannot be empty {booking.PassengerName}"));

            }

        }
    }
}
