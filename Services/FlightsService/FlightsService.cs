using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services.FlightsController
{
    public class FlightsService
    {
        private readonly HttpClient _client;
        private readonly AppDbContext _context;

        public FlightsService(AppDbContext context, HttpClient client)
        {
            _client = client;
            _context = context;
        }

        public async Task<List<MonitoredFlightsModel>?> GetAllFlightsExternalApiData()
        {
            try
            {
                await CopyDataExternalToDataInternal();
                return await _context.MonitoredFlights.ToListAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<ActionResult> GetOneFlightsExternalApiData(int id)
        {
            try
            {
                var response = await _context.MonitoredFlights.SingleOrDefaultAsync(x => x.Id == id);
                return new OkObjectResult(response);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<List<MonitoredFlightsModel>> GetAllFlightInternalData()
        {
            try
            {
                var response = await _context.FlyLinked.ToListAsync();
                var monitoredFlights = new List<MonitoredFlightsModel>();

                foreach (var e in response)
                {
                    var response2 = await GetOneFlightsExternalApiData(int.Parse(e.Flight_Id)) as OkObjectResult;

                    if (response2?.Value is MonitoredFlightsModel flightData)
                    {
                        var monitoredFlight = new MonitoredFlightsModel
                        {
                            Id = flightData.Id,
                            Flight_Date = flightData.Flight_Date,
                            Flight_Status = flightData.Flight_Status,
                            AirportDeparture_Name = flightData.AirportDeparture_Name,
                            AirportDeparture_TimeZone = flightData.AirportDeparture_TimeZone,
                            AirportArrival_Name = flightData.AirportArrival_Name,
                            AirportArrival_TimeZone = flightData.AirportArrival_TimeZone
                        };

                        monitoredFlights.Add(monitoredFlight);
                    }
                }

                return monitoredFlights;
            }
            catch
            {
                return new List<MonitoredFlightsModel>();
            }
        }

        public async Task CopyDataExternalToDataInternal()
        {
            try
            {
                if (await _context.MonitoredFlights.AnyAsync())
                    return;

                var response = await _client.GetFromJsonAsync<ApiResponse>(
                    "https://api.aviationstack.com/v1/flights?access_key=97e6d280fe594a34cd0afdd274887129"
                );

                if (response?.data != null && response.data.Any())
                {
                    var first100 = response.data.Take(100).ToList();

                    foreach (var e in first100)
                    {
                        var monitoredFlight = new MonitoredFlightsModel
                        {
                            Flight_Date = e.flight_date,
                            Flight_Status = e.flight_status,
                            AirportDeparture_Name = e.departure?.airport ?? "N/A",
                            AirportDeparture_TimeZone = e.departure?.timezone ?? "N/A",
                            AirportArrival_Name = e.arrival?.airport ?? "N/A",
                            AirportArrival_TimeZone = e.arrival?.timezone ?? "N/A"
                        };

                        _context.MonitoredFlights.Add(monitoredFlight);
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
            }
        }

        public async Task<ActionResult> CreateRegisterMonitoredFlyght(FlyghtLinkedInUserModel model)
        {
            try
            {
                if (model == null)
                {
                    return new BadRequestResult();
                }

                var response = await GetOneFlightsExternalApiData(int.Parse(model.Flight_Id)) as OkObjectResult;
                if (response == null)
                {
                    return new BadRequestResult();
                }

                if (!_context.MonitoredFlights.Any())
                {
                    await CopyDataExternalToDataInternal();
                }

                _context.FlyLinked.Add(model);
                await _context.SaveChangesAsync();

                return new OkObjectResult(model);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult> DeleteRegisterLinkedInUserFlyghts(int id)
        {
            try
            {
                var flyght = await _context.FlyLinked.SingleOrDefaultAsync(e => e.Flight_Id == id.ToString());

                if (flyght == null)
                {
                    return new NotFoundResult();
                }

                _context.FlyLinked.Remove(flyght);
                await _context.SaveChangesAsync();

                return new OkObjectResult(flyght);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult> PatchLinkedInUserFlyghts(FlyghtLinkedInUserModel model, int id)
        {
            try
            {
                var dataToModify = await _context.FlyLinked.SingleOrDefaultAsync(e => e.Flight_Id == id.ToString());

                if (dataToModify == null)
                {
                    return new NotFoundResult();
                }

                dataToModify.Flight_Id = model.Flight_Id;

                _context.FlyLinked.Update(dataToModify);
                await _context.SaveChangesAsync();

                return new OkObjectResult(dataToModify);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
