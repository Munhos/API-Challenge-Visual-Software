namespace WebApplication1.Models
{
    public class ApiResponse
    {
        public Pagination pagination { get; set; }
        public List<FlightDataModel> data { get; set; }
    }

    public class Pagination
    {
        public int limit { get; set; }
        public int offset { get; set; }
        public int count { get; set; }
        public int total { get; set; }
    }

    public class FlightDataModel
    {
        public string? flight_date { get; set; }         
        public string? flight_status { get; set; }       
        public Departure? departure { get; set; }
        public Arrival? arrival { get; set; }
    }

    public class Departure
    {
        public string? airport { get; set; }
        public string? timezone { get; set; }
    }

    public class Arrival
    {
        public string? airport { get; set; }
        public string? timezone { get; set; }
    }
}
