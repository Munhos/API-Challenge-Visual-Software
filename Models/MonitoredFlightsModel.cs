namespace WebApplication1.Models
{
    public class MonitoredFlightsModel
    {
        public int Id { get; set; }

        public string? Flight_Date { get; set; }
        public string? Flight_Status { get; set; }

        public string? AirportDeparture_Name { get; set; }
        public string? AirportDeparture_TimeZone { get; set; }

        public string? AirportArrival_Name { get; set; }
        public string? AirportArrival_TimeZone { get; set; }
    }
}
