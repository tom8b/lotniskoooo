using System;

namespace CvCreator.Api.Model
{
    public class Connection
    {
        public int ConnectionId { get; set; }
        public string Price { get; set; }
        public Airport Starting_airport { get; set; }
        public Airport Destination_airport { get; set; }
        public DateTime Flight_date { get; set; }
    }
}
