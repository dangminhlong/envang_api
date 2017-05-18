using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.BookApi
{
    public class BookingFlightInfo
    {
        public string Airline { get; set; }
        public string FlightNo { get; set; }
        public decimal TicketPrice { get; set; }
        public decimal TicketFare { get; set; }
        public string TicketType { get; set; }
        public string FromCity { get; set; }
        public string FromCityCode { get; set; }
        public string ToCity { get; set; }
        public string ToCityCode { get; set; }
        public string DepartureDate { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalDate { get; set; }
        public string ArrivalTime { get; set; }

    }
}