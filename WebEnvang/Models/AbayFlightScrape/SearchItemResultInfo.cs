using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.AbayFlightScrape
{
    public class SearchItemResultInfo
    {
        public string AirlineName { get; set; }
        public string Airline
        {
            get
            {
                if (AirlineName == "VietNam Airlines")
                    return "VNA";
                else if (AirlineName == "JetStar")
                    return "JETSTAR";
                else if (AirlineName == "Vietjet Air")
                    return "VIETJET";
                else return "";
            }
        }
        public string TicketType { get; set; }
        public string FromCity { get; set; }
        public string FromCountry { get; set; }
        public string FromAirPort { get; set; }
        public string FromCityCode { get; set; }
        public string DepartureDate { get; set; }
        public string DepartureTime { get; set; }
        public string ToCity { get; set; }
        public string ToCountry { get; set; }
        public string ToAirPort { get; set; }
        public string ToCityCode { get; set; }
        public string ArrivalDate { get; set; }
        public string ArrivalTime { get; set; }
        public string FlightNo { get; set; }
        public int StopNo { get; set; }
        public int Adult { get; set; }
        public decimal AdultFare { get; set; }
        public decimal AdultCharge { get; set; }
        public decimal AdultPrice { get; set; }
        public int Child { get; set; }
        public decimal ChildFare { get; set; }
        public decimal ChildCharge { get; set; }
        public decimal ChildPrice { get; set; }
        public int Infant { get; set; }
        public decimal InfantFare { get; set; }
        public decimal InfantCharge { get; set; }
        public decimal InfantPrice { get; set; }
        public decimal TicketFare { get; set; }
        public decimal TicketPrice { get; set; }
    }
}