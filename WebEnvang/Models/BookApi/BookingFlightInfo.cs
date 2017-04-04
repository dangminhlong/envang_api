using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.BookApi
{
    public class BookingFlightInfo
    {
        public string Brand { get; set; }
        public string FlightNumber { get; set; }
        public string TicketPrice { get; set; }
        public string TotalPrice { get; set; }
        public string TicketType { get; set; }
        public int FromPlaceId { get; set; }
        public int ToPlaceId { get; set; }
        public string FareBasis { get; set; }
        public string DepartTime { get; set; }
        public string LandingTime { get; set; }

    }
}