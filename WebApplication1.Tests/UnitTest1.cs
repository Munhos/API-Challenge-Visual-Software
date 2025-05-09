using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication1.Services.FlightsController;
using Xunit;

namespace WebApplication1.Tests
{
    public class MonitoredFlightControllerTests
    {
        [Fact]
        public async Task ShowAllFlights_ReturnsListOfFlights_WhenFlightsExist()
        {
            var mockService = new Mock<FlightsService>(null);
            var expectedFlights = new List<MonitoredFlightsModel>
            {
                new MonitoredFlightsModel
                {
                    Id = 1,
                    Flight_Date = "2024-01-01",
                    Flight_Status = "active",
                    AirportDeparture_Name = "JFK",
                    AirportDeparture_TimeZone = "America/New_York",
                    AirportArrival_Name = "LAX",
                    AirportArrival_TimeZone = "America/Los_Angeles"
                }
            };

            mockService.Setup(service => service.GetAllFlightsExternalApiData())
                       .ReturnsAsync(expectedFlights);

            var controller = new MonitoredFlightController(mockService.Object);

            var result = await controller.ShowAllFlights();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedFlights.First().Flight_Date, result.First().Flight_Date);
        }

        [Fact]
        public async Task ShowAllFlights_ReturnsEmptyList_WhenNoFlightsExist()
        {
            var mockService = new Mock<FlightsService>(null);
            mockService.Setup(service => service.GetAllFlightsExternalApiData())
                       .ReturnsAsync(new List<MonitoredFlightsModel>());

            var controller = new MonitoredFlightController(mockService.Object);

            var result = await controller.ShowAllFlights();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ShowAllFlights_ReturnsEmptyList_WhenServiceReturnsNull()
        {
            var mockService = new Mock<FlightsService>(null);
            mockService.Setup(service => service.GetAllFlightsExternalApiData())
                       .ReturnsAsync((List<MonitoredFlightsModel>)null); 

            var controller = new MonitoredFlightController(mockService.Object);

            var result = await controller.ShowAllFlights();

            Assert.NotNull(result); 
            Assert.Empty(result);
        }

        [Fact]
        public async Task ShowAllFlights_ThrowsException_WhenServiceFails()
        {
            var mockService = new Mock<FlightsService>(null);
            mockService.Setup(service => service.GetAllFlightsExternalApiData())
                       .ThrowsAsync(new System.Exception("Erro ao obter dados."));

            var controller = new MonitoredFlightController(mockService.Object);

            await Assert.ThrowsAsync<System.Exception>(() => controller.ShowAllFlights());
        }
    }
}
