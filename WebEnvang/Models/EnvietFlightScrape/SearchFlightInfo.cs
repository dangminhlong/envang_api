using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.EnvietFlightScrape
{
    public class SearchFlightInfo
    {
        public string Airline { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public string DepartureDate { get; set; }
        public string ArrivalDate { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public string FlightNo { get; set; }
        public int StopNo { get; set; }
        public decimal AdultFare { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal AdultCharge { get; set; }
        public decimal ChildFare { get; set; }
        public decimal ChildPrice { get; set; }
        public decimal ChildCharge { get; set; }
        public decimal InfantFare { get; set; }
        public decimal InfantPrice { get; set; }
        public decimal InfantCharge { get; set; }
        public string SeatClass { get; set; }
        public decimal AdultServiceFee { get; set; }
        public decimal ChildServiceFee { get; set; }
        public decimal InfantServiceFee { get; set; }
        public decimal TicketFare { get; set; }
        public decimal TicketPrice { get; set; }
    }
}